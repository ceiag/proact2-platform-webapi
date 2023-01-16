using Proact.Services.Entities;

namespace Proact.Services.Models {
    public class BoolAnswerContainer : ISurveyAnswersContainer {
        public SurveyQuestionType Type { get { return SurveyQuestionType.BOOLEAN; } }
    }
}
