using System;
using System.Collections.Generic;

namespace Proact.Services.Entities {
    public class SurveyUsersQuestionsAnswersRelation : TrackableEntity, IEntity {
        public virtual Guid QuestionId { get; set; }
        public virtual SurveyQuestion Question { get; set; }
        public virtual Guid AssignmentId { get; set; }
        public virtual SurveysAssignationRelation AssignmentRelation { get; set; }
        public virtual List<SurveyUserQuestionAnswer> Answers { get; set; }
    }
}
