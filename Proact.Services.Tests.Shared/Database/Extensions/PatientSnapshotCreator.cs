using Proact.Services.Entities;
using Proact.Services.Models;
using Proact.Services.QueriesServices;
using System;
using System.Collections.Generic;

namespace Proact.Services.Tests.Shared;
public static class PatientSnapshotCreator {
    public static DatabaseSnapshotProvider AddPatientWithRandomValues(
        this DatabaseSnapshotProvider snapshotProvider, MedicalTeam medicalTeam, out Patient patient ) {

        User user = null;
        snapshotProvider.AddUserWithRandomValues( medicalTeam.Project.Institute, out user );

        var patientCreationRequest = new PatientCreateRequest() {
            BirthYear = new Random().Next( 1900, 2000 ),
            Gender = "M",
            Email = "patient@email.com",
            FirstName = "john",
            Lastname = "matrix",
        };

        patient = snapshotProvider.ServiceProvider
            .GetQueriesService<IPatientQueriesService>()
            .Create( user, patientCreationRequest );

        snapshotProvider.ServiceProvider.Database.SaveChanges();

        var assignToMedicalTeamRequest = new AssignPatientToMedicalTeamRequest() {
            Code = "XJJ8AX",
            TreatmentStartDate = DateTime.UtcNow,
            TreatmentEndDate = DateTime.UtcNow.AddMonths( 1 ),
            UserId = user.Id
        };

        snapshotProvider.ServiceProvider
            .GetQueriesService<IPatientQueriesService>()
            .AddToMedicalTeam( medicalTeam.Id, assignToMedicalTeamRequest );

        snapshotProvider.ServiceProvider.Database.SaveChanges();
        return snapshotProvider;
    }

    public static DatabaseSnapshotProvider AddPatientWithRandomValues(
        this DatabaseSnapshotProvider snapshotProvider, Institute institute, out Patient patient ) {

        User user = null;
        snapshotProvider.AddUserWithRandomValues( institute, out user );

        var patientCreationRequest = new PatientCreateRequest() {
            BirthYear = new Random().Next( 1900, 2000 ),
            Gender = "M",
            Email = "patient@email.com",
            FirstName = "john",
            Lastname = "matrix",
        };

        patient = snapshotProvider.ServiceProvider
            .GetQueriesService<IPatientQueriesService>()
            .Create( user, patientCreationRequest );

        snapshotProvider.ServiceProvider.Database.SaveChanges();
        return snapshotProvider;
    }

    public static DatabaseSnapshotProvider AddPatientAnonymousWithRandomValues(
       this DatabaseSnapshotProvider snapshotProvider, MedicalTeam medicalTeam, out Patient patient ) {

        User user = null;
        snapshotProvider.AddAnonymousUserWithRandomValues( medicalTeam.Project.Institute, out user );

        var patientCreationRequest = new PatientCreateRequest() {
            BirthYear = new Random().Next( 1900, 2000 ),
            Gender = "M",
            Email = "patient@email.com",
            FirstName = "",
            Lastname = "",
        };

        patient = snapshotProvider.ServiceProvider
            .GetQueriesService<IPatientQueriesService>()
            .Create( user, patientCreationRequest );

        snapshotProvider.ServiceProvider.Database.SaveChanges();

        var assignToMedicalTeamRequest = new AssignPatientToMedicalTeamRequest() {
            Code = "XJJ8AX",
            TreatmentStartDate = DateTime.UtcNow,
            TreatmentEndDate = DateTime.UtcNow.AddMonths( 1 ),
            UserId = user.Id
        };

        snapshotProvider.ServiceProvider
            .GetQueriesService<IPatientQueriesService>()
            .AddToMedicalTeam( medicalTeam.Id, assignToMedicalTeamRequest );

        snapshotProvider.ServiceProvider.Database.SaveChanges();
        return snapshotProvider;
    }

    public static DatabaseSnapshotProvider AddPatients( this DatabaseSnapshotProvider snapshotProvider,
       Institute institute, MedicalTeam medicalTeam, List<PatientCreateRequest> requests ) {

        foreach ( var creationRequest in requests ) {
            User user = null;
            snapshotProvider.AddAnonymousUserWithRandomValues( institute, out user );

            snapshotProvider.ServiceProvider
                .GetQueriesService<IPatientQueriesService>()
                .Create( user, creationRequest );

            snapshotProvider.ServiceProvider.Database.SaveChanges();

            var assignToMedicalTeamRequest = new AssignPatientToMedicalTeamRequest() {
                Code = "XJJ8AX",
                TreatmentStartDate = DateTime.UtcNow,
                TreatmentEndDate = DateTime.UtcNow.AddMonths( 1 ),
                UserId = user.Id
            };

            snapshotProvider.ServiceProvider
                .GetQueriesService<IPatientQueriesService>()
                .AddToMedicalTeam( medicalTeam.Id, assignToMedicalTeamRequest );
        }

        snapshotProvider.ServiceProvider.Database.SaveChanges();
        return snapshotProvider;
    }
}
