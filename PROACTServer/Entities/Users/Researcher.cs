using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Proact.Services.Entities {
    public class Researcher : TrackableEntity, IUserEntity, IEntity {
        public virtual Guid UserId { get; set; }
        public virtual User User { get; set; }
        
        public Researcher() {
            MedicalTeamRelations = new List<ResearchersMedicalTeamRelation>();
        }

        public virtual List<ResearchersMedicalTeamRelation> MedicalTeamRelations { get; set; }
        [NotMapped]
        public List<MedicalTeam> MedicalTeams {
            get {
                return MedicalTeamRelations.Select( x => x.MedicalTeam ).ToList();
            }
        }
    }
}
