using Microsoft.AspNetCore.Mvc;
using Proact.Services.AuthorizationPolicies;
using Proact.Services.Entities;
using Proact.Services.Models;
using Proact.Services.Tests.Shared;
using System.Collections.Generic;
using Xunit;

namespace Proact.Services.FunctionalTests.Patients {
    public class GetPatientsFromProject {
        [Fact]
        public void GetPatientsFromProject_MustReturnOnly3Patients() {
            User instituteAdmin_0 = null;
            Institute institute = null;
            Project project_0 = null;
            Project project_1 = null;
            MedicalTeam medicalTeam_0 = null;
            MedicalTeam medicalTeam_1 = null;
            MedicalTeam medicalTeam_2 = null;
            MedicalTeam medicalTeam_3 = null;
            Patient patient_0 = null;
            Patient patient_1 = null;
            Patient patient_2 = null;
            Patient patient_3 = null;
            Medic medic = null;

            var servicesProvider = new ProactServicesProvider();
            new DatabaseSnapshotProvider( servicesProvider )
                .AddInstituteWithRandomValues( out institute )
                .AddInstituteAdminWithRandomValues( institute, out instituteAdmin_0 )
                .AddProjectWithRandomValues( institute, out project_0 )
                .AddProjectWithRandomValues( institute, out project_1 )
                .AddMedicalTeamWithRandomValues( project_0, out medicalTeam_0 )
                .AddMedicalTeamWithRandomValues( project_0, out medicalTeam_1 )
                .AddMedicalTeamWithRandomValues( project_1, out medicalTeam_2 )
                .AddMedicalTeamWithRandomValues( project_1, out medicalTeam_3 )
                .AddPatientWithRandomValues( medicalTeam_0, out patient_0 )
                .AddPatientWithRandomValues( medicalTeam_0, out patient_1 )
                .AddPatientWithRandomValues( medicalTeam_1, out patient_2 )
                .AddPatientWithRandomValues( medicalTeam_2, out patient_3 )
                .AddMedicWithRandomValues( medicalTeam_0, out medic );

            var patientsController = new PatientControllerProvider(
                servicesProvider, medic.User, Roles.MedicalProfessional );
            var result = patientsController.Controller.GetPatientsFromProject( project_0.Id );

            var patients = ( result as OkObjectResult ).Value as List<PatientModel>;

            Assert.Equal( 3, patients.Count );
        }
    }
}
