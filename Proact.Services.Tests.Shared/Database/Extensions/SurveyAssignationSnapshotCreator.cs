using Proact.Services.Entities;
using Proact.Services.Models;
using Proact.Services.QueriesServices;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Proact.Services.Tests.Shared {
    public static class SurveyAssignationSnapshotCreator {
        public static DatabaseSnapshotProvider AddSurveyAssignationsToPatient(
            this DatabaseSnapshotProvider snapshotProvider,
            Patient patient, Survey survey, out List<SurveysAssignationRelation> surveysAssignations ) {

            var scheduler = snapshotProvider.ServiceProvider
                .GetQueriesService<ISurveySchedulerQueriesService>()
                .Create( new SurveyScheduler() {
                    Id = Guid.NewGuid(),
                    SurveyId = survey.Id,
                    ExpireTime = DateTime.MaxValue,
                    StartTime = DateTime.MinValue,
                    UserId = patient.UserId,
                    Reccurence = SurveyReccurence.Once
                } );

            snapshotProvider.ServiceProvider.Database.SaveChanges();

            var request = new AssignSurveyToPatientRequest() {
                Reccurence = SurveyReccurence.Once,
                SurveyId = survey.Id,
                UserIds = new List<Guid>() { patient.UserId },
                Schedulers = new List<SurveyScheduler> { scheduler }
            };

            surveysAssignations = snapshotProvider.ServiceProvider
                .GetQueriesService<ISurveyAssignationQueriesService>()
                .AssignSurveyToPatients( request );

            snapshotProvider.ServiceProvider.Database.SaveChanges();

            return snapshotProvider;
        }

        public static DatabaseSnapshotProvider AddSurveyAssignationsToPatients(
            this DatabaseSnapshotProvider snapshotProvider,
            List<Patient> patients,
            Survey survey,
            out List<SurveysAssignationRelation> surveysAssignations ) {

            var schedulers = new List<SurveyScheduler>();

            foreach ( var patient in patients ) {
                var scheduler = snapshotProvider.ServiceProvider
                    .GetQueriesService<ISurveySchedulerQueriesService>()
                    .Create( new SurveyScheduler() {
                        Id = Guid.NewGuid(),
                        SurveyId = survey.Id,
                        ExpireTime = DateTime.MaxValue,
                        StartTime = DateTime.MinValue,
                        UserId = patient.UserId,
                        Reccurence = SurveyReccurence.Once
                    } );

                snapshotProvider.ServiceProvider.Database.SaveChanges();
                schedulers.Add( scheduler );
            }

            var request = new AssignSurveyToPatientRequest() {
                Reccurence = SurveyReccurence.Once,
                SurveyId = survey.Id,
                UserIds = patients.Select( x => x.Id ).ToList(),
                Schedulers = schedulers
            };

            surveysAssignations = snapshotProvider.ServiceProvider
                .GetQueriesService<ISurveyAssignationQueriesService>()
                .AssignSurveyToPatients( request );

            snapshotProvider.ServiceProvider.Database.SaveChanges();

            return snapshotProvider;
        }
    }
}
