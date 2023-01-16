using Microsoft.AspNetCore.Mvc;
using Proact.Services.AuthorizationPolicies;
using Proact.Services.Entities;
using Proact.Services.Models;
using Proact.Services.Tests.Shared;
using Xunit;

namespace Proact.Services.FunctionalTests.Patients {
    public class CreatePatient {
        [Fact]
        public void CreatePatient_ConsistencyCheck() {
            var servicesProvider = new ProactServicesProvider();
            Institute institute = null;
            User instituteAdmin = null;
            Project project = null;

            new DatabaseSnapshotProvider( servicesProvider )
                .AddInstituteWithRandomValues( out institute )
                .AddInstituteAdminWithRandomValues( institute, out instituteAdmin )
                .AddProjectWithRandomValues( institute, out project );

            var request = new PatientCreateRequest() {
                Email = "patient@patient.com",
                FirstName = "patient_name",
                Lastname = "patient_surname",
                BirthYear = 1980,
                Gender = "M",
            };

            var patientController = new PatientControllerProvider(
                servicesProvider, instituteAdmin, Roles.InstituteAdmin );
            var result = patientController.Controller.CreatePatient( request );

            var patient = ( result as OkObjectResult ).Value as PatientModel;

            Assert.NotNull( patient );
            Assert.Equal( instituteAdmin.InstituteId, patient.InstituteId );
            Assert.Equal( request.BirthYear, patient.BirthYear );
            Assert.Equal( request.Gender, patient.Gender );
        }
    }
}