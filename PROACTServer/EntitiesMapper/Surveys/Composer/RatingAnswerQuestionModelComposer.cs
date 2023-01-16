using Newtonsoft.Json;
using Proact.Services.Entities;
using Proact.Services.Models;

namespace Proact.Services {
    public class RatingAnswerQuestionModelComposer : IQuestionModelComposer {
        public SurveyQuestionModel Compose(
            SurveyQuestion question, SurveyQuestionModel surveyQuestionModel ) {

            SurveyRatingAnswerContainer ratingAnswersContainer = new SurveyRatingAnswerContainer();

            if ( question.Answers.Count > 0 ) {
                ratingAnswersContainer.Id = question.Answers[0].Id;
            }
            
            surveyQuestionModel.AnswersContainer = ratingAnswersContainer;
            surveyQuestionModel.Properties = GetSurveyQuestionProperties( question );

            return surveyQuestionModel;
        }

        public SurveyCompiledQuestionModel Compose(
            SurveyQuestion question, SurveyCompiledQuestionModel surveyQuestionModel ) {

            SurveyRatingAnswerContainer ratingAnswersContainer = new SurveyRatingAnswerContainer();

            if ( question.Answers.Count > 0 ) {
                ratingAnswersContainer.Id = question.Answers[0].Id;
            }

            surveyQuestionModel.Properties = GetSurveyQuestionProperties( question );

            return surveyQuestionModel;
        }

        public ISurveyQuestionProperties GetSurveyQuestionProperties( SurveyQuestion question ) {
            if ( question.Answers.Count > 0 ) {
                return JsonConvert.DeserializeObject<SurveyMinMaxQuestionProperties>(
                    question.Answers[0].Answer.SerializedProperties );
            }
            else {
                return new SurveyMinMaxQuestionProperties();
            }
        }
    }
}
