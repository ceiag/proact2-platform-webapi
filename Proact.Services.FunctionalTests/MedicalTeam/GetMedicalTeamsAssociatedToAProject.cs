using Microsoft.AspNetCore.Mvc;
using Proact.Services.AuthorizationPolicies;
using Proact.Services.Entities;
using Proact.Services.FunctionalTests.Institutes;
using Proact.Services.Models;
using Proact.Services.Tests.Shared;
using System;
using System.Collections.Generic;
using Xunit;

namespace Proact.Services.FunctionalTests.MedicalTeams {
    public class GetMedicalTeamsAssociatedToAProject {
        [Fact]
        public void GetMedicalTeamsAssociatedToAProject_MustReturnOk() {
            var servicesProvider = new ProactServicesProvider();
            Institute institute = null;
            User instituteAdmin = null;
            Project project_0 = null;
            Project project_1 = null;
            MedicalTeam medicalTeam_0 = null;
            MedicalTeam medicalTeam_1 = null;
            MedicalTeam medicalTeam_2 = null;

            new DatabaseSnapshotProvider( servicesProvider )
                .AddInstituteWithRandomValues( out institute )
                .AddInstituteAdminWithRandomValues( institute, out instituteAdmin )
                .AddProjectWithRandomValues( institute, out project_0 )
                .AddProjectWithRandomValues( institute, out project_1 )
                .AddMedicalTeamWithRandomValues( project_0, out medicalTeam_0 )
                .AddMedicalTeamWithRandomValues( project_0, out medicalTeam_1 )
                .AddMedicalTeamWithRandomValues( project_1, out medicalTeam_2 );

            var medicsController = new MedicalTeamControllerProvider(
                servicesProvider, instituteAdmin, Roles.InstituteAdmin );
            var result = medicsController.Controller
                .GetMedicalTeamsAssociatedToAProject( project_0.Id );

            var medicalTeams = ( result as OkObjectResult ).Value as List<MedicalTeamModel>;

            Assert.Equal( 2, medicalTeams.Count );
        }
    }
}
