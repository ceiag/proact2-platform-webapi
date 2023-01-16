using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Linq;
using Proact.Services.Entities.MedicalTeams;

namespace Proact.Services.Entities.Users;

public class DataManager : TrackableEntity, IUserEntity, IEntity {
    public Guid UserId { get; set; }
    public virtual User User { get; set; }
    public virtual ICollection<DataManagersMedicalTeamRelation> DataManagerTeamRelations { get; set; }

    public DataManager() {
        DataManagerTeamRelations = new HashSet<DataManagersMedicalTeamRelation>();
    }

    [NotMapped]
    public List<MedicalTeam> MedicalTeams {
        get {
            return DataManagerTeamRelations.Select( x => x.MedicalTeam ).ToList();
        }
    }
}
