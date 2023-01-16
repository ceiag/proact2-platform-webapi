using Proact.Services.Entities;
using Proact.Services.Messages;
using Proact.Services.Models;
using System;

namespace Proact.Services.QueriesServices {
    public interface IMessageEditorService : IDataEditorService {
        public MessageModel CreateNewTopicMessage( MessageCreationParams messageCreationParams );
        public MessageModel CreateBroadcastMessage( MessageCreationParams messageCreationParams );
        public MessageModel ReplyToMessage(
            MessageCreationParams messageCreationParams, Guid originalMessageId );
        public void RemoveMessage( Message message );
        public void SetAttachmentPropertiesToMessage(
            MessageAttachmentCreationParams attachmentParams, Guid userId );
    }
}
