using Microsoft.AspNetCore.Mvc;
using Proact.Services.AuthorizationPolicies;
using Proact.Services.Controllers.Institutes;
using Proact.Services.Entities;
using Proact.Services.Models;
using Proact.Services.Tests.Shared;
using System;
using Xunit;

namespace Proact.Services.FunctionalTests.Institutes {
    public class UpdateInstitute {
        private void AssertInstituteUpdateCorrectness(
            InstitutesController controller, InstituteUpdateRequest request, Guid instituteId ) {
            var result = controller.Get( instituteId );
            var instituteResult = ( result as OkObjectResult ).Value as InstituteModel;

            Assert.Equal( request.Name, instituteResult.Name );
        }

        [Fact]
        public void UpdateInstitute_CheckConsistency() {
            User admin = null;
            Institute institute = null;

            var servicesProvider = new ProactServicesProvider();
            new DatabaseSnapshotProvider( servicesProvider )
                .AddSystemAdmin( out admin )
                .AddInstituteWithRandomValues( out institute );

            var request = new InstituteUpdateRequest() {
                Name = "new amazing name!"
            };

            var instituteController = new InstitutesControllerProvider(
                servicesProvider, admin, Roles.SystemAdmin );
            var result = instituteController.Controller.Update( institute.Id, request );

            var instituteResult = ( result as OkObjectResult ).Value as InstituteModel;

            Assert.NotNull( instituteResult );
            AssertInstituteUpdateCorrectness( instituteController.Controller, request, institute.Id );
        }
    }
}
