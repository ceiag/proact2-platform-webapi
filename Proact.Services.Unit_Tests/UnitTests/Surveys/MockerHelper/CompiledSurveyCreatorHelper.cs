using Proact.Services.Entities;
using Proact.Services.Models;
using Proact.Services.QueriesServices;
using Proact.Services.ServicesProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Proact.Services.UnitTests {
    public static class CompiledSurveyCreatorHelper {
        public static List<SurveysQuestionsRelation> AddDummyCompleteQuestionsCatalogToSurvey(
            MockDatabaseUnitTestHelper mockHelper, Survey survey, SurveyAnswersBlock answersBlock ) {
            var surveyQuestionsSet = SurveyCreatorHelper.CreateDummySurveyQuestionSet( mockHelper );

            mockHelper.ServicesProvider.SaveChanges();

            var openAnswerQuestion = SurveyCreatorHelper
                .CreateDummyOpenQuestion( mockHelper, surveyQuestionsSet );
            var ratingQuestion = SurveyCreatorHelper
                .CreateDummyRatingQuestion( mockHelper, surveyQuestionsSet );
            var singleChoiceQuestion = SurveyCreatorHelper
                .CreateDummySingleChoiceQuestion( mockHelper, surveyQuestionsSet, answersBlock );
            var booleanQuestion = SurveyCreatorHelper
                .CreateDummyBoolQuestion( mockHelper, surveyQuestionsSet );
            var moodQuestion = SurveyCreatorHelper
                .CreateDummyMoodQuestion( mockHelper, surveyQuestionsSet );
            var multipleChoiceQuestion = SurveyCreatorHelper
                .CreateDummyMultipleChoiceQuestion( mockHelper, surveyQuestionsSet, answersBlock );

            mockHelper.ServicesProvider.SaveChanges();

            SurveyCreatorHelper.AddQuestionToSurvey( mockHelper, survey.Id,
                    new List<Guid> {
                        openAnswerQuestion.Id,
                        ratingQuestion.Id,
                        singleChoiceQuestion.Id,
                        booleanQuestion.Id,
                        moodQuestion.Id,
                        multipleChoiceQuestion.Id,
                    } );

            mockHelper.ServicesProvider.SaveChanges();

            return mockHelper.ServicesProvider
                .GetQueriesService<ISurveyQueriesService>()
                .Get( survey.Id ).Questions.ToList();
        }

        private static SurveyQuestionCompileRequest CompileWithDummyAnswerToQuestion(
            SurveyQuestion question, SurveyAnswersBlock answersBlock ) {
            if ( question.Type == SurveyQuestionType.OPEN_ANSWER ) {
                return new SurveyQuestionCompileRequest() {
                    QuestionId = question.Id,
                    Answers = new List<SurveyAnswerCompileRequest> {
                        new SurveyAnswerCompileRequest() {
                            Value = "answer to open question!"
                        }
                    }
                };
            }
            else if ( question.Type == SurveyQuestionType.RATING ) {
                return new SurveyQuestionCompileRequest() {
                    QuestionId = question.Id,
                    Answers = new List<SurveyAnswerCompileRequest> {
                        new SurveyAnswerCompileRequest() {
                            Value = "50"
                        }
                    }
                };
            }
            else if ( question.Type == SurveyQuestionType.SINGLE_ANSWER ) {
                return new SurveyQuestionCompileRequest() {
                    QuestionId = question.Id,
                    Answers = new List<SurveyAnswerCompileRequest> {
                        new SurveyAnswerCompileRequest() {
                            AnswerId = answersBlock.Answers[0].Id
                        }
                    }
                };
            }
            else if ( question.Type == SurveyQuestionType.BOOLEAN ) {
                return new SurveyQuestionCompileRequest() {
                    QuestionId = question.Id,
                    Answers = new List<SurveyAnswerCompileRequest> {
                        new SurveyAnswerCompileRequest() {
                            Value = "true"
                        }
                    }
                };
            }
            else if ( question.Type == SurveyQuestionType.MOOD ) {
                return new SurveyQuestionCompileRequest() {
                    QuestionId = question.Id,
                    Answers = new List<SurveyAnswerCompileRequest> {
                        new SurveyAnswerCompileRequest() {
                            Value = "3"
                        }
                    }
                };
            }
            else if ( question.Type == SurveyQuestionType.MULTIPLE_ANSWERS ) {
                return new SurveyQuestionCompileRequest() {
                    QuestionId = question.Id,
                    Answers = new List<SurveyAnswerCompileRequest> {
                        new SurveyAnswerCompileRequest() {
                            AnswerId = answersBlock.Answers[0].Id
                        },
                        new SurveyAnswerCompileRequest() {
                            AnswerId = answersBlock.Answers[2].Id
                        }
                    }
                };
            }

            return null;
        }

        public static List<SurveysAssignationRelation> AssignDummySurveyToPatients(
            MockDatabaseUnitTestHelper mockHelper, Survey survey, List<Patient> patients, int order ) {

            var assignationRequest = new AssignSurveyToPatientsRequest() {
                Reccurence = SurveyReccurence.Monthly,
                StartTime = SurveyAssignationDataUtils.GetStartTimeFormat( DateTime.UtcNow.AddDays( 1 * order ) ),
                ExpireTime = SurveyAssignationDataUtils.GetExpireTimeFormat( DateTime.UtcNow.AddDays( 2 * order ) ),
                SurveyId = survey.Id,
                UserIds = patients.Select( x => x.UserId ).ToList()
            };

            return mockHelper.ServicesProvider
                    .GetQueriesService<ISurveyAssignationQueriesService>()
                    .AssignSurveyToPatients( assignationRequest );
        }

        public static void CompileSurveyWithDummyAnswers(
            MockDatabaseUnitTestHelper mockHelper, Guid userId, Guid assignationId, Guid surveyId, 
            List<SurveysQuestionsRelation> questions, SurveyAnswersBlock answersBlock ) {

            Assert.NotEmpty( questions );

            var compiledSurveyRequest = new SurveyCompileRequest() {
                AssegnationId = assignationId,
                SurveyId = surveyId,
                QuestionsCompiled = new List<SurveyQuestionCompileRequest>()
            };

            foreach ( var question in questions ) {
                compiledSurveyRequest.QuestionsCompiled.Add(
                    CompileWithDummyAnswerToQuestion( question.Question, answersBlock ) );
            }

            mockHelper.ServicesProvider.SaveChanges();

            mockHelper.ServicesProvider
                .GetEditorsService<ISurveyAnswerToQuestionEditorService>()
                .SetCompiledSurveyFromPatient( userId, compiledSurveyRequest );

            mockHelper.ServicesProvider.SaveChanges();
        }
    }
}
