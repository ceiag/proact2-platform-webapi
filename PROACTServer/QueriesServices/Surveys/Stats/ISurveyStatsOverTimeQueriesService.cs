using Proact.Services.Models.SurveyStats;
using System;

namespace Proact.Services.QueriesServices.Surveys.Stats;

public interface ISurveyStatsOverTimeQueriesService : IDataEditorService {
    public SurveyStatsResumeByTime? Get( Guid surveyId, Guid userId );
}
