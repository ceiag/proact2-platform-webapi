using Proact.Services.AuthorizationPolicies;
using Proact.Services.Models;
using Proact.Services.QueriesServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proact.Services.PushNotifications {
    public class MessageNotifierService : IMessageNotifierService {
        private readonly IMessageFormatterService _messageFormatterService;
        private readonly INotificationProviderService _notificationProviderService;
        private readonly IUserNotificationsSettingsEditorService _userNotificationsSettingsEditorService;

        public MessageNotifierService( 
            IMessageFormatterService messageFormatterService,
            IUserNotificationsSettingsEditorService userNotificationsSettingsEditorService,
            INotificationProviderService notificationProviderService ) {
            _messageFormatterService = messageFormatterService;
            _userNotificationsSettingsEditorService = userNotificationsSettingsEditorService;
            _notificationProviderService = notificationProviderService;
        }

        private string GetUserRepliedNotificationMessage( UserRoles userRoles ) {
            if ( userRoles.HasRoleOf( Roles.Patient ) ) {
                return "new_reply_from_patient";
            }
            else if ( userRoles.HasRoleOf( Roles.Nurse ) ) {
                return "new_reply_from_nurse";
            }
            else {
                return "new_reply_from_medic";
            }
        }

        private async Task SendNotificationToRecipients( Guid userId, MessageModel message, string contentId ) {
            var recipientIds = _messageFormatterService
                .GetMessageRecipientsExceptUser( userId, message.MessageId )
                .Select( x => x.Id )
                .ToList();

            var playerIds = _userNotificationsSettingsEditorService
                .GetPlayersIdsActiveNow( recipientIds );

            await _notificationProviderService.SendNewMessageArriveNotificationToUsers(
                playerIds, message.GetOriginalMessageId(), contentId );
        }

        public async Task PerformPatientCreateNewTopic( Guid fromUserId, MessageModel message ) {
            await SendNotificationToRecipients( fromUserId, message, "new_topic_from_patient" );
        }

        public async Task PerformCreateNewBroadcastMessage( Guid fromUserId, MessageModel message ) {
            await SendNotificationToRecipients( fromUserId, message, "new_broadcastmessage" );
        }

        public async Task PerformUserRepliedToMessage( Guid fromUserId, MessageModel message ) {
            await SendNotificationToRecipients( fromUserId, message, "new_reply_to_a_message" );
        }

        public async Task PerformAttachmentReadyForMessage( MessageModel message ) {
            var playerIds = _userNotificationsSettingsEditorService
                .GetPlayersIdsActiveNow( new List<Guid> { (Guid)message.AuthorId } );

            await _notificationProviderService.SendMessageAttachmentReadyToUser(
                playerIds, message.GetOriginalMessageId(), "attachment_ready" );
        }
    }
}
