using System;

namespace Proact.Services.Entities {
    public class NursesMedicalTeamRelation : TrackableEntity, IEntity {
        public virtual Guid NurseId { get; set; }
        public virtual Nurse Nurse { get; set; }
        public virtual Guid MedicalTeamId { get; set; }
        public virtual MedicalTeam MedicalTeam { get; set; }
    }
}
