using Proact.Services.Entities;
using Proact.Services.Models;
using System.Collections.Generic;

namespace Proact.Services.EntitiesMapper {
    public static class MessagesEntityMapper {
        public static MessageModel Map( Message entityMessage ) {
            return new MessageModel() {
                AuthorId = entityMessage.AuthorId,
                AuthorName = entityMessage.Author.Name,
                CreatedDateTime = entityMessage.Created,
                Emotion = entityMessage.Emotion,
                MessageScope = entityMessage.MessageScope,
                State = entityMessage.State,
                IsHandled = entityMessage.IsHandled,
                MedicalTeamId = entityMessage.MedicalTeamId,
                MessageId = entityMessage.Id,
                MessageType = entityMessage.MessageType,
                Modified = entityMessage.Modified,
                OriginalMessageId = entityMessage.OriginalMessageId,
                RecordedTime = entityMessage.RecordedTime,
                AvatarUrl = entityMessage.Author.AvatarUrl,
                Attachment = MessagesAttachmentMapper.Map( entityMessage.MessageAttachment ),
                Body = entityMessage.MessageData.Body,
                Title = entityMessage.MessageData.Title,
                AnalysisCount = entityMessage.Analysis.Count
            };
        }

        public static List<MessageModel> Map( IEnumerable<Message> entityMessages ) {
            var messageModels = new List<MessageModel>();

            foreach ( var entityMessage in entityMessages ) {
                messageModels.Add( Map( entityMessage ) );
            }

            return messageModels;
        }

        public static List<MessageModel> Map( ICollection<MessageReplies> replies ) {
            var messageModels = new List<MessageModel>( replies.Count );

            foreach ( var replyMessage in replies ) {
                messageModels.Add( Map( replyMessage.ReplyMessage ) );
            }

            return messageModels;
        }
    }
}
