using Microsoft.AspNetCore.Mvc;
using Proact.Services.AuthorizationPolicies;
using Proact.Services.Entities;
using Proact.Services.Models;
using Proact.Services.QueriesServices;
using Proact.Services.Tests.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Proact.Services.FunctionalTests.Patients {
    public class SuspendExpiredPatients {
        [Fact]
        public void CreatePatient_ConsistencyCheck() {
            var servicesProvider = new ProactServicesProvider();
            Institute institute = null;
            User instituteAdmin = null;
            Project project = null;
            MedicalTeam medicalTeam = null;

            var expiredPatientsCreationRequests = new List<PatientCreateRequest>();
            for ( int i = 0; i < 10; i++ ) {
                var request = new PatientCreateRequest() {
                    Email = $"patient{i}@patient.com",
                    FirstName = $"patient_name_{i}",
                    Lastname = $"patient_surname_{i}",
                    BirthYear = 1980,
                    Gender = "M",
                };
            }

            var notExpiredPatientsCreationRequests = new List<PatientCreateRequest>();
            for ( int i = 0; i < 5; i++ ) {
                var request = new PatientCreateRequest() {
                    Email = $"patient{i}@patient.com",
                    FirstName = $"patient_name_{i}",
                    Lastname = $"patient_surname_{i}",
                    BirthYear = 1980,
                    Gender = "M",
                };
            }

            new DatabaseSnapshotProvider( servicesProvider )
                .AddInstituteWithRandomValues( out institute )
                .AddInstituteAdminWithRandomValues( institute, out instituteAdmin )
                .AddProjectWithRandomValues( institute, out project )
                .AddMedicalTeamWithRandomValues( project, out medicalTeam )
                .AddPatients( institute, medicalTeam, expiredPatientsCreationRequests )
                .AddPatients( institute, medicalTeam, notExpiredPatientsCreationRequests );

            var patientController = new PatientControllerProvider(
                servicesProvider, instituteAdmin, Roles.InstituteAdmin );
            var result = patientController.Controller.SuspendExpiredPatients();

            Assert.Equal( 200, ( result as OkResult ).StatusCode );

            var allPatients = servicesProvider
                .GetQueriesService<IPatientQueriesService>()
                .GetFromMedicalTeam( medicalTeam.Id );

            var allPatientSuspended = allPatients.Where( 
                x => x.User.State == UserSubscriptionState.Suspended ).ToList();
            var allPatientStillActive = allPatients.Where(
                x => x.User.State == UserSubscriptionState.Active ).ToList();

            Assert.Equal( expiredPatientsCreationRequests.Count, allPatientSuspended.Count );
            Assert.Equal( notExpiredPatientsCreationRequests.Count, allPatientStillActive.Count );
        }
    }
}
