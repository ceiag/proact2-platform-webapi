using Proact.Services.AuthorizationPolicies;
using Proact.Services.Entities;
using Proact.Services.EntitiesMapper;
using Proact.Services.Messages;
using Proact.Services.Models;
using System;

namespace Proact.Services.QueriesServices {
    public class MessageEditorService : IMessageEditorService {
        
        private ProactDatabaseContext _database;

        public MessageEditorService( ProactDatabaseContext database ) {
            _database = database;
        }

        public MessageModel CreateNewTopicMessage( MessageCreationParams messageCreationParams ) {
            var message = CreateNewMessage( messageCreationParams );
            var messageDataBody = CreateMessageData( message, messageCreationParams );

            message.MessageType = GetMessageTypeFromUserRole( messageCreationParams.UserRoles );

            MessagesRecipientHelper.AddRecipientsToMessage(
                    _database, message, messageCreationParams.User, messageCreationParams.MedicalTeam );

            SaveMessageOnDatabase( message, messageDataBody, messageCreationParams.User.Id );

            return MessagesEntityMapper.Map( message );
        }

        public MessageModel CreateBroadcastMessage( MessageCreationParams messageCreationParams ) {
            messageCreationParams.MessageRequestData.Emotion = PatientMood.None;

            var message = CreateNewMessage( messageCreationParams );
            var messageDataBody = CreateMessageData( message, messageCreationParams );

            message.MessageType = MessageType.Broadcast;

            MessagesRecipientHelper.AddRecipientsToMessage(
                    _database, message, messageCreationParams.User, messageCreationParams.MedicalTeam );

            SaveMessageOnDatabase( message, messageDataBody, messageCreationParams.User.Id );

            return MessagesEntityMapper.Map( message );
        }

        public MessageModel ReplyToMessage(
            MessageCreationParams messageCreationParams, Guid originalMessageId ) {
            var replyMessage = CreateNewMessage( messageCreationParams );
            var messageDataBody = CreateMessageData( replyMessage, messageCreationParams );

            replyMessage.OriginalMessageId = originalMessageId;
            replyMessage.MessageType = GetMessageTypeFromUserRole( messageCreationParams.UserRoles );

            var messageReplies = CreateMessageReply( originalMessageId, replyMessage );

            _database.MessagesReplies.Add( messageReplies );

            MessagesRecipientHelper.AddRecipientsToMessage(
                    _database, replyMessage, messageCreationParams.User, messageCreationParams.MedicalTeam );

            SaveMessageOnDatabase( replyMessage, messageDataBody, messageCreationParams.User.Id );

            return MessagesEntityMapper.Map( replyMessage );
        }

        public void RemoveMessage( Message message ) {
            message.Show = false;
            message.State = MessageState.Deleted;
            _database.Messages.Update( message );
        }

        public void SetAttachmentPropertiesToMessage(
            MessageAttachmentCreationParams attachmentParams, Guid userId ) {

            if ( attachmentParams.Message.MessageAttachment != null ) {
                _database.MessageAttachments.Remove( attachmentParams.Message.MessageAttachment );
            }

            if ( attachmentParams.MediaUploadResult == null ) {
                attachmentParams.MediaUploadResult = new MediaUploadedResultModel();
            }

            attachmentParams.Message.MessageAttachment = new MessageAttachment() {
                ContentStatus = attachmentParams.AttachmentStatus,
                ContentUrl = attachmentParams.MediaUploadResult.ContentUrl,
                ThumbnailUrl = attachmentParams.MediaUploadResult.ThumbnailUrl,
                DurationInMilliseconds = attachmentParams.MediaUploadResult.DurationInMilliseconds,
                AttachmentType = attachmentParams.AttachmentType,
                AssetId = attachmentParams.MediaUploadResult.AssetId,
                MessageId = attachmentParams.Message.Id,
                UploadedTime = DateTime.UtcNow,
                FileName = attachmentParams.MediaUploadResult.FileName,
                ContainerName = attachmentParams.MediaUploadResult.ContainerName
            };

            _database.MessageAttachments.Add( attachmentParams.Message.MessageAttachment );
            _database.SaveChangesWithEntityTracking( userId );
        }

        private void SaveMessageOnDatabase( Message message, MessageData messageData, Guid userId ) {
            _database.Messages.Add( message );
            _database.MessagesData.Add( messageData );

            _database.SaveChangesWithEntityTracking( userId );
        }

        private Message CreateNewMessage( MessageCreationParams messageCreationParams ) {
            var message = new Message() {
                Id = Guid.NewGuid(),
                AuthorId = messageCreationParams.User.Id,
                MedicalTeam = messageCreationParams.MedicalTeam,
                OriginalMessageId = Guid.Empty,
                Show = messageCreationParams.ShowAfterCreation,
                Emotion = (PatientMood)messageCreationParams.MessageRequestData.Emotion,
                MessageScope = messageCreationParams.MessageRequestData.MessageScope
            };

            return message;
        }

        private MessageData CreateMessageData( 
            Message message, MessageCreationParams messageCreationParams ) {
            var messageDataBody = new MessageData() {
                Id = Guid.NewGuid(),
                MessageId = message.Id,
                Body = messageCreationParams.MessageRequestData.Body,
                Title = messageCreationParams.MessageRequestData.Title
            };

            return messageDataBody;
        }

        private MessageReplies CreateMessageReply( Guid originalMessageId, Message replyMessage ) {
            var messageReplies = new MessageReplies() {
                OriginalMessageId = originalMessageId,
                ReplyMessageId = replyMessage.Id
            };

            return messageReplies;
        }

        private MessageType GetMessageTypeFromUserRole( UserRoles userRoles ) {
            if ( userRoles.HasRoleOf( Roles.MedicalTeamAdmin )
                || userRoles.HasRoleOf( Roles.MedicalProfessional ) ) {
                return MessageType.Medic;
            }

            if ( userRoles.HasRoleOf( Roles.Patient ) ) {
                return MessageType.Patient;
            }

            return MessageType.Nurse;
        }
    }
}
