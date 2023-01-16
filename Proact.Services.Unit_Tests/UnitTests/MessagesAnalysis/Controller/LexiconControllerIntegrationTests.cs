using Microsoft.AspNetCore.Mvc;
using Moq;
using Proact.Services.AuthorizationPolicies;
using Proact.Services.Controllers;
using Proact.Services.Entities;
using Proact.Services.Models;
using Proact.Services.QueriesServices;
using Proact.Services.UnitTests;
using Proact.Services.UnitTests.MessageAnalysis;
using Proact.UnitTests.Helpers;
using Xunit;

namespace Proact.IntegrationTests.MessageAnalysis.Controller {
    public class LexiconControllerIntegrationTests {
        private LexiconsController CreateLexiconsController(
            MockDatabaseUnitTestHelper mockHelper, string userRole, User user ) {
            var mockChangeTrackingService = new Mock<IChangesTrackingService>();

            var lexiconController = new LexiconsController(
                mockHelper.ServicesProvider.GetQueriesService<ILexiconQueriesService>(),
                mockChangeTrackingService.Object,
                mockHelper.ConsistencyRulesHelper );

            HttpContextMocker.MockHttpContext( lexiconController, user, userRole );

            return lexiconController;
        }

        [Fact]
        public void CreateLexicon_ConsistencyTest() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var user_medic = mockHelper.CreateDummyUser();
                var medic = mockHelper.CreateDummyMedic( user_medic );

                var lexiconCreationRequest = LexiconCreatorHelper.GetDummyLexiconCreationRequest();

                var lexiconController = CreateLexiconsController(
                    mockHelper, Roles.MedicalProfessional, user_medic );

                var lexiconCreatedResult = lexiconController
                    .CreateLexicon( lexiconCreationRequest ) as OkObjectResult;
                var lexiconCreatedModel = lexiconCreatedResult.Value as LexiconModel;

                mockHelper.ServicesProvider.SaveChanges();

                var lexiconRetrievedResult = lexiconController
                    .GetLexicon( lexiconCreatedModel.Id ) as OkObjectResult;
                var lexiconRetrievedModel = lexiconCreatedResult.Value as LexiconModel;

                Assert.Equal( 200, lexiconCreatedResult.StatusCode );
                Assert.Equal( lexiconCreationRequest.Name, lexiconRetrievedModel.Name );
                Assert.Equal( lexiconCreationRequest.Description, lexiconRetrievedModel.Description );
                Assert.Equal( 3, lexiconRetrievedModel.Categories.Count );
                Assert.Equal( 3, lexiconRetrievedModel.Categories[0].Labels.Count );
                Assert.Equal( 3, lexiconRetrievedModel.Categories[1].Labels.Count );
                Assert.Equal( 3, lexiconRetrievedModel.Categories[2].Labels.Count );
                Assert.True( lexiconRetrievedModel.Categories[0].Order < lexiconRetrievedModel.Categories[1].Order
                    && lexiconRetrievedModel.Categories[1].Order < lexiconRetrievedModel.Categories[2].Order );
            }
        }
    }
}
