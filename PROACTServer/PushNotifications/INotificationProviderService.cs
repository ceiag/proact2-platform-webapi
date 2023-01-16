using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Proact.Services.PushNotifications {
    public interface INotificationProviderService {
        public Task SendNewMessageArriveNotificationToUsers( 
            List<Guid> usersPlayerIds, Guid originalMessageId, string contentId );
        public Task<HttpResponseMessage> SendSurveyNotificationToDevices(
            List<Guid> userPlayerIds, string contentId );
        public Task SendMessageAttachmentReadyToUser(
            List<Guid> playerIds, Guid originalMessageId, string contentId );
    }
}
