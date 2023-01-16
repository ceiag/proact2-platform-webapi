using Proact.Services.Entities;
using Proact.Services.Models;

namespace Proact.Services {
    public interface IQuestionModelComposer {
        public SurveyQuestionModel Compose( 
            SurveyQuestion question, SurveyQuestionModel surveyQuestionModel );
        public SurveyCompiledQuestionModel Compose(
            SurveyQuestion question, SurveyCompiledQuestionModel surveyQuestionModel );
    }
}
