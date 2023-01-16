using System;

namespace Proact.Services.Entities {
    public class SurveyQuestionsAnswersRelation : TrackableEntity, IEntity {
        public virtual Guid QuestionId { get; set; }
        public virtual SurveyQuestion Question { get; set; }
        public virtual Guid AnswerId { get; set; }
        public virtual SurveyAnswer Answer { get; set; }
    }
}
