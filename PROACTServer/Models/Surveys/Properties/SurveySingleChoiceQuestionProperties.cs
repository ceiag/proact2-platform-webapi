using Proact.Services.Entities;
using System.Collections.Generic;

namespace Proact.Services.Models {
    public class SurveySingleChoiceQuestionProperties : ISurveyQuestionProperties {
        public List<SelectableAnswerItem> SelectableAnswers { get; set; }

        public SurveyQuestionType Type { 
            get { return SurveyQuestionType.SINGLE_ANSWER; } 
        }
    }
}
