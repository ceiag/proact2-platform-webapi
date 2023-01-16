using Microsoft.AspNetCore.Mvc;
using Proact.Services.AuthorizationPolicies;
using Proact.Services.Entities;
using Proact.Services.Models;
using Proact.Services.Tests.Shared;
using Xunit;

namespace Proact.Services.FunctionalTests.Institutes {
    public class CloseInstitute {
        [Fact]
        public void CloseInstitute_CheckConsistency() {
            User admin = null;
            Institute institute = null;

            var servicesProvider = new ProactServicesProvider();
            new DatabaseSnapshotProvider( servicesProvider )
                .AddSystemAdmin( out admin )
                .AddInstituteWithRandomValues( out institute );

            var instituteController = new InstitutesControllerProvider(
                servicesProvider, admin, Roles.SystemAdmin );
            var result = instituteController.Controller.Close( institute.Id );

            Assert.NotNull( result as OkResult );

            var instituteResult = instituteController.GetInstitute( institute.Id );
            Assert.Equal( InstituteState.Closed, instituteResult.State );
        }
    }
}
