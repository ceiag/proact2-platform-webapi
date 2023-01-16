using Microsoft.AspNetCore.Mvc;
using Proact.Services.AuthorizationPolicies;
using Proact.Services.Entities;
using Proact.Services.Models;
using Proact.Services.Tests.Shared;
using Xunit;

namespace Proact.Services.FunctionalTests.Institutes {
    public class GetWhereImAdmin {
        [Fact]
        public void GetWhereImAdmin_MustReturnOk() {
            User admin = null;
            Institute institute = null;
            User instituteAdmin = null;

            var servicesProvider = new ProactServicesProvider();
            new DatabaseSnapshotProvider( servicesProvider )
                .AddSystemAdmin( out admin )
                .AddInstituteWithRandomValues( out institute )
                .AddInstituteAdminWithRandomValues( institute, out instituteAdmin );

            var instituteController = new InstitutesControllerProvider(
                servicesProvider, instituteAdmin, Roles.InstituteAdmin );

            var result = instituteController.Controller.GetWhereImAdmin();

            Assert.NotNull( result as OkObjectResult );
        }

        [Fact]
        public void GetWhereImAdmin_MustReturnNotFound() {
            User admin = null;
            Institute institute = null;
            User instituteAdmin = null;

            var servicesProvider = new ProactServicesProvider();
            new DatabaseSnapshotProvider( servicesProvider )
                .AddSystemAdmin( out admin )
                .AddInstituteWithRandomValues( out institute )
                .AddInstituteAdminWithRandomValues( institute, out instituteAdmin );

            var instituteController = new InstitutesControllerProvider(
                servicesProvider, admin, Roles.InstituteAdmin );

            var result = instituteController.Controller.GetWhereImAdmin();

            Assert.NotNull( result as NotFoundObjectResult );
        }
    }
}
