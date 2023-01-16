using System;

namespace Proact.Services.Entities {
    public class InstituteAdmin : TrackableEntity, IEntity {
        public virtual User User { get; set; }
        public virtual Guid UserId { get; set; }
        public virtual Institute Institute { get; set; }
        public virtual Guid InstituteId { get; set; }
    }
}
