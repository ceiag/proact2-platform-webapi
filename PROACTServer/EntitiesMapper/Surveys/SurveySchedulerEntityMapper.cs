using Proact.Services.Entities;
using Proact.Services.Models;
using System.Collections.Generic;

namespace Proact.Services.EntitiesMapper {
    public static class ScheduledSurveyEntityMapper {
        public static List<SurveySchedulerModel> Map( List<SurveyScheduler> schedulers ) {
            var schedulersModel = new List<SurveySchedulerModel>();
            
            foreach ( var scheduler in schedulers ) {
                schedulersModel.Add( Map( scheduler ) );
            }

            return schedulersModel;
        }

        public static SurveySchedulerModel Map( SurveyScheduler scheduler ) {
            return new SurveySchedulerModel() {
                Id = scheduler.Id,
                ExpireTime = scheduler.ExpireTime,
                StartTime = scheduler.StartTime,
                LastSubmission = scheduler.LastSubmission,
                Reccurence = scheduler.Reccurence,
                SurveyId = scheduler.SurveyId,
                UserId = scheduler.UserId
            };
        }
    }
}
