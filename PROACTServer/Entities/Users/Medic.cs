using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Proact.Services.Entities {
    public class Medic : TrackableEntity, IUserEntity, IEntity {
        public Medic() {
            MedicalTeamRelations = new List<MedicsMedicalTeamRelation>();
        }

        public Guid UserId { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<MedicsMedicalTeamRelation> MedicalTeamRelations { get; set; }

        [NotMapped]
        public List<MedicalTeam> MedicalTeams {
            get {
                return MedicalTeamRelations.Select( x => x.MedicalTeam ).ToList();
            }
        }
    }
}
