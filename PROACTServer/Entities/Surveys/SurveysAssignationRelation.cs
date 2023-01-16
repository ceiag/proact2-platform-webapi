using System;
using System.Collections.Generic;

namespace Proact.Services.Entities {
    public class SurveysAssignationRelation : TrackableEntity, IEntity {
        public virtual Guid UserId { get; set; }
        public virtual User User { get; set; }
        public virtual Guid SurveyId { get; set; }
        public virtual Survey Survey { get; set; }
        public virtual Guid? SchedulerId { get; set; }
        public virtual SurveyScheduler Scheduler { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime ExpireTime { get; set; }
        public DateTime? CompletedDateTime { get; set; }
        public bool Completed { get; set; }
        public virtual List<SurveyUsersQuestionsAnswersRelation> UserAnswers { get; set; }
    }
}
