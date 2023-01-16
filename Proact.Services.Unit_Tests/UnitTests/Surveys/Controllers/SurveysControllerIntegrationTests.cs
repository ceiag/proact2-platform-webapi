using Microsoft.AspNetCore.Mvc;
using Moq;
using Proact.Services;
using Proact.Services.AuthorizationPolicies;
using Proact.Services.Controllers.Surveys;
using Proact.Services.Models;
using Proact.Services.QueriesServices;
using Proact.Services.UnitTests;
using Proact.UnitTests.Helpers;
using System;
using System.Collections.Generic;
using Xunit;

namespace Proact.IntegrationTests.Surveys.Controllers {
    public class SurveysControllerIntegrationTests {
        private SurveyController CreateSurveyController(
            MockDatabaseUnitTestHelper mockHelper, string userRole ) {
            var user = mockHelper.CreateDummyUser();
            var mockChangeTrackingService = new Mock<IChangesTrackingService>();

            var surveyController = new SurveyController(
                mockChangeTrackingService.Object,
                mockHelper.ConsistencyRulesHelper,
                mockHelper.ServicesProvider.GetQueriesService<ISurveyQueriesService>() );

            HttpContextMocker.MockHttpContext( surveyController, user, userRole );

            return surveyController;
        }

        [Fact]
        public void EditSurvey_ConsistencyCheck() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var project = mockHelper.CreateDummyProject();
                var questionsSet = SurveyCreatorHelper.CreateDummySurveyQuestionSet( mockHelper );
                var survey = SurveyCreatorHelper.CreateDummySurvey( mockHelper );

                mockHelper.ServicesProvider.SaveChanges();

                var question_0 = SurveyCreatorHelper.CreateDummyOpenQuestion( mockHelper, questionsSet );
                var question_1 = SurveyCreatorHelper.CreateDummyOpenQuestion( mockHelper, questionsSet );

                var surveyController = CreateSurveyController( mockHelper, Roles.SystemAdmin );

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

                var result = surveyController.EditSurvey( editSurveyRequest ) as OkObjectResult;
                var resultProjectModel = result.Value as SurveyModel;

                Assert.Equal( 200, result.StatusCode );
                Assert.Equal( editSurveyRequest.Title, resultProjectModel.Title );
                Assert.Equal( editSurveyRequest.Description, resultProjectModel.Description );
                Assert.Equal( editSurveyRequest.Version, resultProjectModel.Version );
                Assert.NotNull( resultProjectModel.Questions[0] );
                Assert.NotNull( resultProjectModel.Questions[1] );
            }
        }
    }
}
