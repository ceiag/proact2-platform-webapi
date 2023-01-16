using Microsoft.AspNetCore.Mvc;
using Proact.Services.AuthorizationPolicies;
using Proact.Services.Entities;
using Proact.Services.Models;
using Proact.Services.Tests.Shared;
using System.Collections.Generic;
using Xunit;

namespace Proact.Services.FunctionalTests.Projects {
    public class GetProjectsWhereImAssociated {
        [Fact]
        public void _AsResearcher_MustReturnOne() {
            User instituteAdmin = null;
            Institute institute = null;
            Project project = null;
            MedicalTeam medicalTeam = null;
            Researcher researcher = null;

            var servicesProvider = new ProactServicesProvider();
            new DatabaseSnapshotProvider( servicesProvider )
                .AddInstituteWithRandomValues( out institute )
                .AddInstituteAdminWithRandomValues( institute, out instituteAdmin )
                .AddProjectWithRandomValues( institute, out project )
                .AddMedicalTeamWithRandomValues( project, out medicalTeam )
                .AddResearcherWithRandomValues( medicalTeam, out researcher );

            var projectsController = new ProjectsControllerProvider(
                servicesProvider, researcher.User, Roles.Researcher );
            var result = projectsController.Controller.GetProjectsWhereImAssociated();

            var projects = ( result as OkObjectResult ).Value as List<ProjectModel>;

            Assert.Single( projects );
        }

        [Fact]
        public void _AsMedic_MustReturnOne() {
            User instituteAdmin = null;
            Institute institute = null;
            Project project = null;
            MedicalTeam medicalTeam = null;
            Medic medic = null;

            var servicesProvider = new ProactServicesProvider();
            new DatabaseSnapshotProvider( servicesProvider )
                .AddInstituteWithRandomValues( out institute )
                .AddInstituteAdminWithRandomValues( institute, out instituteAdmin )
                .AddProjectWithRandomValues( institute, out project )
                .AddMedicalTeamWithRandomValues( project, out medicalTeam )
                .AddMedicWithRandomValues( medicalTeam, out medic );

            var projectsController = new ProjectsControllerProvider(
                servicesProvider, medic.User, Roles.MedicalProfessional );
            var result = projectsController.Controller.GetProjectsWhereImAssociated();

            var projects = ( result as OkObjectResult ).Value as List<ProjectModel>;

            Assert.Single( projects );
        }
    }
}
