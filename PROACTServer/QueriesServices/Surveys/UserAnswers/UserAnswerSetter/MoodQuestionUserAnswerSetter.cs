using Newtonsoft.Json;
using Proact.Services.Entities;
using Proact.Services.Models;
using Proact.Services.QueriesServices;
using System;

namespace Proact.Services.UserAnswersSetter {
    public class MoodQuestionUserAnswerSetter : IUserAnswerSetter {
        private readonly ISurveyAnswerToQuestionQueriesService _surveyAnswerToQuestionQueriesHelper;

        public MoodQuestionUserAnswerSetter(
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
                int patientMoodAsInt = int.Parse( compiledQuestion.Answers[0].Value );

                PatientMood patientMood = (PatientMood)patientMoodAsInt;
            }
            catch ( Exception e ) {
                throw new Exception(
                    $"Something goes wrong during Validations of question with id {question.Id} " +
                    $"error: {e.Message}" );
            }
        }
    }
}
