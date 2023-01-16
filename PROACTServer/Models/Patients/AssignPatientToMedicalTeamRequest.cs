using System;
using System.ComponentModel.DataAnnotations;

namespace Proact.Services;
public class AssignPatientToMedicalTeamRequest {
    [Required]
    public Guid UserId { get; set; }
    [Required]
    public DateTime? TreatmentStartDate { get; set; }
    [Required]
    public DateTime? TreatmentEndDate { get; set; }
    [Required]
    public string Code { get; set; }
}
