using Proact.Services.Entities;
using Proact.Services.Models;

namespace Proact.Services {
    public class BoolAnswerQuestionModelComposer : IQuestionModelComposer {
        public SurveyQuestionModel Compose(
            SurveyQuestion question, SurveyQuestionModel surveyQuestionModel ) {
            surveyQuestionModel.AnswersContainer = new BoolAnswerContainer();
            surveyQuestionModel.Properties
                = new SurveyNoQuestionProperties( SurveyQuestionType.BOOLEAN );

            return surveyQuestionModel;
        }

        public SurveyCompiledQuestionModel Compose( 
            SurveyQuestion question, SurveyCompiledQuestionModel surveyQuestionModel ) {
            surveyQuestionModel.Properties = new SurveyNoQuestionProperties( SurveyQuestionType.BOOLEAN );

            return surveyQuestionModel;
        }
    }
}
