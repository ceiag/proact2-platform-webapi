using System;
using System.Collections.Generic;

namespace Proact.Services.Entities {
    public class SurveyQuestion : TrackableEntity, IEntity {
        public string Title { get; set; }
        public string Question { get; set; }
        public int Order { get; set; }
        public SurveyQuestionType Type { get; set; }
        public virtual List<SurveyQuestionsAnswersRelation> Answers { get; set; }
            = new List<SurveyQuestionsAnswersRelation>();
        public virtual SurveyQuestionsSet QuestionsSet { get; set; }
        public virtual Guid QuestionsSetId { get; set; }
    }
}
