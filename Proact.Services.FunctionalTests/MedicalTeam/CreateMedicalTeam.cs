using Microsoft.AspNetCore.Mvc;
using Proact.Services.AuthorizationPolicies;
using Proact.Services.Entities;
using Proact.Services.FunctionalTests.Institutes;
using Proact.Services.Models;
using Proact.Services.Tests.Shared;
using System;
using Xunit;

namespace Proact.Services.FunctionalTests.MedicalTeams {
    public class CreateMedicalTeam {
        [Fact]
        public void CreateMedicalTeam_ConsistencyCheck() {
            var servicesProvider = new ProactServicesProvider();
            Institute institute = null;
            User instituteAdmin = null;
            Project project = null;

            new DatabaseSnapshotProvider( servicesProvider )
                .AddInstituteWithRandomValues( out institute )
                .AddInstituteAdminWithRandomValues( institute, out instituteAdmin )
                .AddProjectWithRandomValues( institute, out project );

            var request = new MedicalTeamCreateRequest() {
                AddressLine1 = Guid.NewGuid().ToString(),
                AddressLine2 = Guid.NewGuid().ToString(),
                City = Guid.NewGuid().ToString(),
                Country = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString(),
                Phone = Guid.NewGuid().ToString(),
                PostalCode = Guid.NewGuid().ToString(),
                RegionCode = "IT-GE",
                StateOrProvince = "IT",
                TimeZone = "GMT+1"
            };

            var medicsController = new MedicalTeamControllerProvider(
                servicesProvider, instituteAdmin, Roles.InstituteAdmin );
            var result = medicsController.Controller.CreateMedicalTeam( project.Id, request );

            var medicalTeam = ( result as OkObjectResult ).Value as MedicalTeamModel;

            Assert.NotNull( medicalTeam );
            Assert.Equal( institute.Id, medicalTeam.Project.InstituteId );
            Assert.Equal( request.AddressLine1, medicalTeam.AddressLine1 );
            Assert.Equal( request.AddressLine2, medicalTeam.AddressLine2 );
            Assert.Equal( request.City, medicalTeam.City );
            Assert.Equal( request.Country, medicalTeam.Country );
            Assert.Equal( request.Name, medicalTeam.Name );
            Assert.Equal( request.Phone, medicalTeam.Phone );
            Assert.Equal( request.PostalCode, medicalTeam.PostalCode );
            Assert.Equal( request.RegionCode, medicalTeam.RegionCode );
            Assert.Equal( request.TimeZone, medicalTeam.TimeZone );
        }
    }
}
