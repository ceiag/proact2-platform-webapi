using Proact.Services.Entities;
using Proact.Services.Models;

namespace Proact.Services {
    public class OpenAnswerQuestionModelComposer : IQuestionModelComposer {
        public SurveyQuestionModel Compose(
            SurveyQuestion question, SurveyQuestionModel surveyQuestionModel ) {

            var openAnswerContainer = new SurveyOpenAnswerContainer();

            surveyQuestionModel.AnswersContainer = openAnswerContainer;
            surveyQuestionModel.Properties = new SurveyOpenQuestionProperties();

            return surveyQuestionModel;
        }

        public SurveyCompiledQuestionModel Compose( 
            SurveyQuestion question, SurveyCompiledQuestionModel surveyQuestionModel ) {

            surveyQuestionModel.Properties = new SurveyOpenQuestionProperties();

            return surveyQuestionModel;
        }
    }
}
