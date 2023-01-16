using System;

namespace Proact.Services.Entities {
    public class MedicsMedicalTeamRelation : TrackableEntity, IEntity {
        public virtual Guid MedicId { get; set; }
        public virtual Medic Medic { get; set; }
        public virtual Guid MedicalTeamId { get; set; }
        public virtual MedicalTeam MedicalTeam { get; set; }
    }
}
