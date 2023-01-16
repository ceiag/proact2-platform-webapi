using Proact.Services.Models;

namespace Proact.Services.PushNotifications {
    public interface INotificationTextProviderService {
        public NotificationTextContents GetNotificationText( string contentId );
    }
}
