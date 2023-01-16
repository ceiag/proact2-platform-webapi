using Microsoft.AspNetCore.Mvc;
using Proact.Services.AuthorizationPolicies;
using Proact.Services.Entities;
using Proact.Services.Models;
using Proact.Services.Tests.Shared;
using Xunit;

namespace Proact.Services.FunctionalTests.Researchers {
    public class CreateResearcher {
        [Fact]
        public void CreateResearcher_ConsistencyCheck() {
            var servicesProvider = new ProactServicesProvider();
            Institute institute = null;
            User instituteAdmin = null;
            Project project = null;

            new DatabaseSnapshotProvider( servicesProvider )
                .AddInstituteWithRandomValues( out institute )
                .AddInstituteAdminWithRandomValues( institute, out instituteAdmin )
                .AddProjectWithRandomValues( institute, out project );

            var request = new CreateResearcherRequest() {
                Email = "researcher@medic.com",
                FirstName = "researcher_name",
                Lastname = "researcher_surname"
            };

            var researcherController = new MedicsControllerProvider(
                servicesProvider, instituteAdmin, Roles.InstituteAdmin );
            var result = researcherController.Controller.Create( request );

            var reseacherResult = ( result as OkObjectResult ).Value as ResearcherModel;

            Assert.NotNull( reseacherResult );
            Assert.Equal( instituteAdmin.InstituteId, reseacherResult.InstituteId );
        }
    }
}
