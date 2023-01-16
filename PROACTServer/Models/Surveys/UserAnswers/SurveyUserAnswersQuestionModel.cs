using System;
using System.Collections.Generic;

namespace Proact.Services.Models {
    public class SurveyUserAnswersQuestionModel {
        public Guid QuestionId { get; set; }
        public Guid QuestionsSetId { get; set; }
        public Guid SurveyId { get; set; }
        public List<SurveyUserAnswerItemModel> Answers { get; set; }
    }
}
