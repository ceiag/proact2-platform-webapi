using System;

namespace Proact.Services.Entities {
    public class SurveysQuestionsRelation : TrackableEntity, IEntity {
        public virtual Guid QuestionId { get; set; }
        public virtual SurveyQuestion Question { get; set; }
        public virtual Guid SurveyId { get; set; }
        public virtual Survey Survey { get; set; }
    }
}
