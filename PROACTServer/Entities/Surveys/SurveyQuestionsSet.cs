using System;
using System.Collections.Generic;

namespace Proact.Services.Entities {
    public class SurveyQuestionsSet : TrackableEntity, IEntity {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Version { get; set; }
        public QuestionsSetsState State { get; set; } = QuestionsSetsState.DRAW;
        public virtual List<SurveyQuestion> Questions { get; set; }
        public virtual Guid? ProjectId { get; set; }
    }
}
