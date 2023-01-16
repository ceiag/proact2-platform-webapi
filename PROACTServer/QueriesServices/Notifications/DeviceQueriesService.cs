using Proact.Services.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Proact.Services.QueriesServices {
    public class DeviceQueriesService : IDeviceQueriesService {
        private readonly ProactDatabaseContext _database;

        public DeviceQueriesService( ProactDatabaseContext database ) {
            _database = database;
        }

        public Device Get( Guid playerId ) {
            return _database.Devices.FirstOrDefault( x => x.PlayerId == playerId );
        }

        private void RemoveOldPlayerIdIfExist( Guid userId, Guid playerId) {
            var existingDevice = Get( playerId );

            if ( existingDevice != null ) {
                Remove( userId, playerId );

                _database.SaveChangesWithEntityTracking( userId );
            }
        }

        public Device Add( Guid userId, Guid notificationsSettingsId, Guid playerId ) {
            RemoveOldPlayerIdIfExist( userId, playerId );

            return _database.Devices.Add( new Device() {
                PlayerId = playerId,
                NotificationSettingsId = notificationsSettingsId,
            } ).Entity;
        }

        public Device Remove( Guid userId, Guid playerId ) {
            return _database.Devices.Remove( Get( playerId ) ).Entity;
        }

        public List<Guid> GetPlayerIds( Guid userId ) {
            return _database.Devices
                .Where( x => x.NotificationSettings.UserId == userId )
                .Select( x => x.PlayerId ).ToList();
        }

        public List<Guid> GetPlayersIds( List<Guid> userIds ) {
            return _database.Devices
                .Where( x => userIds.Contains( x.PlayerId ) )
                .Select( x => x.PlayerId ).ToList();
        }
    }
}
