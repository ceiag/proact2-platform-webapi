using Microsoft.AspNetCore.Mvc;
using Moq;
using Proact.Services.AuthorizationPolicies;
using Proact.Services.Controllers;
using Proact.Services.Models;
using Proact.Services.QueriesServices;
using Proact.Services.UnitTests;
using Proact.Services.UnitTests.MessageAnalysis;
using Proact.UnitTests.Helpers;
using Xunit;

namespace Proact.IntegrationTests.ProjectsProperties.Controllers {
    public class ProjectPropertiesControllerIntegrationTests {
        private ProjectPropertiesCreateRequest _createProjectPropsRequest 
            = new ProjectPropertiesCreateRequest() {
            MedicsCanSeeOtherAnalisys = true,
            MessageCanBeAnalizedAfterMinutes = 19,
            MessageCanNotBeDeletedAfterMinutes = 3
        };

        private ProjectPropertiesUpdateRequest _updateProjectPropsRequest 
            = new ProjectPropertiesUpdateRequest() {
            MedicsCanSeeOtherAnalisys = true,
            MessageCanBeAnalizedAfterMinutes = 19,
            MessageCanNotBeDeletedAfterMinutes = 3
        };

        private ProjectPropertiesController CreateProjectPropertiesController(
            MockDatabaseUnitTestHelper mockHelper, string userRole ) {
            var user = mockHelper.CreateDummyUser();
            var mockChangeTrackingService = new Mock<IChangesTrackingService>();

            var projectPropsController = new ProjectPropertiesController(
                mockHelper.ServicesProvider.GetQueriesService<IProjectPropertiesQueriesService>(),
                mockChangeTrackingService.Object,
                mockHelper.ConsistencyRulesHelper );

            HttpContextMocker.MockHttpContext( projectPropsController, user, userRole );

            return projectPropsController;
        }

        [Fact]
        public void CreateProjectProperties_ConsistencyTest() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var project = mockHelper.CreateDummyProject();
                var projectPropsController 
                    = CreateProjectPropertiesController( mockHelper, Roles.SystemAdmin );

                var result = projectPropsController
                    .Create( project.Id, _createProjectPropsRequest ) as OkObjectResult;
                var resultProjectPropsModel = result.Value as ProjectPropertiesModel;

                Assert.Equal( 200, result.StatusCode );
                Assert.Equal( _createProjectPropsRequest.MedicsCanSeeOtherAnalisys, 
                    resultProjectPropsModel.MedicsCanSeeOtherAnalisys );
                Assert.Equal( _createProjectPropsRequest.MessageCanNotBeDeletedAfterMinutes,
                    resultProjectPropsModel.MessageCanNotBeDeletedAfterMinutes );
                Assert.Equal( _createProjectPropsRequest.MessageCanBeAnalizedAfterMinutes,
                    resultProjectPropsModel.MessageCanBeAnalizedAfterMinutes );
            }
        }

        [Fact]
        public void UpdateProjectProperties_ConsistencyTest() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var project = mockHelper.CreateDummyProject();
                var projectPropsController
                    = CreateProjectPropertiesController( mockHelper, Roles.SystemAdmin );

                var resultCreation = projectPropsController
                    .Create( project.Id, _createProjectPropsRequest ) as OkObjectResult;

                mockHelper.ServicesProvider.SaveChanges();

                var resultUpdate = projectPropsController
                    .Update( project.Id, _updateProjectPropsRequest ) as OkObjectResult;
                var resultProjectPropsModel = resultCreation.Value as ProjectPropertiesModel;

                Assert.Equal( 200, resultUpdate.StatusCode );
                Assert.Equal( _updateProjectPropsRequest.MedicsCanSeeOtherAnalisys,
                    resultProjectPropsModel.MedicsCanSeeOtherAnalisys );
                Assert.Equal( _updateProjectPropsRequest.MessageCanNotBeDeletedAfterMinutes,
                    resultProjectPropsModel.MessageCanNotBeDeletedAfterMinutes );
                Assert.Equal( _updateProjectPropsRequest.MessageCanBeAnalizedAfterMinutes,
                    resultProjectPropsModel.MessageCanBeAnalizedAfterMinutes );
            }
        }

        [Fact]
        public void AddLexiconToProjectProperties_ConsistencyTest() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var project = mockHelper.CreateDummyProject();
                var lexicon = LexiconCreatorHelper.CreateDummyLexicon( mockHelper );

                var projectPropsController
                    = CreateProjectPropertiesController( mockHelper, Roles.SystemAdmin );

                var resultCreation = projectPropsController
                    .Create( project.Id, _createProjectPropsRequest ) as OkObjectResult;

                mockHelper.ServicesProvider.SaveChanges();

                var addLexiconRequest = new LexiconAssignationRequest() {
                    LexiconId = lexicon.Id,
                };

                var result = projectPropsController
                    .AddLexiconToProject( project.Id, addLexiconRequest ) as OkResult;

                Assert.Equal( 200, result.StatusCode );

                mockHelper.ServicesProvider.SaveChanges();

                var projectPropertiesRetrieved = projectPropsController.Get( project.Id ) as OkObjectResult;
                var projectPropertiesRetrievedModel 
                    = projectPropertiesRetrieved.Value as ProjectPropertiesModel;

                Assert.Equal( 200, projectPropertiesRetrieved.StatusCode );
            }
        }
    }
}
