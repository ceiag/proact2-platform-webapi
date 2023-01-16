using Microsoft.AspNetCore.Mvc;
using Moq;
using Proact.Comparators;
using Proact.Services.AuthorizationPolicies;
using Proact.Services.Controllers;
using Proact.Services.Models;
using Proact.Services.QueriesServices;
using Proact.Services.UnitTests;
using Proact.UnitTests.Helpers;
using System;
using System.Collections.Generic;
using Xunit;

namespace Proact.IntegrationTests.Projects.Controllers {
    public class ProjectsControllerIntegrationTests {
        private ProjectsController CreateProjectController(
            MockDatabaseUnitTestHelper mockHelper, string userRole ) {
            var user = mockHelper.CreateDummyUser();
            var mockChangeTrackingService = new Mock<IChangesTrackingService>();

            var projectController = new ProjectsController(
                mockChangeTrackingService.Object,
                mockHelper.ConsistencyRulesHelper,
                mockHelper.ServicesProvider.GetQueriesService<IProjectQueriesService>(),
                mockHelper.ServicesProvider.GetQueriesService<IProjectPropertiesQueriesService>(),
                mockHelper.ServicesProvider.GetEditorsService<IProjectStateEditorService>() );

            HttpContextMocker.MockHttpContext( projectController, user, userRole );

            return projectController;
        }

        [Fact]
        public void CreateProject_ConsinstencyCheck() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var projectRequest = new ProjectCreateRequest() {
                    Name = Guid.NewGuid().ToString(),
                    Description = Guid.NewGuid().ToString(),
                    SponsorName = Guid.NewGuid().ToString(),
                    Properties = new ProjectPropertiesCreateRequest() {
                        MedicsCanSeeOtherAnalisys = true,
                        MessageCanBeAnalizedAfterMinutes = 10,
                        MessageCanBeRepliedAfterMinutes = 11,
                        MessageCanNotBeDeletedAfterMinutes = 3
                    }
                };

                var projectController = CreateProjectController( mockHelper, Roles.SystemAdmin );
                var result = projectController.CreateProject( projectRequest ) as OkObjectResult;
                var resultModel = result.Value as ProjectModel;

                Assert.Equal( 200, result.StatusCode );
                Assert.NotNull( resultModel );
                Assert.Equal( projectRequest.Name, resultModel.Name );
                Assert.Equal( projectRequest.Description, resultModel.Description );
                Assert.Equal( projectRequest.SponsorName, resultModel.SponsorName );
                Assert.True( projectRequest.Properties.MedicsCanSeeOtherAnalisys );
                Assert.Equal( projectRequest.Properties.MessageCanBeAnalizedAfterMinutes,
                   resultModel.Properties.MessageCanBeAnalizedAfterMinutes );
                Assert.Equal( projectRequest.Properties.MessageCanBeRepliedAfterMinutes,
                   resultModel.Properties.MessageCanBeRepliedAfterMinutes );
                Assert.Equal( projectRequest.Properties.MessageCanNotBeDeletedAfterMinutes,
                   resultModel.Properties.MessageCanNotBeDeletedAfterMinutes );
            }
        }

        [Fact]
        public void GetProjectMustReturnOk() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var project = mockHelper.CreateDummyProject();
                var projectController = CreateProjectController( mockHelper, Roles.SystemAdmin );

                var result = projectController.GetProject( project.Id ) as OkObjectResult;
                var resultProjectModel = result.Value as ProjectModel;

                ProjectEqual.AssertEqual( project, resultProjectModel );
                Assert.Equal( 200, result.StatusCode );
            }       
        }

        [Fact]
        public void GetProjectMustReturnNotFound() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var project = mockHelper.CreateDummyProject();
                var projectController = CreateProjectController( mockHelper, Roles.SystemAdmin );

                var result = projectController.GetProject( Guid.NewGuid() ) as NotFoundObjectResult;

                Assert.NotNull( result );
                Assert.Equal( 404, result.StatusCode );
            }
        }

        [Fact]
        public void GetProjectsMustReturnFiveProjectsAsSystemAdmin() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                mockHelper.CreateDummyProject();
                mockHelper.CreateDummyProject();
                mockHelper.CreateDummyProject();
                mockHelper.CreateDummyProject();
                mockHelper.CreateDummyProject();

                var projectController = CreateProjectController( mockHelper, Roles.SystemAdmin );

                var result = projectController.GetProjects() as OkObjectResult;
                var resultProjectModel = result.Value as List<ProjectModel>;

                Assert.Equal( 5, resultProjectModel.Count );
                Assert.Equal( 200, result.StatusCode );
            }
        }

        [Fact]
        public void AssignAdminToProjectMustReturnOk() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var project = mockHelper.CreateDummyProject();
                var user = mockHelper.CreateDummyUser();
                var medic = mockHelper.CreateDummyMedic( user );

                var projectController = CreateProjectController( mockHelper, Roles.SystemAdmin );

                var result = projectController.AssignAdminToProject( project.Id, user.Id ) as OkResult;

                Assert.Equal( 200, result.StatusCode );
            }
        }
    }
}
