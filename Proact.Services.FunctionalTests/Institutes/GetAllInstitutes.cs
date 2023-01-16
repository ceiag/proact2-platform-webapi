using Microsoft.AspNetCore.Mvc;
using Proact.Services.AuthorizationPolicies;
using Proact.Services.Entities;
using Proact.Services.Models;
using Proact.Services.Tests.Shared;
using System.Collections.Generic;
using Xunit;

namespace Proact.Services.FunctionalTests.Institutes {
    public class GetAllInstitutes {
        [Fact]
        public void GetAllInstitute_ReturnTwo() {
            User admin = null;
            Institute institute_0 = null;
            Institute institute_1 = null;

            var servicesProvider = new ProactServicesProvider();
            new DatabaseSnapshotProvider( servicesProvider )
                .AddSystemAdmin( out admin )
                .AddInstituteWithRandomValues( out institute_0 )
                .AddInstituteWithRandomValues( out institute_1 );

            var instituteController = new InstitutesControllerProvider(
                servicesProvider, admin, Roles.SystemAdmin );
            var result = instituteController.Controller.GetAll();

            var instituteResult = ( result as OkObjectResult ).Value as List<InstituteModel>;

            Assert.Equal( 2, instituteResult.Count );
        }
    }
}
