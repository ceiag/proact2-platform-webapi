using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Proact.Services.AuthorizationPolicies;
using Proact.Services.Entities;
using Proact.Services.Models;
using Proact.Services.Tests.Shared;
using System;
using Xunit;

namespace Proact.Services.FunctionalTests.Patients;
public class AssignPatientToMedicalTeam {
    [Fact]
    public void _ConsistencyCheck() {
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

        var provider = new PatientControllerProvider(
            servicesProvider, medic.User, Roles.MedicalTeamAdmin );
        var apiResult = provider.Controller
            .AssignPatientToMedicalTeam( medicalTeam.Id, request );

        Assert.Equal( 200, ( apiResult as OkResult ).StatusCode );

        var patientAssignedApiResult = ( provider.Controller
            .GetPatient( medicalTeam.Id, patient.UserId ) as OkObjectResult )
            .Value as PatientModel;

        Assert.Equal( request.Code, patientAssignedApiResult.Code );
        Assert.Equal( medicalTeam.Id, patientAssignedApiResult.MedicalTeam[0].MedicalTeamId );
        Assert.Equal( request.TreatmentStartDate, patientAssignedApiResult.TreatmentStartDate );
        Assert.Equal( request.TreatmentEndDate, patientAssignedApiResult.TreatmentEndDate );
    }

    [Fact]
    public void _Must_Override_The_Existing_One() {
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

        var provider_0 = new PatientControllerProvider(
            servicesProvider, medic_0.User, Roles.MedicalTeamAdmin );
        provider_0.Controller.AssignPatientToMedicalTeam( medicalTeam_0.Id, request_0 );

        var request_1 = new AssignPatientToMedicalTeamRequest() {
            Code = "BAHV92",
            TreatmentStartDate = DateTime.UtcNow,
            TreatmentEndDate = DateTime.UtcNow.AddDays( 4 ),
            UserId = patient.UserId
        };

        var provider_1 = new PatientControllerProvider(
            servicesProvider, medic_1.User, Roles.MedicalTeamAdmin );
        provider_1.Controller.AssignPatientToMedicalTeam( medicalTeam_1.Id, request_1 );

        var patientAssignedApiResult = ( provider_0.Controller
           .GetPatient( medicalTeam_1.Id, patient.UserId ) as OkObjectResult )
           .Value as PatientModel;

        Assert.Equal( request_1.Code, patientAssignedApiResult.Code );
        Assert.Equal( medicalTeam_1.Id, patientAssignedApiResult.MedicalTeam[0].MedicalTeamId );
        Assert.Equal( request_1.TreatmentStartDate, patientAssignedApiResult.TreatmentStartDate );
        Assert.Equal( request_1.TreatmentEndDate, patientAssignedApiResult.TreatmentEndDate );
    }
}
