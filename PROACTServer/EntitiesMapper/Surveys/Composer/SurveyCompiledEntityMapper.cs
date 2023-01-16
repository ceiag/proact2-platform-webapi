using Proact.Services.Entities;
using Proact.Services.Models;
using System.Collections.Generic;
using System.Linq;

namespace Proact.Services {
    public static class SurveyCompiledEntityMapper {
        public static SurveyCompiledModel Map( SurveysAssignationRelation assignment ) {
            return new SurveyCompiledModel() {
                Id = assignment.Survey.Id,
                UserId = assignment.User.Id,
                AssignationId = assignment.Id,
                Title = assignment.Survey.Title,
                Description = assignment.Survey.Description,
                Version = assignment.Survey.Version,
                ExpireTime = assignment.ExpireTime,
                StartTime = assignment.StartTime,
                SurveyState = assignment.Survey.SurveyState,
                CompletedDateTime = assignment.CompletedDateTime,
                Questions = Map( 
                    assignment.Survey.Questions, assignment.UserAnswers )
                        .OrderBy( x => x.Order ).ToList()
            };
        }

        public static List<SurveyCompiledModel> Map( List<SurveysAssignationRelation> assignments ) {
            var surveysCompiled = new List<SurveyCompiledModel>();

            foreach ( var assignment in assignments ) {
                surveysCompiled.Add( Map( assignment ) );
            }

            return surveysCompiled;
        }

        public static List<SurveyCompiledQuestionModel> Map( 
            List<SurveysQuestionsRelation> questionRelations, 
            List<SurveyUsersQuestionsAnswersRelation> answers ) {
            var surveyCompiledQuestions = new List<SurveyCompiledQuestionModel>();

            foreach ( var questionRelation in questionRelations ) {
                surveyCompiledQuestions.Add( Map( questionRelation, answers ) );
            }

            return surveyCompiledQuestions;
        }

        private static readonly SurveyQuestionComposerProvider _surveyQuestionComposerProvider
            = new SurveyQuestionComposerProvider();

        public static SurveyCompiledQuestionModel Map( 
            SurveysQuestionsRelation questionRelation, List<SurveyUsersQuestionsAnswersRelation> answers ) {
            var questionAnswers = answers.FirstOrDefault( 
                x => x.QuestionId == questionRelation.QuestionId );

            var compiledQuestion = new SurveyCompiledQuestionModel() {
                Question = questionRelation.Question.Question,
                Title = questionRelation.Question.Title,
                Order = questionRelation.Question.Order,
                QuestionId = questionRelation.Question.Id,
                Type = questionRelation.Question.Type,
                QuestionsSetId = questionRelation.Question.QuestionsSetId,
                CompiledAnswers = Map( questionAnswers.Answers )
            };

            return _surveyQuestionComposerProvider.Compose( questionRelation.Question, compiledQuestion );
        }

        public static List<SurveyCompiledQuestionAnswerModel> Map( List<SurveyUserQuestionAnswer> answers ) {
            var surveyCompiledQuestionAnswers = new List<SurveyCompiledQuestionAnswerModel>();

            foreach ( var answer in answers ) {
                surveyCompiledQuestionAnswers.Add( Map( answer ) );
            }

            return surveyCompiledQuestionAnswers;
        }

        public static SurveyCompiledQuestionAnswerModel Map( SurveyUserQuestionAnswer answer ) {
            return new SurveyCompiledQuestionAnswerModel() {
                AnswerId = answer.AnswerId,
                Value = answer.Value
            };
        }
    }
}
