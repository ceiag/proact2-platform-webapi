using Microsoft.AspNetCore.Mvc;
using Proact.Services.AuthorizationPolicies;
using Proact.Services.Entities;
using Proact.Services.Models;
using Proact.Services.Tests.Shared;
using System.Collections.Generic;
using Xunit;

namespace Proact.Services.FunctionalTests.Patients {
    public class GetPatientsFromMedicalTeam {
        [Fact]
        public void _TwoAnonymousTwoWithName() {
            User instituteAdmin = null;
            Institute institute = null;
            Project project = null;
            MedicalTeam medicalTeam = null;
            Patient patient_0 = null;
            Patient patient_1 = null;
            Patient patient_2 = null;
            Patient patient_3 = null;
            Medic medic = null;

            var servicesProvider = new ProactServicesProvider();
            new DatabaseSnapshotProvider( servicesProvider )
                .AddInstituteWithRandomValues( out institute )
                .AddInstituteAdminWithRandomValues( institute, out instituteAdmin )
                .AddProjectWithRandomValues( institute, out project )
                .AddMedicalTeamWithRandomValues( project, out medicalTeam )
                .AddPatientAnonymousWithRandomValues( medicalTeam, out patient_0 )
                .AddPatientAnonymousWithRandomValues( medicalTeam, out patient_1 )
                .AddPatientWithRandomValues( medicalTeam, out patient_2 )
                .AddPatientWithRandomValues( medicalTeam, out patient_3 )
                .AddMedicWithRandomValues( medicalTeam, out medic );

            var patientsController = new PatientControllerProvider(
                servicesProvider, medic.User, Roles.MedicalProfessional );
            var result = patientsController.Controller.GetPatientsFromMedicalTeam( medicalTeam.Id );

            var patients = ( result as OkObjectResult ).Value as List<PatientModel>;

            Assert.Equal( 4, patients.Count );
            Assert.Equal( patient_0.Code, patients[0].Name );
            Assert.Equal( patient_1.Code, patients[1].Name );
            Assert.Equal( patient_2.User.Name, patients[2].Name );
            Assert.Equal( patient_3.User.Name, patients[3].Name );
        }
    }
}
