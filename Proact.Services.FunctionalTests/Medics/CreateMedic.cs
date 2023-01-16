using Microsoft.AspNetCore.Mvc;
using Proact.Services.AuthorizationPolicies;
using Proact.Services.Entities;
using Proact.Services.Models;
using Proact.Services.Tests.Shared;
using Xunit;

namespace Proact.Services.FunctionalTests.Medics {
    public class CreateMedic {
        [Fact]
        public void CreateMedic_ConsistencyCheck() {
            var servicesProvider = new ProactServicesProvider();
            Institute institute = null;
            User instituteAdmin = null;
            Project project = null;

            new DatabaseSnapshotProvider( servicesProvider )
                .AddInstituteWithRandomValues( out institute )
                .AddInstituteAdminWithRandomValues( institute, out instituteAdmin )
                .AddProjectWithRandomValues( institute, out project );

            var request = new CreateMedicRequest() {
                Email = "medic@medic.com",
                FirstName = "medic_name",
                Lastname = "medic_surname"
            };

            var medicsController = new MedicsControllerProvider(
                servicesProvider, instituteAdmin, Roles.InstituteAdmin );
            var result = medicsController.Controller.CreateMedic( request );

            var medic = ( result as OkObjectResult ).Value as MedicModel;

            Assert.NotNull( medic );
            Assert.Equal( instituteAdmin.InstituteId, medic.InstituteId );
        }
    }
}
