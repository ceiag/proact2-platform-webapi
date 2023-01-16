using Proact.Services.Entities;
using Proact.Services.Models;
using Proact.Services.QueriesServices;
using System;
using System.Collections.Generic;

namespace Proact.Services.UserAnswersSetter {
    public class MultipleChoiceQuestionUserAnswerSetter : IUserAnswerSetter {
        private readonly ISurveyAnswerToQuestionQueriesService _surveyAnswerToQuestionQueriesHelper;
        private readonly ISurveyAnswersQueriesService _surveyAnswersQueriesService;
        public MultipleChoiceQuestionUserAnswerSetter(
            ISurveyAnswerToQuestionQueriesService surveyAnswerToQuestionQueriesService,
            ISurveyAnswersQueriesService surveyAnswersQueriesService ) {
            _surveyAnswerToQuestionQueriesHelper = surveyAnswerToQuestionQueriesService;
            _surveyAnswersQueriesService = surveyAnswersQueriesService;
        }

        public SurveyUserAnswersQuestionModel SetUserAnswerToQuestionSurvey(
            SurveysAssignationRelation assignmentRelation, SurveyQuestionCompileRequest compiledQuestion ) {
            var userAnswers = new List<SurveyUserQuestionAnswer>();

            foreach ( var answer in compiledQuestion.Answers ) {
                var userAnswer = new SurveyUserQuestionAnswer() {
                    AnswerId = answer.AnswerId,
                    Value = _surveyAnswersQueriesService.Get( answer.AnswerId ).LabelId
                };

                userAnswers.Add( userAnswer );
            }

            return SurveyUserAnswersEntityMapper.Map( _surveyAnswerToQuestionQueriesHelper
                .SetUserAswersQuestion( assignmentRelation, compiledQuestion.QuestionId, userAnswers ) );
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
