using Proact.Services.Entities.Users;
using System;

namespace Proact.Services.Entities.MedicalTeams;

public class DataManagersMedicalTeamRelation : TrackableEntity, IEntity {
    public virtual Guid DataManagerId { get; set; }
    public virtual DataManager DataManager { get; set; }
    public virtual Guid MedicalTeamId { get; set; }
    public virtual MedicalTeam MedicalTeam { get; set; }
}