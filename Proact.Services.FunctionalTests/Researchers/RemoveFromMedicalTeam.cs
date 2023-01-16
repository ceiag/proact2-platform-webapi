using Microsoft.AspNetCore.Mvc;
using Proact.Services.AuthorizationPolicies;
using Proact.Services.Entities;
using Proact.Services.Models;
using Proact.Services.Tests.Shared;
using System.Collections.Generic;
using Xunit;

namespace Proact.Services.FunctionalTests.Researchers {
    public class RemoveFromMedicalTeam {
        [Fact]
        public void RemoveResearchers_MustReturnOne() {
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
            var result = researcherController.Controller
                .RemoveFromMedicalTeam( medicalTeam.Id, researcher_0.UserId );

            var removeResult = ( result as OkResult );
            Assert.NotNull( removeResult );

            var researchersIntoMedicalTeamActionResult 
                = researcherController.Controller.GetResearchers( medicalTeam.Id );

            var researchersIntoMedicalTeam = ( researchersIntoMedicalTeamActionResult as OkObjectResult )
                .Value as List<ResearcherModel>;

            Assert.Single( researchersIntoMedicalTeam );
        }
    }
}
