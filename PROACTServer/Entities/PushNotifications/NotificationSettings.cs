using System;
using System.Collections.Generic;

namespace Proact.Services.Entities {
    public class NotificationSettings : TrackableEntity, IEntity, IChangeHistoryTrackingEntity {
        public bool Active { get; set; } = true;
        public TimeSpan StartAt { get; set; } = TimeSpan.Zero;
        public TimeSpan StopAt { get; set; } = TimeSpan.Zero;
        public virtual Guid UserId { get; set; }
        public virtual User User { get; set; }
        public virtual List<Device> Devices { get; set; }

        public bool AllDay { 
            get {
                return StartAt == StopAt && Active;
            }
        }
    }
}
