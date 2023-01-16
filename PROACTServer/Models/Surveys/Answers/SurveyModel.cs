using Proact.Services.Entities;
using System;
using System.Collections.Generic;

namespace Proact.Services.Models;
public class SurveyModel {
    public Guid Id { get; set; }
    public Guid ProjectId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Version { get; set; }
    public DateTime Created { get; set; }
    public DateTime? StartTime { get; set; }
    public DateTime? ExpireTime { get; set; }
    public SurveyReccurence? Reccurence { get; set; }
    public SurveyState SurveyState { get; set; } = SurveyState.DRAW;
    public List<SurveyQuestionModel> Questions { get; set; }
    public List<PatientModel> AssignedPatients { get; set; } = new();
}
