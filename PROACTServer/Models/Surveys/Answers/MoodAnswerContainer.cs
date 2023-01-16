using Proact.Services.Entities;

namespace Proact.Services.Models {
    public class MoodAnswerContainer : ISurveyAnswersContainer {
        public SurveyQuestionType Type { get { return SurveyQuestionType.MOOD; } }
    }
}
