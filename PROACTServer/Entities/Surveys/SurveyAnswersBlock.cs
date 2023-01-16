using System;
using System.Collections.Generic;

namespace Proact.Services.Entities {
    public class SurveyAnswersBlock : TrackableEntity, IEntity {
        public virtual Guid? ProjectId { get; set; }
        public virtual List<SurveyAnswer> Answers { get; set; }
    }
}
