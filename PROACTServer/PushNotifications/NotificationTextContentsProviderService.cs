using Microsoft.Extensions.Localization;
using Proact.Services.Models;
using System.Globalization;

namespace Proact.Services.PushNotifications {
    public class NotificationTextContentsProviderService : INotificationTextProviderService {

        private readonly IStringLocalizer<Resource> _localizer;

        public NotificationTextContentsProviderService( IStringLocalizer<Resource> localizer ) {
            _localizer = localizer;
        }

        public NotificationTextContents GetNotificationText( string contentId ) {
            CultureInfo.CurrentUICulture = new CultureInfo( "us-US", false );
            string en = _localizer[contentId];

            CultureInfo.CurrentUICulture = new CultureInfo( "it-IT", false );
            string it = _localizer[contentId];

            return new NotificationTextContents() {
                en = en,
                it = it
            };
        }
    }
}
