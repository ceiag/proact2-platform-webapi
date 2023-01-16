using Proact.Services.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Proact.Services.Models;
public class AssignSurveyToPatientRequest {
    public List<SurveyScheduler> Schedulers { get; set; }
    public Guid SurveyId { get; set; }
    [Required]
    public List<Guid> UserIds { get; set; }
    [Required]
    public SurveyReccurence Reccurence { get; set; }
}
