using System;

namespace Proact.Services.Entities {
    public class SurveyAnswer : TrackableEntity, IEntity {
        public string LabelId { get; set; }
        public string SerializedProperties { get; set; }
        public virtual Guid? AnswersBlockId { get; set; }
        public virtual SurveyAnswersBlock? AnswersBlock { get; set; }
    }
}
