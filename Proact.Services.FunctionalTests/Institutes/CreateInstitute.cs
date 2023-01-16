using Microsoft.AspNetCore.Mvc;
using Proact.Services.AuthorizationPolicies;
using Proact.Services.Controllers.Institutes;
using Proact.Services.Entities;
using Proact.Services.Models;
using Proact.Services.Tests.Shared;
using System;
using Xunit;

namespace Proact.Services.FunctionalTests.Institutes {
    public class CreateInstitute {
        [Fact]
        public void CreateInstitute_IntegrityCheck() {
            User admin = null;

            var servicesProvider = new ProactServicesProvider();
            new DatabaseSnapshotProvider( servicesProvider )
                .AddSystemAdmin( out admin );

            var instituteCreateRequest = new InstituteCreationRequest() {
                Name = "amazing institute",
            };

            var instituteController = new InstitutesControllerProvider(
                servicesProvider, admin, Roles.SystemAdmin );
            var result = instituteController.Controller.Create( instituteCreateRequest );

            var instituteCreationResult = ( result as OkObjectResult ).Value as InstituteModel;

            var instituteResult = instituteController.GetInstitute( instituteCreationResult.Id );
            Assert.Equal( instituteCreateRequest.Name, instituteResult.Name );
            Assert.Equal( instituteCreationResult.Id, instituteResult.Id );
        }
    }
}
