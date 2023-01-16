using Microsoft.AspNetCore.Mvc;
using Proact.Services.AuthorizationPolicies;
using Proact.Services.Entities;
using Proact.Services.Models;
using Proact.Services.Tests.Shared;
using Xunit;

namespace Proact.Services.FunctionalTests.Nurses {
    public class CreateNurse {
        [Fact]
        public void CreateNurse_ConsistencyCheck() {
            var servicesProvider = new ProactServicesProvider();
            Institute institute = null;
            User instituteAdmin = null;
            Project project = null;

            new DatabaseSnapshotProvider( servicesProvider )
                .AddInstituteWithRandomValues( out institute )
                .AddInstituteAdminWithRandomValues( institute, out instituteAdmin )
                .AddProjectWithRandomValues( institute, out project );

            var request = new CreateNurseRequest() {
                Email = "nurse@nurse.com",
                FirstName = "nurse_name",
                Lastname = "nurse_surname"
            };

            var nursesController = new NursesControllerProvider(
                servicesProvider, instituteAdmin, Roles.InstituteAdmin );
            var result = nursesController.Controller.CreateNurse( request );

            var nurse = ( result as OkObjectResult ).Value as NurseModel;

            Assert.NotNull( nurse );
            Assert.Equal( instituteAdmin.InstituteId, nurse.InstituteId );
        }
    }
}