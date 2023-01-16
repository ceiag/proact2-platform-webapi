using Proact.Services.Entities;
using Proact.Services.Models;
using Proact.Services.QueriesServices;
using Proact.Services.ServicesProviders;
using System;
using System.Collections.Generic;
using Xunit;

namespace Proact.Services.UnitTests.SurveysComplied {
    public class SurveyCompiledUnitTests {
        private void CreateTwoCompletedTwoUncompletedSurveyForPatient(
            MockDatabaseUnitTestHelper mockHelper, Patient patient ) {
            var survey = SurveyCreatorHelper.CreateDummySurvey( mockHelper );
            var answersBlock = SurveyCreatorHelper.CreateDummyAnswersBlock( mockHelper );

            mockHelper.ServicesProvider.SaveChanges();

            var dummyQuestions = CompiledSurveyCreatorHelper
                .AddDummyCompleteQuestionsCatalogToSurvey( mockHelper, survey, answersBlock );

            mockHelper.ServicesProvider.SaveChanges();

            var dummyAssignations_0 = CompiledSurveyCreatorHelper
                .AssignDummySurveyToPatients( mockHelper, survey, new List<Patient>() { patient }, 1 );

            var dummyAssignations_1 = CompiledSurveyCreatorHelper
                .AssignDummySurveyToPatients( mockHelper, survey, new List<Patient>() { patient }, 0 );

            CompiledSurveyCreatorHelper
                .AssignDummySurveyToPatients( mockHelper, survey, new List<Patient>() { patient }, 3 );

            CompiledSurveyCreatorHelper
                .AssignDummySurveyToPatients( mockHelper, survey, new List<Patient>() { patient }, 2 );

            mockHelper.ServicesProvider.SaveChanges();

            CompiledSurveyCreatorHelper.CompileSurveyWithDummyAnswers(
                mockHelper, patient.UserId, dummyAssignations_0[0].Id, 
                survey.Id, dummyQuestions, answersBlock );

            CompiledSurveyCreatorHelper.CompileSurveyWithDummyAnswers(
                mockHelper, patient.UserId, dummyAssignations_1[0].Id, 
                survey.Id, dummyQuestions, answersBlock );

            mockHelper.ServicesProvider.SaveChanges();
        }

        [Fact]
        public void GetOnlyCompiledSurveysByPatientCheck() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var user = mockHelper.CreateDummyUser();
                var patient = mockHelper.CreateDummyPatient( user );

                CreateTwoCompletedTwoUncompletedSurveyForPatient( mockHelper, patient );

                var completedSurveysList = mockHelper.ServicesProvider
                    .GetQueriesService<ISurveyAssignationQueriesService>()
                    .GetCompletedSurveysAssigned( patient.UserId );

                Assert.NotNull( completedSurveysList );
                Assert.Equal( 2, completedSurveysList.Count );
                Assert.True( completedSurveysList[0].Completed );
                Assert.True( completedSurveysList[1].Completed );
                Assert.True( 
                    completedSurveysList[0].CompletedDateTime > completedSurveysList[1].CompletedDateTime );
            }
        }

        [Fact]
        public void GetOnlyNotCompiledSurveysByPatientCheck() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var user = mockHelper.CreateDummyUser();
                var patient = mockHelper.CreateDummyPatient( user );

                CreateTwoCompletedTwoUncompletedSurveyForPatient( mockHelper, patient );

                var completedSurveysList = mockHelper.ServicesProvider
                    .GetQueriesService<ISurveyAssignationQueriesService>()
                    .GetNotCompletedSurveysAssigned( patient.UserId );

                Assert.NotNull( completedSurveysList );
                Assert.True( completedSurveysList.Count == 2 );
                Assert.False( completedSurveysList[0].Completed );
                Assert.False( completedSurveysList[1].Completed );
                Assert.True( completedSurveysList[0].ExpireTime < completedSurveysList[1].ExpireTime );
            }
        }

        [Fact]
        public void GetAnswersToSurveyConsistencyCheck() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var user = mockHelper.CreateDummyUser();
                var patient = mockHelper.CreateDummyPatient( user );
                var surveyQuestionsSet = SurveyCreatorHelper.CreateDummySurveyQuestionSet( mockHelper );
                var survey = SurveyCreatorHelper.CreateDummySurvey( mockHelper );
                var answersBlock = SurveyCreatorHelper.CreateDummyAnswersBlock( mockHelper );

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

                var patients = new List<Patient>() { patient };

                var assignSurveyToPatientRequest = new AssignSurveyToPatientsRequest() {
                    Reccurence = SurveyReccurence.Monthly,
                    StartTime = DateTime.UtcNow,
                    ExpireTime = DateTime.UtcNow.AddDays( 10 ),
                    SurveyId = survey.Id,
                    UserIds = new List<Guid> { user.Id }
                };

                var assignments = mockHelper.ServicesProvider
                    .GetQueriesService<ISurveyAssignationQueriesService>()
                    .AssignSurveyToPatients( assignSurveyToPatientRequest );

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

                var compiledSurveyRequest = new SurveyCompileRequest() {
                    AssegnationId = assignments[0].Id,
                    SurveyId = survey.Id,
                    QuestionsCompiled = new List<SurveyQuestionCompileRequest> {
                       new SurveyQuestionCompileRequest() {
                           QuestionId = openAnswerQuestion.Id,
                           Answers = new List<SurveyAnswerCompileRequest> {
                              new SurveyAnswerCompileRequest() {
                                  Value = "answer to open question!"
                              }
                           }
                       },
                       new SurveyQuestionCompileRequest() {
                           QuestionId = ratingQuestion.Id,
                           Answers = new List<SurveyAnswerCompileRequest> {
                              new SurveyAnswerCompileRequest() {
                                  Value = "50"
                              }
                           }
                       },
                       new SurveyQuestionCompileRequest() {
                           QuestionId = singleChoiceQuestion.Id,
                           Answers = new List<SurveyAnswerCompileRequest> {
                              new SurveyAnswerCompileRequest() {
                                  AnswerId = answersBlock.Answers[0].Id
                              }
                           }
                       },
                       new SurveyQuestionCompileRequest() {
                           QuestionId = booleanQuestion.Id,
                           Answers = new List<SurveyAnswerCompileRequest> {
                              new SurveyAnswerCompileRequest() {
                                  Value = "true"
                              }
                           }
                       },
                       new SurveyQuestionCompileRequest() {
                           QuestionId = moodQuestion.Id,
                           Answers = new List<SurveyAnswerCompileRequest> {
                              new SurveyAnswerCompileRequest() {
                                  Value = "3"
                              }
                           }
                       },
                       new SurveyQuestionCompileRequest() {
                           QuestionId = multipleChoiceQuestion.Id,
                           Answers = new List<SurveyAnswerCompileRequest> {
                              new SurveyAnswerCompileRequest() {
                                  AnswerId = answersBlock.Answers[0].Id
                              },
                              new SurveyAnswerCompileRequest() {
                                  AnswerId = answersBlock.Answers[2].Id
                              }
                           }
                       }
                    }
                };

                mockHelper.ServicesProvider.SaveChanges();

                mockHelper.ServicesProvider
                    .GetEditorsService<ISurveyAnswerToQuestionEditorService>()
                    .SetCompiledSurveyFromPatient( user.Id, compiledSurveyRequest );

                mockHelper.ServicesProvider.SaveChanges();

                var compiledSurvey = mockHelper.ServicesProvider
                    .GetEditorsService<ISurveyAnswerToQuestionEditorService>()
                    .GetCompiledSurveyFromPatient( assignments[0].Id );

                Assert.NotNull( compiledSurvey );
                Assert.Equal( survey.Id, compiledSurvey.Id );
                Assert.Equal( survey.Title, compiledSurvey.Title );
                Assert.Equal( survey.Description, compiledSurvey.Description );
                Assert.Equal( survey.Version, compiledSurvey.Version );
                Assert.Equal( survey.SurveyState, compiledSurvey.SurveyState );
                Assert.Equal( assignments[0].StartTime, compiledSurvey.StartTime );
                Assert.Equal( assignments[0].ExpireTime, compiledSurvey.ExpireTime );
                AssertQuestionEqual( openAnswerQuestion, compiledSurvey.Questions[0] );
                AssertQuestionEqual( ratingQuestion, compiledSurvey.Questions[1] );
                AssertQuestionEqual( singleChoiceQuestion, compiledSurvey.Questions[2] );
                AssertQuestionEqual( booleanQuestion, compiledSurvey.Questions[3] );
                AssertQuestionEqual( moodQuestion, compiledSurvey.Questions[4] );
                AssertQuestionEqual( multipleChoiceQuestion, compiledSurvey.Questions[5] );

                for ( int i = 0; i < compiledSurveyRequest.QuestionsCompiled.Count; ++i ) {
                    AssertAnswersEqual(
                        compiledSurveyRequest.QuestionsCompiled[i].Answers,
                        compiledSurvey.Questions[i].CompiledAnswers );
                }
            }
        }

        private void AssertQuestionEqual(
                SurveyQuestionModel questionModel, SurveyCompiledQuestionModel compiledQuestionModel ) {
            Assert.Equal( questionModel.Id, compiledQuestionModel.QuestionId );
            Assert.Equal( questionModel.Question, compiledQuestionModel.Question );
            Assert.Equal( questionModel.Title, compiledQuestionModel.Title );
            Assert.Equal( questionModel.Order, compiledQuestionModel.Order );
            Assert.Equal( questionModel.QuestionsSetId, compiledQuestionModel.QuestionsSetId );
            Assert.Equal( questionModel.Type, compiledQuestionModel.Type );
        }

        private void AssertAnswersEqual( 
            List<SurveyAnswerCompileRequest> requests, List<SurveyCompiledQuestionAnswerModel> compiledAnswers ) {
            for ( int i = 0; i < requests.Count; ++i ) {
                if ( !string.IsNullOrEmpty( requests[i].Value ) ) {
                    Assert.Equal( requests[i].Value, compiledAnswers[i].Value );
                }
                else {
                    Assert.Equal( requests[i].AnswerId, compiledAnswers[i].AnswerId );
                }
            }
        }
    }
}
