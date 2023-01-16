using System;
using System.Collections.Generic;

namespace Proact.Services.Entities;
public class Patient : TrackableEntity, IUserEntity, IEntity {
    public string Code { get; set; }
    public Guid UserId { get; set; }
    public virtual User User { get; set; }
    public Guid? MedicalTeamId { get; set; }
    public virtual MedicalTeam MedicalTeam { get; set; }
    public int BirthYear { get; set; }
    public string Gender { get; set; }
    public DateTime? TreatmentStartDate { get; set; }
    public DateTime? TreatmentEndDate { get; set; }
}
