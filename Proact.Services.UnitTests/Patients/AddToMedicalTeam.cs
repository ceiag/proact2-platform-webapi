using Proact.Services.Entities;
using Proact.Services.Tests.Shared;
using Xunit;
using Proact.Services.QueriesServices;
using System;

namespace Proact.Services.UnitTests.Patients;
public class AddToMedicalTeam {
    [Fact]
    public void _CheckCorrectness_After_First_Assignation() {
        User instituteAdmin = null;
        Institute institute = null;
        Project project = null;
        MedicalTeam medicalTeam = null;
        Medic medic = null;
        Patient patient = null;

        var servicesProvider = new ProactServicesProvider();
        new DatabaseSnapshotProvider( servicesProvider )
            .AddInstituteWithRandomValues( out institute )
            .AddInstituteAdminWithRandomValues( institute, out instituteAdmin )
            .AddProjectWithRandomValues( institute, out project )
            .AddMedicalTeamWithRandomValues( project, out medicalTeam )
            .AddMedicWithRandomValues( medicalTeam, out medic )
            .AddAdminToMedicalTeam( medic, medicalTeam )
            .AddPatientWithRandomValues( institute, out patient );

        var request = new AssignPatientToMedicalTeamRequest() {
            Code = "CJHD88",
            TreatmentStartDate = DateTime.UtcNow,
            TreatmentEndDate = DateTime.UtcNow.AddDays( 2 ),
            UserId = patient.UserId
        };

        servicesProvider
            .GetQueriesService<IPatientQueriesService>()
            .AddToMedicalTeam( medicalTeam.Id, request );

        var treatmentsHistory = servicesProvider
            .GetQueriesService<IPatientQueriesService>()
            .GetTreatmentHistoriesByPatientId( patient.Id );

        var patientSavedOnDb = servicesProvider
            .GetQueriesService<IPatientQueriesService>()
            .Get( patient.UserId );

        Assert.Single( treatmentsHistory );
        Assert.Equal( request.Code, patientSavedOnDb.Code );
        Assert.Equal( request.TreatmentStartDate, patientSavedOnDb.TreatmentStartDate );
        Assert.Equal( request.TreatmentEndDate, patientSavedOnDb.TreatmentEndDate );
        Assert.Equal( medicalTeam.Id, patient.MedicalTeamId );
    }

    [Fact]
    public void _CheckCorrectness_After_Second_Assignation() {
        User instituteAdmin = null;
        Institute institute = null;
        Project project = null;
        MedicalTeam medicalTeam_0 = null;
        MedicalTeam medicalTeam_1 = null;
        Medic medic_0 = null;
        Medic medic_1 = null;
        Patient patient = null;

        var servicesProvider = new ProactServicesProvider();
        new DatabaseSnapshotProvider( servicesProvider )
            .AddInstituteWithRandomValues( out institute )
            .AddInstituteAdminWithRandomValues( institute, out instituteAdmin )
            .AddProjectWithRandomValues( institute, out project )
            .AddMedicalTeamWithRandomValues( project, out medicalTeam_0 )
            .AddMedicalTeamWithRandomValues( project, out medicalTeam_1 )
            .AddMedicWithRandomValues( medicalTeam_0, out medic_0 )
            .AddMedicWithRandomValues( medicalTeam_1, out medic_1 )
            .AddAdminToMedicalTeam( medic_0, medicalTeam_0 )
            .AddAdminToMedicalTeam( medic_1, medicalTeam_1 )
            .AddPatientWithRandomValues( institute, out patient );

        var request_0 = new AssignPatientToMedicalTeamRequest() {
            Code = "CJHD88",
            TreatmentStartDate = DateTime.UtcNow,
            TreatmentEndDate = DateTime.UtcNow.AddDays( 2 ),
            UserId = patient.UserId
        };

        servicesProvider
            .GetQueriesService<IPatientQueriesService>()
            .AddToMedicalTeam( medicalTeam_0.Id, request_0 );
        
        var request_1 = new AssignPatientToMedicalTeamRequest() {
            Code = "CYCA82",
            TreatmentStartDate = DateTime.UtcNow,
            TreatmentEndDate = DateTime.UtcNow.AddDays( 24 ),
            UserId = patient.UserId
        };

        servicesProvider
            .GetQueriesService<IPatientQueriesService>()
            .AddToMedicalTeam( medicalTeam_1.Id, request_1 );

        var treatmentsHistory = servicesProvider
            .GetQueriesService<IPatientQueriesService>()
            .GetTreatmentHistoriesByPatientId( patient.Id );

        var patientSavedOnDb = servicesProvider
            .GetQueriesService<IPatientQueriesService>()
            .Get( patient.UserId );

        Assert.Equal( 2, treatmentsHistory.Count );
        Assert.Equal( request_1.Code, patientSavedOnDb.Code );
        Assert.Equal( request_1.TreatmentStartDate, patientSavedOnDb.TreatmentStartDate );
        Assert.Equal( request_1.TreatmentEndDate, patientSavedOnDb.TreatmentEndDate );
        Assert.Equal( medicalTeam_1.Id, patient.MedicalTeamId );
    }
}
