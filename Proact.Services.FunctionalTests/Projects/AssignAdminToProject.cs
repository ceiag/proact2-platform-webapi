using Microsoft.AspNetCore.Mvc;
using Proact.Services.AuthorizationPolicies;
using Proact.Services.Controllers;
using Proact.Services.Entities;
using Proact.Services.Models;
using Proact.Services.Tests.Shared;
using System;
using Xunit;

namespace Proact.Services.FunctionalTests.Projects {
    public class AssignAdminToProject {
        private void AssertProjectAdminCorrectness( 
            ProjectsController projectsController, Guid projectId, Guid adminId ) {
            var result = projectsController.GetProjectAdmin( projectId ) as OkObjectResult;
            var projectAdmin = ( result.Value as UserModel );

            Assert.Equal( adminId, projectAdmin.UserId );
        }

        [Fact]
        public void AssignAdminToProject_ReturnOk() {
            User instituteAdmin = null;
            Institute institute = null;
            Project project = null;
            MedicalTeam medicalTeam = null;
            Medic projectAdmin = null;

            var servicesProvider = new ProactServicesProvider();
            new DatabaseSnapshotProvider( servicesProvider )
                .AddInstituteWithRandomValues( out institute )
                .AddInstituteAdminWithRandomValues( institute, out instituteAdmin )
                .AddProjectWithRandomValues( institute, out project )
                .AddMedicalTeamWithRandomValues( project, out medicalTeam )
                .AddMedicWithRandomValues( medicalTeam, out projectAdmin );

            var projectsController = new ProjectsControllerProvider(
                servicesProvider, instituteAdmin, Roles.InstituteAdmin );
            var result = projectsController.Controller
                .AssignAdminToProject( project.Id, projectAdmin.UserId );

            Assert.Equal( 200, ( result as OkResult ).StatusCode );
        }

        [Fact]
        public void AssignAdminToProject_FromOtherInstitute_ReturnBadRequest() {
            User instituteAdmin_0 = null;
            User instituteAdmin_1 = null;
            Institute institute_0 = null;
            Institute institute_1 = null;
            Project project_0 = null;
            Project project_1 = null;
            MedicalTeam medicalTeam_0 = null;
            MedicalTeam medicalTeam_1 = null;
            Medic projectAdmin_0 = null;
            Medic projectAdmin_1 = null;

            var servicesProvider = new ProactServicesProvider();
            new DatabaseSnapshotProvider( servicesProvider )
                .AddInstituteWithRandomValues( out institute_0 )
                .AddInstituteWithRandomValues( out institute_1 )
                .AddInstituteAdminWithRandomValues( institute_0, out instituteAdmin_0 )
                .AddInstituteAdminWithRandomValues( institute_1, out instituteAdmin_1 )
                .AddProjectWithRandomValues( institute_0, out project_0 )
                .AddProjectWithRandomValues( institute_1, out project_1 )
                .AddMedicalTeamWithRandomValues( project_0, out medicalTeam_0 )
                .AddMedicalTeamWithRandomValues( project_1, out medicalTeam_1 )
                .AddMedicWithRandomValues( medicalTeam_0, out projectAdmin_0 )
                .AddMedicWithRandomValues( medicalTeam_1, out projectAdmin_1 );

            var projectsController = new ProjectsControllerProvider(
                servicesProvider, instituteAdmin_0, Roles.InstituteAdmin );
            var result = projectsController.Controller
                .AssignAdminToProject( project_1.Id, projectAdmin_1.UserId );

            Assert.Equal( 400, ( result as BadRequestObjectResult ).StatusCode );
        }
    }
}
