using Proact.Services.Entities;
using System;
using System.Collections.Generic;

namespace Proact.Services.QueriesServices {
    public interface IUserNotificationSettingsQueriesService : IQueriesService {
        public NotificationSettings GetByUserId( Guid userId );
        public List<NotificationSettings> GetByUserIds( List<Guid> userIds );
        public void SetActive( Guid userId, bool active );
        public void SetRange( Guid userId, TimeSpan startAt, TimeSpan stopAt );
        public void Reset( Guid userId );
        public bool IsConfigured( Guid userId );
        public void CreateIfNotExist( Guid userId );
        public List<NotificationSettings> GetActiveNow( List<Guid> userIds );
        public List<NotificationSettings> GetActiveNow( Guid userId );
        public List<Device> GetDevicesOfUsers( List<Guid> userIds );
    }
}
