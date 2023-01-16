using Proact.Services.Entities;
using Proact.Services.Models;

namespace Proact.Services {
    public class MoodAnswerQuestionModelComposer : IQuestionModelComposer {
        public SurveyQuestionModel Compose(
            SurveyQuestion question, SurveyQuestionModel surveyQuestionModel ) {

            surveyQuestionModel.AnswersContainer = new MoodAnswerContainer();
            surveyQuestionModel.Properties
                = new SurveyNoQuestionProperties( SurveyQuestionType.MOOD );

            return surveyQuestionModel;
        }

        public SurveyCompiledQuestionModel Compose(
            SurveyQuestion question, SurveyCompiledQuestionModel surveyQuestionModel ) {

            surveyQuestionModel.Properties
                = new SurveyNoQuestionProperties( SurveyQuestionType.MOOD );

            return surveyQuestionModel;
        }
    }
}
