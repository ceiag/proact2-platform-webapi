using Proact.Services.Entities;
using System;
using System.Collections.Generic;

namespace Proact.Services.Models.SurveyStats;
public class QuestionAnswer {
    public string Value { get; set; }
    public int Count { get; set; }
}

public class QuestionWithAnswers {
    public string Title { get; set; }
    public string Question { get; set; }
    public ISurveyQuestionProperties SerializedProperties { get; set; }
    public SurveyQuestionType Type { get; set; }
    public List<QuestionAnswer> Answers { get; set; } = new List<QuestionAnswer>();
}

public class SurveyStatsResume {
    public string Title { get; set; }
    public string Description { get; set; }
    public string Version { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime ExpireTime { get; set; }
    public int Participants { get; set; }
    public int Completed { get; set; }
    public List<QuestionWithAnswers> QuestionWithAnswers { get; set; }
        = new List<QuestionWithAnswers>();
}
