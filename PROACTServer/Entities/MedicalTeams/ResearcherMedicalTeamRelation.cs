using System;

namespace Proact.Services.Entities {
    public class ResearchersMedicalTeamRelation : TrackableEntity, IEntity {
        public virtual Guid ResearcherId { get; set; }
        public virtual Researcher Researcher { get; set; }
        public virtual Guid MedicalTeamId { get; set; }
        public virtual MedicalTeam MedicalTeam { get; set; }
    }
}
