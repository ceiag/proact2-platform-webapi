using System;

namespace Proact.Services.Entities;
public class TreatmentHistory : TrackableEntity, IEntity {
    public string Code { get; set; }
    public DateTime StartAt { get; set; }
    public DateTime ExpireAt { get; set; }
    public virtual Guid? MedicalTeamId { get; set; }
    public virtual Guid PatientId { get; set; }
}
