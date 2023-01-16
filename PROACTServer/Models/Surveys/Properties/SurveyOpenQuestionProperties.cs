using Proact.Services.Entities;

namespace Proact.Services.Models {
    public class SurveyOpenQuestionProperties : ISurveyQuestionProperties {
        public SurveyQuestionType Type { get { return SurveyQuestionType.OPEN_ANSWER; } }
    }
}
