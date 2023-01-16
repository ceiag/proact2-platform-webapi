using System;

namespace Proact.Services.Entities {
    public class User : TrackableEntity, IEntity, IChangeHistoryTrackingEntity {
        public string AnonimizedName {
            get {
                return Id.ToReadable().ToUpper();
            }
        }

        public string Name { get; set; }
        public string AccountId { get; set; }
        public string Title { get; set; }
        public string AvatarUrl { get; set; } = "";
        public UserSubscriptionState State { get; set; }
        public virtual Institute Institute { get; set; }
        public virtual Guid? InstituteId { get; set; }
        public virtual NotificationSettings NotificationSettings { get; set; }
    }
}
