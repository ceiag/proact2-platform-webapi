using Proact.Services.Entities;

namespace Proact.Services.Models {
    public class SurveyNoQuestionProperties : ISurveyQuestionProperties {
        private SurveyQuestionType _type;

        public SurveyNoQuestionProperties( SurveyQuestionType type ) {
            _type = type;
        }

        public SurveyQuestionType Type { get { return _type; } }
    }
}
