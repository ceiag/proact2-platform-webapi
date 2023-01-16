using System;

namespace Proact.Services.Entities {
    public class Device : TrackableEntity, IEntity, IChangeHistoryTrackingEntity {
        public Guid PlayerId { get; set; }
        public virtual Guid NotificationSettingsId { get; set; }
        public virtual NotificationSettings NotificationSettings { get; set; }
    }
}
