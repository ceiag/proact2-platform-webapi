using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Proact.Services.Entities;
using Proact.Services.Messages;
using Proact.Services.Models;
using Proact.Services.Models.Messages;
using Proact.Services.QueriesServices;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Proact.Services.AzureMediaServices {
    public class MessageMediaFileCreatorService : IMessageMediaFileCreatorService {
        private IMediaFilesUploaderService _mediaFilesUploaderService;
        private IMessageEditorService _messageEditorService;
        private IFilesStorageService _mediaStorageService;
        private ProactDatabaseContext _database;

        private const string _tempStorageNamePrefix = "media-temp-";

        public MessageMediaFileCreatorService( 
            ProactDatabaseContext database,
            IMessageEditorService messageCreatorService, IFilesStorageService mediaStorageService ) {
            _database = database;
            _messageEditorService = messageCreatorService;
            _mediaStorageService = mediaStorageService;
        }

        public void SetMediaUploaderMethod( IMediaFilesUploaderService mediaFilesUploaderService ) {
            _mediaFilesUploaderService = mediaFilesUploaderService;
        }

        private string GetTempStorageName( Guid messageId ) {
            return $"{_tempStorageNamePrefix}queue";
        }

        public async Task<MediaUploadedResultModel> UploadMediaFileForAsyncEncoding( 
            Guid messageId, IFormFile mediaFile ) {
            var mediaFileStream = mediaFile.OpenReadStream();

            var mediaFileStoringInfos = new MediaFileStoringInfoModel() {
                FileName = $"{messageId}",
                ContainerName = GetTempStorageName( messageId ),
            };

            var uploadResult = await _mediaStorageService.UploadMediaFile( 
                mediaFileStream, AccessFolderType.PUBLIC, mediaFileStoringInfos );

            return uploadResult;
        }

        private async Task<Stream> LoadMediaFileFromStorage( Guid messageId ) {
            string fileName = $"{messageId}";

            var mediaFileStream = await _mediaStorageService
                .GetMediaFileStream( GetTempStorageName( messageId ), fileName );

            return mediaFileStream;
        }

        public async Task CreateMediaFile( 
            Guid userId, Guid messageId, AttachmentType attachmentType ) {
            var mediaFileFromStorage = await LoadMediaFileFromStorage( messageId );

            var mediaUploadResult = await _mediaFilesUploaderService
                    .UploadMediaFileOnStorage( userId, mediaFileFromStorage, attachmentType );

            CreateAttachmentInfos( userId, messageId, attachmentType, mediaUploadResult );

            await DeleteMediaFile( messageId );
        }

        public async Task CreateMediaFile(
            Guid userId, Guid messageId, IFormFile fileStream, AttachmentType attachmentType ) {
            var mediaUploadResult = await _mediaFilesUploaderService
                    .UploadMediaFileOnStorage( userId, fileStream.OpenReadStream(), attachmentType );

            CreateAttachmentInfos( userId, messageId, attachmentType, mediaUploadResult );
        }

        private void CreateAttachmentInfos( 
            Guid userId, Guid messageId, AttachmentType attachmentType, MediaUploadedResultModel result ) {
            CreateAttachmentOnDatabase( messageId, attachmentType );

            var user = _database.Users.FirstOrDefault( x => x.Id == userId );
            var message = _database.Messages.FirstOrDefault( x => x.Id == messageId );

            SetMessageAsVisibileIfOk( result, message );
            SaveAttachmentResultOnDatabase( result, user, message, attachmentType );
        }

        private async Task DeleteMediaFile( Guid messageId ) {
            var mediaFileStoringInfos = new MediaFileStoringInfoModel() {
                FileName = $"{messageId}",
                ContainerName = GetTempStorageName( messageId ),
            };

            await _mediaStorageService.DeleteMediaFileIfExist( mediaFileStoringInfos );
        }

        private void SetMessageAsVisibileIfOk( 
            MediaUploadedResultModel mediaUploadedResult, Message message ) {
            message.Show = mediaUploadedResult.UploadOk;
        }

        private void CreateAttachmentOnDatabase( Guid messageId, AttachmentType attachmentType ) {
            var message = _database.Messages.FirstOrDefault( x => x.Id == messageId );

            var attachmentParams = new MessageAttachmentCreationParams() {
                AttachmentType = attachmentType,
                AttachmentStatus = MessageContentStatusEnum.Uploading,
                Message = message
            };

            _messageEditorService.SetAttachmentPropertiesToMessage( attachmentParams, message.Author.Id );
        }

        private void SaveAttachmentResultOnDatabase( 
            MediaUploadedResultModel mediaUploadedResult,
            User user, Message message, AttachmentType attachmentType ) {
            var attachmentParams = new MessageAttachmentCreationParams() {
                AttachmentType = attachmentType,
                MediaUploadResult = mediaUploadedResult,
                Message = message
            };

            if ( mediaUploadedResult.UploadOk ) {
                attachmentParams.AttachmentStatus = MessageContentStatusEnum.Ready;
            }
            else {
                attachmentParams.AttachmentStatus = MessageContentStatusEnum.Failed;
            }

            _messageEditorService.SetAttachmentPropertiesToMessage( attachmentParams, message.Id );
            _database.SaveChangesWithEntityTracking( user.Id );
        }

        public async Task<MediaFileSecurityInfosModel> GetMediaFileSecurityInfos( MessageAttachment messageAttachment ) {
            return await _mediaFilesUploaderService.GetMediaFileSecurityInfos( messageAttachment );
        }
    }
}
