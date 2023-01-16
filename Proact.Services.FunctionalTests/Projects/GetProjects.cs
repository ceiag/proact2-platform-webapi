using Microsoft.AspNetCore.Mvc;
using Proact.Services.AuthorizationPolicies;
using Proact.Services.Entities;
using Proact.Services.Models;
using Proact.Services.Tests.Shared;
using System.Collections.Generic;
using Xunit;

namespace Proact.Services.FunctionalTests.Projects {
    public class GetProjects {
        [Fact]
        public void GetProjects_ReturnOnlyFromMyInstitute() {
            User instituteAdmin_0 = null;
            User instituteAdmin_1 = null;
            Institute institute_0 = null;
            Institute institute_1 = null;
            Project project_0 = null;
            Project project_1 = null;
            Project project_2 = null;

            var servicesProvider = new ProactServicesProvider();
            new DatabaseSnapshotProvider( servicesProvider )
                .AddInstituteWithRandomValues( out institute_0 )
                .AddInstituteWithRandomValues( out institute_1 )
                .AddInstituteAdminWithRandomValues( institute_0, out instituteAdmin_0 )
                .AddInstituteAdminWithRandomValues( institute_1, out instituteAdmin_1 )
                .AddProjectWithRandomValues( institute_0, out project_0 )
                .AddProjectWithRandomValues( institute_0, out project_1 )
                .AddProjectWithRandomValues( institute_1, out project_2 );

            var projectsController = new ProjectsControllerProvider(
                servicesProvider, instituteAdmin_0, Roles.InstituteAdmin );
            var result = projectsController.Controller.GetProjectsAll();

            var projects = ( result as OkObjectResult ).Value as List<ProjectModel>;

            Assert.Equal( 2, projects.Count );
        }
    }
}
