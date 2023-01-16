using Microsoft.AspNetCore.Http;
using NAudio.Wave;
using Proact.Services.AuthorizationPolicies;
using Proact.Services.AzureMediaServices;
using Proact.Services.Entities;
using Proact.Services.EntitiesMapper;
using Proact.Services.Messages;
using Proact.Services.Models;
using Proact.Services.Models.Messages;
using Proact.Services.PushNotifications;
using Proact.Services.QueriesServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Proact.Services {
    public class MessageAttachmentManagerService : IMessageAttachmentManagerService {
        private readonly IFilesStorageService _mediaStorageService;
        private readonly IMessageEditorService _messageEditorService;
        private readonly IMessagesQueriesService _messagesQueriesService;
        private readonly IMessageNotifierService _messageNotifierService;
        private readonly ProactDatabaseContext _database;
        private readonly IMediaFilesUploaderService _mediaFilesUploaderService;

        public MessageAttachmentManagerService(
            IMediaFilesUploaderService mediaFilesUploaderService,
            IFilesStorageService mediaStorageService,
            IMessageEditorService messageEditorService, IMessagesQueriesService messagesQueriesService,
            IMessageNotifierService messageNotifierService, ProactDatabaseContext database ) {
            _mediaStorageService = mediaStorageService;
            _messageEditorService = messageEditorService;
            _messagesQueriesService = messagesQueriesService;
            _messageNotifierService = messageNotifierService;
            _database = database;
            _mediaFilesUploaderService = mediaFilesUploaderService;
        }

        public Task<MediaFileSecurityInfosModel> GetAttachmentUrlWithSASToken( MessageAttachment attachment ) {
            return _mediaFilesUploaderService.GetMediaFileSecurityInfos( attachment );
        }

        public async Task AttachVideoFileFromTempFolderToMessage( Guid messageId ) {
            var message = _messagesQueriesService.GetMessage( messageId );

            string fileFromTempFolderPath = Path.Combine( MediaFileUploaderNamingResolver
                .GetPathForTempMediaFiles( message.MessageAttachment.FileName ) );

            var fileStream = File.OpenRead( fileFromTempFolderPath );
            var result = await _mediaFilesUploaderService.UploadMediaFileOnStorage(
               (Guid)message.AuthorId, fileStream, AttachmentType.VIDEO );

            var messageModel = MessagesEntityMapper.Map( message );
            CreateFinalAttachmentInfoOnDatabaseForMessage( result, message, AttachmentType.VIDEO );
            await AlertUserForAttachmentReady( messageModel );
            await AlertOtherPartecipantsForMessageReady( messageModel );

            fileStream.Close();
            DeleteFile( fileFromTempFolderPath );
        }

        public async Task<MediaUploadedResultModel> AttachAudioFileFromTempFolderToMessage( 
            Message message, CreateAttachMediaFileRequest request ) {            
            
            string completePath = Path.Combine( MediaFileUploaderNamingResolver
                .GetPathForTempMediaFiles( request.FileName ) );
            var fileStream = File.Open( completePath, FileMode.Open );
            
            var mediaFileInfos = MediaFileUploaderNamingResolver
                .CreateMediaFileNamingForAudio( (Guid)message.AuthorId );
            mediaFileInfos.FileName = MediaFileUploaderNamingResolver
                .GetFileNameForAudio( mediaFileInfos.Uniqueness, request.Extension );

            var audioUploadResult = await _mediaStorageService.UploadFileDirectlyIntoContainer(
                fileStream, AccessFolderType.PRIVATE, request.ContentType,
                mediaFileInfos.ContainerName, mediaFileInfos.FileName );

            fileStream.Close();

            audioUploadResult.DurationInMilliseconds = GetMillisecondsFromAudioFile( completePath );
            audioUploadResult.AssetId = mediaFileInfos.Uniqueness.ToString();
            CreateFinalAttachmentInfoOnDatabaseForMessage( audioUploadResult, message, AttachmentType.AUDIO );

            DeleteFile( completePath );
            return audioUploadResult;
        }

        public async Task<MediaUploadedResultModel> AttachImageFileToMessage(
            Message message, IFormFile mediaFile ) {
            var mediaFileInfos = MediaFileUploaderNamingResolver
                .CreateMediaFileNamingForImage( (Guid)message.AuthorId );

            var imageUploadResult = await _mediaStorageService.UploadMediaFile(
                mediaFile.OpenReadStream(), AccessFolderType.PUBLIC, mediaFileInfos );

            CreateFinalAttachmentInfoOnDatabaseForMessage( imageUploadResult, message, AttachmentType.IMAGE );
            return imageUploadResult;
        }

        public async Task UploadMediaFileOnTempFolder( IFormFile file, Message message, AttachmentType type ) {
            string completePath = Path.Combine( MediaFileUploaderNamingResolver
                .GetPathForTempMediaFiles( file.FileName ) );

            using ( Stream fileStream = new FileStream( completePath, FileMode.Create ) ) {
                await file.OpenReadStream().CopyToAsync( fileStream );
            }

            CreatePreloadingAttachmentInfoOnDatabaseForMessage( file, message, type );
        }

        private void DeleteFile( string path ) {
            File.Delete( path );
        }

        private void CreatePreloadingAttachmentInfoOnDatabaseForMessage( 
            IFormFile file, Message message, AttachmentType attachmentType ) {
            var attachmentParams = new MessageAttachmentCreationParams() {
                AttachmentStatus = MessageContentStatusEnum.NotUploaded,
                AttachmentType = attachmentType,
                Message = message,
                MediaUploadResult = new MediaUploadedResultModel() {
                    FileName = Path.GetFileName( file.FileName )
                }
            };

            _messageEditorService.SetAttachmentPropertiesToMessage( attachmentParams, (Guid)message.AuthorId );
            _database.SaveChangesWithEntityTracking( (Guid)message.AuthorId );
        }

        private void CreateFinalAttachmentInfoOnDatabaseForMessage(
            MediaUploadedResultModel mediaUploadedResult, Message message, AttachmentType attachmentType ) {
            var attachmentParams = new MessageAttachmentCreationParams() {
                AttachmentType = attachmentType,
                MediaUploadResult = mediaUploadedResult,
                Message = message,
            };

            attachmentParams.AttachmentStatus 
                = mediaUploadedResult.UploadOk ? MessageContentStatusEnum.Ready 
                    : attachmentParams.AttachmentStatus = MessageContentStatusEnum.Failed;

            _messageEditorService.SetAttachmentPropertiesToMessage( attachmentParams, (Guid)message.AuthorId );
            _messagesQueriesService.SetAsVisibile( message.Id );
            _database.SaveChangesWithEntityTracking( (Guid)message.AuthorId );
        }

        private async Task AlertUserForAttachmentReady( MessageModel message ) {
            await _messageNotifierService.PerformAttachmentReadyForMessage( message );
        }

        private async Task AlertOtherPartecipantsForMessageReady( MessageModel message ) {
            if ( message.IsOriginalMessage ) {
                await _messageNotifierService.PerformPatientCreateNewTopic( (Guid)message.AuthorId, message );
            }
            else {
                await _messageNotifierService.PerformUserRepliedToMessage( (Guid)message.AuthorId, message );
            }
        }

        private int GetMillisecondsFromAudioFile( string completePath ) {
            var waveReader = new WaveFileReader( completePath );
            int duration = (int)waveReader.TotalTime.TotalMilliseconds;
            waveReader.Close();
            return duration;
        }
    }
}
