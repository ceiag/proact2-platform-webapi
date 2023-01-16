using Newtonsoft.Json;
using Proact.Services.Entities;
using System;
using System.Collections.Generic;

namespace Proact.Services.Models.SurveyStats;

public sealed class SurveyStatsAnswer {
    public DateTime Date { get; set; }
    public List<string> Answers { get; set; } = new();
}

public sealed class SurveyStatsQuestion {
    public Guid Id { get; set; }
    public string Question { get; set; }
    public string Title { get; set; }
    public SurveyQuestionType Type { get; set; }
    public object Properties { get; set; }
    public List<SurveyStatsAnswer> Answers { get; set; } = new();
}

public sealed class SurveyStatsResumeByTime {
    public string Title { get; set; }
    public string Description { get; set; }
    public string Version { get; set; }
    public DateTime? StartTime { get; set; }
    public DateTime? ExpireTime { get; set; }
    public List<SurveyStatsQuestion> Questions { get; set; } = new();
}

