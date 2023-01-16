using Proact.Services.Entities;

namespace Proact.Services.Models {
    public class SurveyOpenAnswerContainer : ISurveyAnswersContainer {
        public SurveyQuestionType Type { get { return SurveyQuestionType.OPEN_ANSWER; } }
    }
}
