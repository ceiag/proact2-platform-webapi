using Microsoft.AspNetCore.Mvc;
using Proact.Services.AuthorizationPolicies;
using Proact.Services.Entities;
using Proact.Services.Models;
using Proact.Services.Tests.Shared;
using System.Collections.Generic;
using Xunit;

namespace Proact.Services.FunctionalTests.Researchers {
    public class GetResearchers {
        [Fact]
        public void GetResearchers_MustReturnTwo() {
            var servicesProvider = new ProactServicesProvider();
            Institute institute = null;
            User instituteAdmin = null;
            Project project = null;
            MedicalTeam medicalTeam = null;
            Researcher researcher_0 = null;
            Researcher researcher_1 = null;

            new DatabaseSnapshotProvider( servicesProvider )
                .AddInstituteWithRandomValues( out institute )
                .AddInstituteAdminWithRandomValues( institute, out instituteAdmin )
                .AddProjectWithRandomValues( institute, out project )
                .AddMedicalTeamWithRandomValues( project, out medicalTeam )
                .AddResearcherWithRandomValues( medicalTeam, out researcher_0 )
                .AddResearcherWithRandomValues( medicalTeam, out researcher_1 );

            var researcherController = new MedicsControllerProvider(
                servicesProvider, instituteAdmin, Roles.InstituteAdmin );
            var result = researcherController.Controller.GetResearchers( medicalTeam.Id );

            var reseacherResult = ( result as OkObjectResult ).Value as List<ResearcherModel>;

            Assert.NotNull( reseacherResult );
            Assert.Equal( 2, reseacherResult.Count );
        }
    }
}
