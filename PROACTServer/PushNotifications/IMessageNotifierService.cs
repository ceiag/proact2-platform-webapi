using Proact.Services.Models;
using System;
using System.Threading.Tasks;

namespace Proact.Services.PushNotifications {
    public interface IMessageNotifierService {
        public Task PerformPatientCreateNewTopic( Guid userId, MessageModel message );
        public Task PerformCreateNewBroadcastMessage( Guid userId, MessageModel message );
        public Task PerformUserRepliedToMessage( Guid fromUserId, MessageModel message );
        public Task PerformAttachmentReadyForMessage( MessageModel message );
    }
}
