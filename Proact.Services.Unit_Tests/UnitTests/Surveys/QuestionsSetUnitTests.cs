using Proact.Services.Models;
using Proact.Services.QueriesServices;
using Proact.Services.ServicesProviders;
using Xunit;

namespace Proact.Services.UnitTests.QuestionsSets {
    public class QuestionsSetUnitTests {
        [Fact]
        public void CreateQuestionsSetCheck() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var user = mockHelper.CreateDummyUser();
                var patient = mockHelper.CreateDummyPatient( user );
                var surveyQuestionsSet_0 = SurveyCreatorHelper.CreateDummySurveyQuestionSet( mockHelper );
                var surveyQuestionsSet_1 = SurveyCreatorHelper.CreateDummySurveyQuestionSet( mockHelper );

                mockHelper.ServicesProvider.SaveChanges();

                var questionsSets = mockHelper.ServicesProvider
                    .GetQueriesService<ISurveyQuestionsSetQueriesService>().GetsAll();

                Assert.True( questionsSets.Count == 2 );
            }
        }

        [Fact]
        public void DeleteQuestionsSetCheck() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var user = mockHelper.CreateDummyUser();
                var patient = mockHelper.CreateDummyPatient( user );
                var surveyQuestionsSet_0 = SurveyCreatorHelper.CreateDummySurveyQuestionSet( mockHelper );
                var surveyQuestionsSet_1 = SurveyCreatorHelper.CreateDummySurveyQuestionSet( mockHelper );

                mockHelper.ServicesProvider.SaveChanges();

                mockHelper.ServicesProvider
                    .GetQueriesService<ISurveyQuestionsSetQueriesService>()
                    .Delete( surveyQuestionsSet_0.Id );

                mockHelper.ServicesProvider.SaveChanges();

                var questionsSets = mockHelper.ServicesProvider
                    .GetQueriesService<ISurveyQuestionsSetQueriesService>()
                    .GetsAll();

                Assert.True( questionsSets.Count == 1 );
            }
        }

        [Fact]
        public void EditQuestionsSetCheck() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var user = mockHelper.CreateDummyUser();
                var patient = mockHelper.CreateDummyPatient( user );
                var surveyQuestionsSet_0 = SurveyCreatorHelper.CreateDummySurveyQuestionSet( mockHelper );

                mockHelper.ServicesProvider.SaveChanges();

                var surveyQuestionsSet_1 = new QuestionsSetEditRequest() {
                    Version = "versiontest",
                    Description = "descriptiontest",
                    Title = "titletest"
                };

                mockHelper.ServicesProvider
                    .GetQueriesService<ISurveyQuestionsSetQueriesService>()
                    .Edit( surveyQuestionsSet_0.Id, surveyQuestionsSet_1 );

                mockHelper.ServicesProvider.SaveChanges();

                var questionsSet = mockHelper.ServicesProvider
                    .GetQueriesService<ISurveyQuestionsSetQueriesService>()
                    .Get( surveyQuestionsSet_0.Id );

                Assert.NotNull( questionsSet );
                Assert.True( questionsSet.Title == surveyQuestionsSet_1.Title );
                Assert.True( questionsSet.Description == surveyQuestionsSet_1.Description );
                Assert.True( questionsSet.Version == surveyQuestionsSet_1.Version );
            }
        }

        [Fact]
        public void QuestionsSetCheckQuestionsOrder() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var surveyQuestionsSet = SurveyCreatorHelper.CreateDummySurveyQuestionSet( mockHelper );
                var question_0 = SurveyCreatorHelper.CreateDummyOpenQuestion( mockHelper, surveyQuestionsSet );
                var question_1 = SurveyCreatorHelper.CreateDummyOpenQuestion( mockHelper, surveyQuestionsSet );

                mockHelper.ServicesProvider.SaveChanges();

                Assert.True( question_1.Order > question_0.Order );
            }
        }

        [Fact]
        public void QuestionsSetPublishCheck() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var surveyQuestionsSet = SurveyCreatorHelper.CreateDummySurveyQuestionSet( mockHelper );
                var question_0 = SurveyCreatorHelper.CreateDummyOpenQuestion( mockHelper, surveyQuestionsSet );
                var question_1 = SurveyCreatorHelper.CreateDummyOpenQuestion( mockHelper, surveyQuestionsSet );

                mockHelper.ServicesProvider.SaveChanges();

                mockHelper.ServicesProvider
                    .GetQueriesService<ISurveyQuestionsSetQueriesService>()
                    .SetState( surveyQuestionsSet.Id, QuestionsSetsState.PUBLISHED );

                var questionsSetRetrived = mockHelper.ServicesProvider
                    .GetQueriesService<ISurveyQuestionsSetQueriesService>()
                    .Get( surveyQuestionsSet.Id );

                Assert.Equal( QuestionsSetsState.PUBLISHED, questionsSetRetrived.State );
            }
        }
    }
}
