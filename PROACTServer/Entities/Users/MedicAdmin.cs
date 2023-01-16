using System;

namespace Proact.Services.Entities {
    public class MedicAdmin : TrackableEntity, IUserEntity, IEntity {
        public Guid UserId { get; set; }
        public virtual User User { get; set; }
        public Guid MedicalTeamId { get; set; }
        public virtual MedicalTeam MedicalTeam { get; set; }
    }
}
