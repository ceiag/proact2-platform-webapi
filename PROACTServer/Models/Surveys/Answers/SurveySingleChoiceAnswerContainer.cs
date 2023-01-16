using Proact.Services.Entities;
using System;
using System.Collections.Generic;

namespace Proact.Services.Models {
    public class SurveySingleChoiceAnswerContainer : ISurveyAnswersContainer {
        public SurveyQuestionType Type { get { return SurveyQuestionType.SINGLE_ANSWER; } }
        public Guid AnswersBlockId { get; set; }
        public List<SelectableAnswerItem> SelectableAnswers { get; set; }
    }
}
