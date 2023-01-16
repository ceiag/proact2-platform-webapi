using Microsoft.AspNetCore.Mvc;
using Proact.Services.AuthorizationPolicies;
using Proact.Services.Entities;
using Proact.Services.Models;
using Proact.Services.Tests.Shared;
using System.Collections.Generic;
using Xunit;

namespace Proact.Services.FunctionalTests.Researchers {
    public class GetAll {
        [Fact]
        public void GetAll_MustReturnTwo() {
            var servicesProvider = new ProactServicesProvider();
            Institute institute_0 = null;
            Institute institute_1 = null;
            User instituteAdmin_0 = null;
            User instituteAdmin_1 = null;
            Project project_0 = null;
            Project project_1 = null;
            MedicalTeam medicalTeam_0 = null;
            MedicalTeam medicalTeam_1 = null;
            Researcher researcher_0 = null;
            Researcher researcher_1 = null;
            Researcher researcher_2 = null;
            Researcher researcher_3 = null;

            new DatabaseSnapshotProvider( servicesProvider )
                .AddInstituteWithRandomValues( out institute_0 )
                .AddInstituteWithRandomValues( out institute_1 )
                .AddInstituteAdminWithRandomValues( institute_0, out instituteAdmin_0 )
                .AddInstituteAdminWithRandomValues( institute_1, out instituteAdmin_1 )
                .AddProjectWithRandomValues( institute_0, out project_0 )
                .AddProjectWithRandomValues( institute_1, out project_1 )
                .AddMedicalTeamWithRandomValues( project_0, out medicalTeam_0 )
                .AddMedicalTeamWithRandomValues( project_1, out medicalTeam_1 )
                .AddResearcherWithRandomValues( medicalTeam_0, out researcher_0 )
                .AddResearcherWithRandomValues( medicalTeam_0, out researcher_1 )
                .AddResearcherWithRandomValues( medicalTeam_1, out researcher_2 )
                .AddResearcherWithRandomValues( medicalTeam_1, out researcher_3 );

            var researcherController = new MedicsControllerProvider(
                servicesProvider, instituteAdmin_0, Roles.InstituteAdmin );
            var result = researcherController.Controller.GetAll();

            var reseacherResult = ( result as OkObjectResult ).Value as List<ResearcherModel>;

            Assert.NotNull( reseacherResult );
            Assert.Equal( 2, reseacherResult.Count );
        }
    }
}
