using Proact.Services.Models;
using Proact.Services.Models.SurveyStats;
using System;
using System.Collections.Generic;

namespace Proact.Services.QueriesServices.Surveys.Stats;
public interface ISurveysStatsQueriesService : IDataEditorService {
    public SurveyStatsResume GetStatsResumeForSurvey( Guid surveyId );
    public List<SurveyModel> GetAllSurveysWithAssignedPatients( Guid projectId );
}
