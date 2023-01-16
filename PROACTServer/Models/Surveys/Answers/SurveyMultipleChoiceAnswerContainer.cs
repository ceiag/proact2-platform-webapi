using Proact.Services.Entities;
using System;
using System.Collections.Generic;

namespace Proact.Services.Models {
    public class SurveyMultipleChoiceAnswerContainer : ISurveyAnswersContainer {
        public SurveyQuestionType Type { get { return SurveyQuestionType.MULTIPLE_ANSWERS; } }
        public Guid AnswersBlockId { get; set; }
        public List<SelectableAnswerItem> SelectableAnswers { get; set; }
    }
}
