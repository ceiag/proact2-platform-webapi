using System;

namespace Proact.Services.Entities {
    public class SurveyUserQuestionAnswer : TrackableEntity, IEntity {
        public virtual Guid? AnswerId { get; set; }
        public virtual SurveyAnswer Answer { get; set; }
        public virtual Guid AssignmentId { get; set; }
        public virtual SurveysAssignationRelation AssignmentRelation { get; set; }
        public string Value { get; set; }
    }
}
