using Proact.Services.Entities;
using Proact.Services.EntitiesMapper;
using Proact.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Proact.Services.QueriesServices {
    public class UserNotificationsSettingsEditorService : IUserNotificationsSettingsEditorService {
        private readonly IUserNotificationSettingsQueriesService _notificationSettingsQueriesService;
        private readonly IDeviceQueriesService _deviceQueriesService;

        public UserNotificationsSettingsEditorService(
            IUserNotificationSettingsQueriesService notificationSettingsQueriesService,
            IDeviceQueriesService deviceQueriesService ) {
            _notificationSettingsQueriesService = notificationSettingsQueriesService;
            _deviceQueriesService = deviceQueriesService;
        }

        private void CreateNewSettingsIfNotExist( Guid userId ) {
            _notificationSettingsQueriesService.CreateIfNotExist( userId );
        }

        public Device AddDevice( Guid userId, Guid playerId ) {
            CreateNewSettingsIfNotExist( userId );
            var notificationSettings = _notificationSettingsQueriesService.GetByUserId( userId );

            return _deviceQueriesService.Add( userId, notificationSettings.Id, playerId );
        }

        public List<Guid> GetPlayerIds( Guid userId ) {
            return _deviceQueriesService.GetPlayerIds( userId );
        }

        public List<Guid> GetPlayersIds( List<Guid> userIds ) {
            return _deviceQueriesService.GetPlayersIds( userIds );
        }

        public List<Guid> GetPlayersIdsActiveNow( List<Guid> userIds ) {
            var notificationsActiveNow = _notificationSettingsQueriesService.GetActiveNow( userIds );

            var playerIds = new List<Guid>();

            foreach ( var notSettings in notificationsActiveNow ) {
                playerIds.AddRange( notSettings.Devices.Select( x => x.PlayerId ) );
            }

            return playerIds;
        }

        public bool IsConfigured( Guid userId ) {
            return _notificationSettingsQueriesService.IsConfigured( userId );
        }

        public Device RemoveDevice( Guid userId, Guid playerId ) {
            return _deviceQueriesService.Remove( userId, playerId );
        }

        public void ResetConfiguration( Guid userId ) {
            _notificationSettingsQueriesService.Reset( userId );
        }

        public void SetActive( Guid userId, bool active ) {
            _notificationSettingsQueriesService.SetActive( userId, active );
        }

        public void SetRange( Guid userId, TimeSpan startAt, TimeSpan stopAt ) {
            _notificationSettingsQueriesService.SetRange( userId, startAt, stopAt );
        }

        public NotificationSettingsModel GetNotificationSettingsByUserId( Guid userId ) {
            return NotificationSettingsMapper.Map(
                _notificationSettingsQueriesService.GetByUserId( userId ) );
        }
    }
}
