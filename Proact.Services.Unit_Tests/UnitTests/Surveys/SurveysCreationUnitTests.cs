using Proact.Services.Models;
using Proact.Services.QueriesServices;
using Proact.Services.ServicesProviders;
using System;
using System.Collections.Generic;
using Xunit;

namespace Proact.Services.UnitTests.Surveys {
    public class SurveysCreationUnitTests {
        [Fact]
        public void SurveyCreationConsistencyCheck() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var questionsSet = SurveyCreatorHelper.CreateDummySurveyQuestionSet( mockHelper );
                var question_0 = SurveyCreatorHelper.CreateDummyOpenQuestion( mockHelper, questionsSet );
                var question_1 = SurveyCreatorHelper.CreateDummyOpenQuestion( mockHelper, questionsSet );

                mockHelper.ServicesProvider.SaveChanges();

                var creationRequest = new SurveyCreationRequest() {
                    Description = "Description",
                    Title = "Title",
                    Version = "Version",
                    QuestionsSetId = questionsSet.Id,
                    QuestionsIds = new List<Guid>() {
                        question_0.Id,
                        question_1.Id,
                    }
                };

                var createdSurvey = mockHelper.ServicesProvider
                    .GetQueriesService<ISurveyQueriesService>()
                    .Create( creationRequest );

                mockHelper.ServicesProvider.SaveChanges();

                mockHelper.ServicesProvider
                    .GetQueriesService<ISurveyQueriesService>()
                    .AddQuestions( createdSurvey.Id, creationRequest.QuestionsIds );

                mockHelper.ServicesProvider.SaveChanges();

                var queriedSurvey = mockHelper.ServicesProvider
                    .GetQueriesService<ISurveyQueriesService>()
                    .Get( createdSurvey.Id );

                Assert.NotNull( queriedSurvey );
                Assert.Equal( 2, queriedSurvey.Questions.Count );
                Assert.Equal( creationRequest.Description, queriedSurvey.Description );
                Assert.Equal( creationRequest.Title, queriedSurvey.Title );
                Assert.Equal( creationRequest.Version, queriedSurvey.Version );
            }
        }

        [Fact]
        public void SurveyDeleteConsistencyCheck() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var questionsSet = SurveyCreatorHelper.CreateDummySurveyQuestionSet( mockHelper );
                var survey_0 = SurveyCreatorHelper.CreateDummySurvey( mockHelper );
                var survey_1 = SurveyCreatorHelper.CreateDummySurvey( mockHelper );

                mockHelper.ServicesProvider.SaveChanges();

                var question_0 = SurveyCreatorHelper.CreateDummyOpenQuestion( mockHelper, questionsSet );
                var question_1 = SurveyCreatorHelper.CreateDummyOpenQuestion( mockHelper, questionsSet );

                mockHelper.ServicesProvider.SaveChanges();

                var addedQuestion_0 = mockHelper.ServicesProvider
                    .GetQueriesService<ISurveyQueriesService>()
                    .AddQuestions( survey_0.Id, new List<Guid>() { question_0.Id } );

                var addedQuestion_1 = mockHelper.ServicesProvider
                    .GetQueriesService<ISurveyQueriesService>()
                    .AddQuestions( survey_0.Id, new List<Guid>() { question_1.Id } );

                mockHelper.ServicesProvider.SaveChanges();

                var createSurveys = mockHelper.ServicesProvider
                    .GetQueriesService<ISurveyQueriesService>()
                    .GetsAll();

                Assert.Equal( 2, createSurveys.Count );

                mockHelper.ServicesProvider
                    .GetQueriesService<ISurveyQueriesService>()
                    .Delete( survey_0.Id );

                mockHelper.ServicesProvider.SaveChanges();

                var updateSurveys = mockHelper.ServicesProvider
                    .GetQueriesService<ISurveyQueriesService>()
                    .GetsAll();

                Assert.Single( updateSurveys );
            }
        }

        [Fact]
        public void SurveyEditConsistencyCheck() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var questionsSet = SurveyCreatorHelper.CreateDummySurveyQuestionSet( mockHelper );
                var survey = SurveyCreatorHelper.CreateDummySurvey( mockHelper );

                mockHelper.ServicesProvider.SaveChanges();

                var question_0 = SurveyCreatorHelper.CreateDummyOpenQuestion( mockHelper, questionsSet );
                var question_1 = SurveyCreatorHelper.CreateDummyOpenQuestion( mockHelper, questionsSet );

                var editSurveyRequest = new SurveyEditRequest() {
                    SurveyId = survey.Id,
                    QuestionsSetId = questionsSet.Id,
                    Title = Guid.NewGuid().ToString(),
                    Description = Guid.NewGuid().ToString(),
                    Version = Guid.NewGuid().ToString(),
                    QuestionsIds = new List<Guid>() {
                        question_0.Id,
                        question_1.Id
                    }
                };

                var deletedSurvey = mockHelper.ServicesProvider
                    .GetQueriesService<ISurveyQueriesService>()
                    .Delete( editSurveyRequest.SurveyId );

                mockHelper.ServicesProvider.SaveChanges();

                var createdSurvey = mockHelper.ServicesProvider
                    .GetQueriesService<ISurveyQueriesService>()
                    .Create( editSurveyRequest );

                mockHelper.ServicesProvider.SaveChanges();

                mockHelper.ServicesProvider
                    .GetQueriesService<ISurveyQueriesService>()
                    .AddQuestions( createdSurvey.Id, editSurveyRequest.QuestionsIds );

                mockHelper.ServicesProvider.SaveChanges();

                var updateSurveys = mockHelper.ServicesProvider
                    .GetQueriesService<ISurveyQueriesService>()
                    .Get( createdSurvey.Id );

                Assert.Equal( editSurveyRequest.Title, updateSurveys.Title );
                Assert.Equal( editSurveyRequest.Description, updateSurveys.Description );
                Assert.Equal( editSurveyRequest.Version, updateSurveys.Version );
                Assert.Equal( question_0.Id, updateSurveys.Questions[0].QuestionId );
                Assert.Equal( question_1.Id, updateSurveys.Questions[1].QuestionId );
            }
        }

        [Fact]
        public void SurveyAddQuestionsConsistencyCheck() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var questionsSet = SurveyCreatorHelper.CreateDummySurveyQuestionSet( mockHelper );
                var survey = SurveyCreatorHelper.CreateDummySurvey( mockHelper );

                mockHelper.ServicesProvider.SaveChanges();

                var question_0 = SurveyCreatorHelper.CreateDummyOpenQuestion( mockHelper, questionsSet );
                var question_1 = SurveyCreatorHelper.CreateDummyOpenQuestion( mockHelper, questionsSet );

                mockHelper.ServicesProvider.SaveChanges();

                var addedQuestion = mockHelper.ServicesProvider
                    .GetQueriesService<ISurveyQueriesService>()
                    .AddQuestions( survey.Id, new List<Guid>() { question_0.Id, question_1.Id } );

                mockHelper.ServicesProvider.SaveChanges();

                var updatedSurvey = mockHelper.ServicesProvider
                    .GetQueriesService<ISurveyQueriesService>().Get( survey.Id );

                Assert.Equal( 2, updatedSurvey.Questions.Count );
                Assert.Equal( addedQuestion[0].Id, updatedSurvey.Questions[0].Id );
                Assert.Equal( addedQuestion[1].Id, updatedSurvey.Questions[1].Id );
            }
        }

        [Fact]
        public void SurveyRemoveQuestionsConsistencyCheck() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var questionsSet = SurveyCreatorHelper.CreateDummySurveyQuestionSet( mockHelper );
                var survey = SurveyCreatorHelper.CreateDummySurvey( mockHelper );

                mockHelper.ServicesProvider.SaveChanges();

                var question_0 = SurveyCreatorHelper.CreateDummyOpenQuestion( mockHelper, questionsSet );
                var question_1 = SurveyCreatorHelper.CreateDummyOpenQuestion( mockHelper, questionsSet );

                mockHelper.ServicesProvider.SaveChanges();

                var addedQuestion = mockHelper.ServicesProvider
                    .GetQueriesService<ISurveyQueriesService>()
                    .AddQuestions( survey.Id, new List<Guid>() { question_0.Id, question_1.Id } );

                mockHelper.ServicesProvider.SaveChanges();

                var removeQuestionRequest = new RemoveQuestionFromSurveyRequest() {
                    QuestionId = question_0.Id,
                    SurveyId = survey.Id,
                    QuestionsSetId = questionsSet.Id,
                };

                mockHelper.ServicesProvider
                    .GetQueriesService<ISurveyQueriesService>()
                    .RemoveQuestion( survey.Id, question_1.Id );

                mockHelper.ServicesProvider.SaveChanges();

                var updatedSurvey = mockHelper.ServicesProvider
                    .GetQueriesService<ISurveyQueriesService>().Get( survey.Id );

                Assert.Single( updatedSurvey.Questions );
            }
        }
    }
}
