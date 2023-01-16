using Proact.Services.Entities;
using Proact.Services.Models;
using Proact.Services.QueriesServices;
using System;

namespace Proact.Services.UserAnswersSetter {
    public class SingleChoiceQuestionUserAnswerSetter : IUserAnswerSetter {
        private readonly ISurveyAnswerToQuestionQueriesService _surveyAnswerToQuestionQueriesHelper;
        private readonly ISurveyAnswersQueriesService _surveyAnswersQueriesService;
        public SingleChoiceQuestionUserAnswerSetter(
            ISurveyAnswerToQuestionQueriesService surveyAnswerToQuestionQueriesService,
            ISurveyAnswersQueriesService surveyAnswersQueriesService ) {
            _surveyAnswerToQuestionQueriesHelper = surveyAnswerToQuestionQueriesService;
            _surveyAnswersQueriesService = surveyAnswersQueriesService;
        }

        public SurveyUserAnswersQuestionModel SetUserAnswerToQuestionSurvey(
            SurveysAssignationRelation assignmentRelation, SurveyQuestionCompileRequest compiledQuestion ) {
            var answerId = compiledQuestion.Answers[0].AnswerId;

            var userAnswer = new SurveyUserQuestionAnswer() {
                AnswerId = answerId,
                Value = _surveyAnswersQueriesService.Get( answerId ).LabelId
            };

            return SurveyUserAnswersEntityMapper.Map( _surveyAnswerToQuestionQueriesHelper
                .SetUserAnswerQuestion( assignmentRelation, compiledQuestion.QuestionId, userAnswer ) );
        }

        public void Validate( SurveyQuestion question, SurveyQuestionCompileRequest compiledQuestion ) {
            var answerId = compiledQuestion.Answers[0].AnswerId;
            var answer = _surveyAnswersQueriesService.Get( answerId );

            if ( answer == null ) {
                throw new Exception( $"Answer with id {answerId} could not be found!" );
            }
        }
    }
}
