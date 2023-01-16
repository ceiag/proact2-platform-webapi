using Newtonsoft.Json;
using Proact.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Proact.Services.PushNotifications {
    public class OneSignalProviderService : INotificationProviderService {
        private readonly string _createNotificationRequestUrl = "https://onesignal.com/api/v1/notifications";
        
        private INotificationTextProviderService _notificationTextProvider;

        public OneSignalProviderService( INotificationTextProviderService notificationTextProviderService ) {
            _notificationTextProvider = notificationTextProviderService;
        }

        private string[] GetUsersPlayerIdsFormattedForOneSignal( List<Guid> playerIds ) {
            return playerIds.Select( x => x.ToString() ).ToArray();
        }

        public async Task SendNewMessageArriveNotificationToUsers( 
            List<Guid> playerIds, Guid originalMessageId, string contentId ) {
            var pushRequest = new OneSignalNewMessageNotificationCreationRequest() {
                app_id = OneSignalConfiguration.AppId,
                contents = _notificationTextProvider.GetNotificationText( contentId ),
                include_player_ids = GetUsersPlayerIdsFormattedForOneSignal( playerIds ),
                data = new OneSignalMessageInfoData() {
                    OpenMessageDetail = originalMessageId
                }
            };

            await SendNotification( pushRequest );
        }

        public async Task<HttpResponseMessage> SendSurveyNotificationToDevices(
            List<Guid> playerIds, string contentId ) {
            var pushRequest = new OneSignalNewSurveyNotificationCreationRequest() {
                app_id = OneSignalConfiguration.AppId,
                contents = _notificationTextProvider.GetNotificationText( contentId ),
                include_player_ids = GetUsersPlayerIdsFormattedForOneSignal( playerIds ),
                data = new OneSignalSurveyInfoData()
            };

            return await SendNotification( pushRequest );
        }

        public async Task SendMessageAttachmentReadyToUser(
            List<Guid> playerIds, Guid originalMessageId, string contentId ) {
            var pushRequest = new OneSignalNewMessageNotificationCreationRequest() {
                app_id = OneSignalConfiguration.AppId,
                contents = _notificationTextProvider.GetNotificationText( contentId ),
                include_player_ids = GetUsersPlayerIdsFormattedForOneSignal( playerIds ),
                data = new OneSignalMessageInfoData() {
                    OpenMessageDetail = originalMessageId
                }
            };

            await SendNotification( pushRequest );
        }

        private async Task<HttpResponseMessage> SendNotification( 
            OneSignalNotificationCreationRequest pushRequest ) {
            var oneSignalParams = JsonConvert.SerializeObject( pushRequest );

            var httpRequest = new HttpClient();

            httpRequest.DefaultRequestHeaders.Authorization
                = new AuthenticationHeaderValue( "Basic", OneSignalConfiguration.AppKey );

            HttpContent content = new StringContent(
                oneSignalParams, Encoding.UTF8, "application/json" );

            return await httpRequest.PostAsync( _createNotificationRequestUrl, content );
        }
    }
}
