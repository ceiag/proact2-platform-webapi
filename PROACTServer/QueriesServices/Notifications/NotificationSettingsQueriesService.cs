using Microsoft.EntityFrameworkCore;
using Proact.Services.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Proact.Services.QueriesServices {
    public class NotificationSettingsQueriesService : IUserNotificationSettingsQueriesService {
        private readonly ProactDatabaseContext _database;

        private readonly Func<NotificationSettings, bool> _devicesActiveNowPredicate
            = x => x.StartAt == x.StopAt
                || x.StartAt <= new TimeSpan( DateTime.UtcNow.Hour, DateTime.UtcNow.Minute, 0 )
                    && x.StopAt >= new TimeSpan( DateTime.UtcNow.Hour, DateTime.UtcNow.Minute, 0 );

        public NotificationSettingsQueriesService( ProactDatabaseContext database ) {
            _database = database;
        }

        private void AddNotificationSettingsIfNotExist( Guid userId ) {
            if ( _database.NotificationSettings.Count() == 0 || !IsConfigured( userId ) ) {
                var notSettings = new NotificationSettings() {
                    UserId = userId
                };

                _database.NotificationSettings.Add( notSettings );
                _database.SaveChangesWithEntityTracking( userId );
            }
        }

        public void SetActive( Guid userId, bool active ) {
            AddNotificationSettingsIfNotExist( userId );
            GetByUserId( userId ).Active = active;
        }

        public void SetRange( Guid userId, TimeSpan startAt, TimeSpan stopAt ) {
            AddNotificationSettingsIfNotExist( userId );
            GetByUserId( userId ).StartAt = startAt;
            GetByUserId( userId ).StopAt = stopAt;
        }

        public void Reset( Guid userId ) {
            AddNotificationSettingsIfNotExist( userId );
            var notificationSettings = GetByUserId( userId );
            notificationSettings.StartAt = TimeSpan.Zero;
            notificationSettings.StopAt = TimeSpan.Zero;
            notificationSettings.Active = true;
        }

        public NotificationSettings GetByUserId( Guid userId ) {
            return _database.NotificationSettings.FirstOrDefault( x => x.UserId == userId );
        }

        public List<NotificationSettings> GetByUserIds( List<Guid> userIds ) {
            return _database.NotificationSettings
                .Include( x => x.Devices )
                .Where( x => userIds.Contains( x.UserId ) )
                .Where( x => x.Active )
                .ToList();
        }

        public List<Device> GetDevicesOfUsers( List<Guid> userIds ) {
            var devices = new List<Device>();

            foreach ( var not in GetByUserIds( userIds ).ToList() ) {
                devices.AddRange( not.Devices );
            }

            return devices;
        }

        public bool IsConfigured( Guid userId ) {
            return GetByUserId( userId ) != null;
        }

        public void CreateIfNotExist( Guid userId ) {
            AddNotificationSettingsIfNotExist( userId );
            _database.SaveChangesWithEntityTracking( userId );
        }

        public List<NotificationSettings> GetActiveNow( List<Guid> userIds ) {
            var utcNow = new TimeSpan( DateTime.UtcNow.Hour, DateTime.UtcNow.Minute, 0 );

            return _database.NotificationSettings
                .Include( x => x.Devices )
                .Where( x => userIds.Contains( x.UserId ) )
                .Where( x => x.Active )
                .Where( _devicesActiveNowPredicate )
                .ToList();
        }

        public List<NotificationSettings> GetActiveNow( Guid userId ) {
            return GetActiveNow( new List<Guid> { userId } );
        }
    }
}
