using Microsoft.AspNetCore.Mvc;
using Proact.Services.AuthorizationPolicies;
using Proact.Services.Controllers;
using Proact.Services.Entities;
using Proact.Services.Models;
using Proact.Services.Tests.Shared;
using System;
using Xunit;

namespace Proact.Services.FunctionalTests.Projects {
    public class CreateProject {
        private void AssertProjectCreationCorrectness( ProjectsController controller, Guid projectId ) {
            var projectModelCreated = controller.GetProject( projectId );
            Assert.NotNull( projectModelCreated );
        }

        [Fact]
        public void CreateNewProject_IntegrityCheck() {
            User systemAdmin = null;
            User instituteAdmin = null;
            Institute institute = null;

            var servicesProvider = new ProactServicesProvider();
            new DatabaseSnapshotProvider( servicesProvider )
                .AddInstituteWithRandomValues( out institute )
                .AddInstituteAdminWithRandomValues( institute, out instituteAdmin )
                .AddUserWithRandomValues( institute, out systemAdmin );

            var projectCreateRequest = new ProjectCreateRequest() {
                Name = "project_name",
                Description = "project_description",
                SponsorName = "project_sponsor",
                Properties = new ProjectPropertiesCreateRequest() {
                    IsAnalystConsoleActive = true,
                    IsSurveysSystemActive = true,
                    MedicsCanSeeOtherAnalisys = true,
                    MessageCanBeAnalizedAfterMinutes = 1,
                    MessageCanBeRepliedAfterMinutes = 2,
                    MessageCanNotBeDeletedAfterMinutes = 3,
                    IsMessagingActive = false
                }
            };

            var projectsController = new ProjectsControllerProvider(
                servicesProvider, instituteAdmin, Roles.InstituteAdmin );
            var result = projectsController.Controller.CreateProject( projectCreateRequest );

            var projectModelResult = ( result as OkObjectResult ).Value as ProjectModel;

            AssertProjectCreationCorrectness( projectsController.Controller, projectModelResult.ProjectId );
            Assert.Equal( projectCreateRequest.Name, projectModelResult.Name );
            Assert.Equal( projectCreateRequest.Description, projectModelResult.Description );
            Assert.Equal( projectCreateRequest.SponsorName, projectModelResult.SponsorName );
            Assert.Equal( projectCreateRequest.Properties.IsAnalystConsoleActive, 
                projectModelResult.Properties.IsAnalystConsoleActive );
            Assert.Equal( projectCreateRequest.Properties.IsSurveysSystemActive,
                projectModelResult.Properties.IsSurveysSystemActive );
            Assert.Equal( projectCreateRequest.Properties.MedicsCanSeeOtherAnalisys,
                projectModelResult.Properties.MedicsCanSeeOtherAnalisys );
            Assert.Equal( projectCreateRequest.Properties.MessageCanBeAnalizedAfterMinutes,
                projectModelResult.Properties.MessageCanBeAnalizedAfterMinutes );
            Assert.Equal( projectCreateRequest.Properties.MessageCanBeRepliedAfterMinutes,
                projectModelResult.Properties.MessageCanBeRepliedAfterMinutes );
            Assert.Equal( projectCreateRequest.Properties.MessageCanNotBeDeletedAfterMinutes,
                projectModelResult.Properties.MessageCanNotBeDeletedAfterMinutes );
            Assert.Equal( projectCreateRequest.Properties.IsMessagingActive,
                projectModelResult.Properties.IsMessagingActive );
        }

        [Fact]
        public void CreateNewProject_WithAllFalseParams_IntegrityCheck() {
            User systemAdmin = null;
            User instituteAdmin = null;
            Institute institute = null;

            var servicesProvider = new ProactServicesProvider();
            new DatabaseSnapshotProvider( servicesProvider )
                .AddInstituteWithRandomValues( out institute )
                .AddInstituteAdminWithRandomValues( institute, out instituteAdmin )
                .AddUserWithRandomValues( institute, out systemAdmin );

            var projectCreateRequest = new ProjectCreateRequest() {
                Name = "project_name",
                Description = "project_description",
                SponsorName = "project_sponsor",
                Properties = new ProjectPropertiesCreateRequest() {
                    IsAnalystConsoleActive = false,
                    IsSurveysSystemActive = false,
                    MedicsCanSeeOtherAnalisys = false,
                    MessageCanBeAnalizedAfterMinutes = 1,
                    MessageCanBeRepliedAfterMinutes = 2,
                    MessageCanNotBeDeletedAfterMinutes = 3,
                    IsMessagingActive = false
                }
            };

            var projectsController = new ProjectsControllerProvider(
                servicesProvider, instituteAdmin, Roles.InstituteAdmin );
            var result = projectsController.Controller.CreateProject( projectCreateRequest );

            var projectModelResult = ( result as OkObjectResult ).Value as ProjectModel;

            AssertProjectCreationCorrectness( projectsController.Controller, projectModelResult.ProjectId );
            Assert.Equal( projectCreateRequest.Name, projectModelResult.Name );
            Assert.Equal( projectCreateRequest.Description, projectModelResult.Description );
            Assert.Equal( projectCreateRequest.SponsorName, projectModelResult.SponsorName );
            Assert.Equal( projectCreateRequest.Properties.IsAnalystConsoleActive,
                projectModelResult.Properties.IsAnalystConsoleActive );
            Assert.Equal( projectCreateRequest.Properties.IsSurveysSystemActive,
                projectModelResult.Properties.IsSurveysSystemActive );
            Assert.Equal( projectCreateRequest.Properties.MedicsCanSeeOtherAnalisys,
                projectModelResult.Properties.MedicsCanSeeOtherAnalisys );
            Assert.Equal( projectCreateRequest.Properties.MessageCanBeAnalizedAfterMinutes,
                projectModelResult.Properties.MessageCanBeAnalizedAfterMinutes );
            Assert.Equal( projectCreateRequest.Properties.MessageCanBeRepliedAfterMinutes,
                projectModelResult.Properties.MessageCanBeRepliedAfterMinutes );
            Assert.Equal( projectCreateRequest.Properties.MessageCanNotBeDeletedAfterMinutes,
                projectModelResult.Properties.MessageCanNotBeDeletedAfterMinutes );
            Assert.Equal( projectCreateRequest.Properties.IsMessagingActive,
                projectModelResult.Properties.IsMessagingActive );
        }
    }
}
