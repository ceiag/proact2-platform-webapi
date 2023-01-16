using Newtonsoft.Json;
using Proact.Services.Entities;
using Proact.Services.Models;
using Proact.Services.QueriesServices;
using System;

namespace Proact.Services.UserAnswersSetter {
    public class RatingQuestionUserAnswerSetter : IUserAnswerSetter {
        private readonly ISurveyAnswerToQuestionQueriesService _surveyAnswerToQuestionQueriesHelper;

        public RatingQuestionUserAnswerSetter(
            ISurveyAnswerToQuestionQueriesService surveyAnswerToQuestionQueriesService ) {
            _surveyAnswerToQuestionQueriesHelper = surveyAnswerToQuestionQueriesService;
        }

        public SurveyUserAnswersQuestionModel SetUserAnswerToQuestionSurvey(
           SurveysAssignationRelation assignmentRelation, SurveyQuestionCompileRequest compiledQuestion ) {
            var userAnswer = new SurveyUserQuestionAnswer() {
                AnswerId = null,
                Value = compiledQuestion.Answers[0].Value
            };

            return SurveyUserAnswersEntityMapper.Map( _surveyAnswerToQuestionQueriesHelper
                .SetUserAnswerQuestion( assignmentRelation, compiledQuestion.QuestionId, userAnswer ) );
        }

        public void Validate( SurveyQuestion question, SurveyQuestionCompileRequest compiledQuestion ) {
            try {
                var ratingProps = JsonConvert.DeserializeObject<SurveyMinMaxQuestionProperties>(
                    question.Answers[0].Answer.SerializedProperties );

                int ratingValue = int.Parse( compiledQuestion.Answers[0].Value );

                if ( ratingValue < ratingProps.Min || ratingValue > ratingProps.Max ) {
                    throw new Exception(
                        $"The selected value must be minor of {ratingProps.Max} and greater of {ratingProps.Min}" );
                }
            }
            catch ( Exception e ) {
                throw new Exception(
                    $"Something goes wrong during Validations of question with id {question.Id} " +
                    $"error: {e.Message}" );
            }
        }
    }
}
