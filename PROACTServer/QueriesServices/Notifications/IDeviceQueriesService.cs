using Proact.Services.Entities;
using System;
using System.Collections.Generic;

namespace Proact.Services.QueriesServices {
    public interface IDeviceQueriesService : IQueriesService {
        public Device Get( Guid playerId );
        public Device Add( Guid userId, Guid notificationsSettingsId, Guid playerId );
        public Device Remove( Guid userId, Guid playerId );
        public List<Guid> GetPlayerIds( Guid userId );
        public List<Guid> GetPlayersIds( List<Guid> userIds );
    }
}
