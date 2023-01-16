using Proact.Services.Entities;
using Proact.Services.Models;
using Proact.Services.QueriesServices;
using System;

namespace Proact.Services.UserAnswersSetter {
    public class OpenQuestionUserAnswerSetter : IUserAnswerSetter {
        private readonly ISurveyAnswerToQuestionQueriesService _surveyAnswerToQuestionQueriesHelper;

        public OpenQuestionUserAnswerSetter( 
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
            if ( string.IsNullOrEmpty( compiledQuestion.Answers[0].Value ) ) {
                throw new Exception( "Value can not be null or empty!" );
            }
        }
    }
}
