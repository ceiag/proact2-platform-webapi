using Proact.Services.Entities;
using Proact.Services.Models;
using System;
using System.Collections.Generic;

namespace Proact.Services.QueriesServices {
    public interface IUserNotificationsSettingsEditorService : IDataEditorService {
        public void SetActive( Guid userId, bool active );
        public void SetRange( Guid userId, TimeSpan startAt, TimeSpan stopAt );
        public void ResetConfiguration( Guid userId );
        public bool IsConfigured( Guid userId );
        public Device AddDevice( Guid userId, Guid playerId );
        public Device RemoveDevice( Guid userId, Guid playerId );
        public List<Guid> GetPlayerIds( Guid userId );
        public List<Guid> GetPlayersIds( List<Guid> userIds );
        public List<Guid> GetPlayersIdsActiveNow( List<Guid> userIds );
        public NotificationSettingsModel GetNotificationSettingsByUserId( Guid userId );
    }
}
