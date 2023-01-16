using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Proact.Services.Entities {
    public class Nurse : TrackableEntity, IUserEntity, IEntity {
        public Guid UserId { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<NursesMedicalTeamRelation> MedicalTeamRelations { get; set; }

        public Nurse() {
            MedicalTeamRelations = new HashSet<NursesMedicalTeamRelation>();
        }

        [NotMapped]
        public List<MedicalTeam> MedicalTeams {
            get {
                return MedicalTeamRelations.Select( x => x.MedicalTeam ).ToList();
            }
        }
    }
}
