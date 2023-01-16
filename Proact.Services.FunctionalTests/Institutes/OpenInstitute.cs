using Microsoft.AspNetCore.Mvc;
using Proact.Services.AuthorizationPolicies;
using Proact.Services.Entities;
using Proact.Services.Models;
using Proact.Services.Tests.Shared;
using Xunit;

namespace Proact.Services.FunctionalTests.Institutes {
    public class OpenInstitute {
        [Fact]
        public void OpenInstitute_CheckConsistency() {
            User admin = null;
            Institute institute = null;

            var servicesProvider = new ProactServicesProvider();
            new DatabaseSnapshotProvider( servicesProvider )
                .AddSystemAdmin( out admin )
                .AddInstituteWithRandomValues( out institute );

            var instituteController = new InstitutesControllerProvider(
                servicesProvider, admin, Roles.SystemAdmin );
            instituteController.Controller.Close( institute.Id );

            var result = instituteController.Controller.Open( institute.Id );

            Assert.NotNull( result as OkResult );

            var instituteResult = instituteController.GetInstitute( institute.Id );
            Assert.Equal( InstituteState.Open, instituteResult.State );
        }
    }
}
