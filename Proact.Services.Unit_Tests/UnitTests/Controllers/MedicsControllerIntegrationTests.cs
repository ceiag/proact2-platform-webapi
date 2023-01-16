using Microsoft.AspNetCore.Mvc;
using Moq;
using Proact.Services;
using Proact.Services.AuthorizationPolicies;
using Proact.Services.Controllers;
using Proact.Services.QueriesServices;
using Proact.Services.UnitTests;
using Proact.UnitTests.Helpers;
using Xunit;

namespace Proact.IntegrationTests.Medics.Controllers {
    public class MedicsControllerIntegrationTests {
        private MedicsController CreateMedicsController(
            MockDatabaseUnitTestHelper mockHelper, string userRole ) {
            var user = mockHelper.CreateDummyUser();
            var mockChangeTrackingService = new Mock<IChangesTrackingService>();

            var medicsController = new MedicsController(
                mockChangeTrackingService.Object,
                mockHelper.ServicesProvider.GetQueriesService<IMedicalTeamQueriesService>(),
                mockHelper.ServicesProvider.GetQueriesService<IMedicQueriesService>(),
                mockHelper.ServicesProvider.GetEditorsService<IUsersCreatorQueriesService>(),
                mockHelper.ConsistencyRulesHelper );

            HttpContextMocker.MockHttpContext( medicsController, user, userRole );

            return medicsController;
        }

        [Fact]
        public void GetMedicMustReturnOk() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var project = mockHelper.CreateDummyProject();
                var medicalTeam = mockHelper.CreateDummyMedicalTeam( project );
                var user = mockHelper.CreateDummyUser();
                var medic = mockHelper.CreateDummyMedic( user );

                var medicsController = CreateMedicsController( mockHelper, Roles.SystemAdmin );
                
                var result = medicsController.GetMedic( medicalTeam.Id, user.Id ) as OkObjectResult;
                var resultMedicModel = result.Value as MedicModel;
                
                Assert.Equal( 200, result.StatusCode );
                Assert.Equal( user.Id, resultMedicModel.UserId );
            }
        }
    }
}
