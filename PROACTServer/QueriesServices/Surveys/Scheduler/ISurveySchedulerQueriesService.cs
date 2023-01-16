using Proact.Services.Entities;
using Proact.Services.Models;
using System;
using System.Collections.Generic;

namespace Proact.Services.QueriesServices {
    public interface ISurveySchedulerQueriesService : IQueriesService {
        public void Create( List<SurveyScheduler> schedulers );
        public List<SurveyScheduler> Create( CreateScheduledSurveyRequest request );
        public SurveyScheduler Create( SurveyScheduler scheduler );
        public SurveyScheduler GetBySchedulerId( Guid schedulerId );
        public List<SurveyScheduler> GetBySchedulerIds( List<Guid> schedulerIds );
        public List<SurveyScheduler> GetByUserId( Guid userId );
        public List<SurveyScheduler> GetBySurveyId( Guid surveyId );
        public void SetSchedulersAsProcessed( List<SurveyScheduler> schedulers );
        public List<SurveyScheduler> GetNotProcessedOnceSurveyScheduled();
        public List<SurveyScheduler> GetNotProcessedDailySurveyScheduled();
        public List<SurveyScheduler> GetNotProcessedWeeklySurveyScheduled();
        public List<SurveyScheduler> GetNotProcessedMonthlySurveyScheduled();
    }
}
