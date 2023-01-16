using Microsoft.AspNetCore.Mvc;
using Proact.Services.AuthorizationPolicies;
using Proact.Services.Entities;
using Proact.Services.Models;
using Proact.Services.Tests.Shared;
using System;
using System.Collections.Generic;
using Xunit;

namespace Proact.Services.FunctionalTests.Surveys.Assignations {
    public class AssignSurveyToPatients {
        [Fact]
        public void AssignSurveyToPatients_Once_ConsistencyCheck() {
            User instituteAdmin = null;
            Institute institute = null;
            Project project = null;
            MedicalTeam medicalTeam = null;
            Medic medic = null;
            Patient patient_0 = null;
            Patient patient_1 = null;
            SurveyQuestionsSet questionsSet = null;
            Survey survey = null;

            var servicesProvider = new ProactServicesProvider();
            new DatabaseSnapshotProvider( servicesProvider )
                .AddInstituteWithRandomValues( out institute )
                .AddInstituteAdminWithRandomValues( institute, out instituteAdmin )
                .AddProjectWithRandomValues( institute, out project )
                .AddMedicalTeamWithRandomValues( project, out medicalTeam )
                .AddMedicWithRandomValues( medicalTeam, out medic )
                .AddAdminToMedicalTeam( medic, medicalTeam )
                .AddPatientWithRandomValues( medicalTeam, out patient_0 )
                .AddPatientWithRandomValues( medicalTeam, out patient_1 )
                .AddQuestionsSetWithRandomValues( project, out questionsSet, true )
                .AddSurveyWithRandomValues( project, out survey );

            var request = new CreateScheduledSurveyRequest() {
                StartTime = DateTime.UtcNow,
                ExpireTime = DateTime.UtcNow.AddYears( 1 ),
                Reccurence = SurveyReccurence.Once,
                SurveyId = survey.Id,
                UserIds = new List<Guid>() { patient_0.UserId, patient_1.UserId }
            };

            var provider = new SurveyAssignationControllerProvider(
                servicesProvider, medic.User, Roles.MedicalTeamAdmin );
            var apiResult = provider.Controller.AssignSurveyToPatients( survey.Id, request );

            Assert.Equal( 200, ( apiResult as OkResult ).StatusCode );

            var assignationResults = ( provider.Controller
                .GetPatientsAssignedToSurvey( survey.Id ) as OkObjectResult )
                .Value as List<SurveyAssignationModel>;

            Assert.Equal( 2, assignationResults.Count );
            Assert.False( assignationResults[0].Expired );
            Assert.Equal( SurveyReccurence.Once, assignationResults[0].Reccurence );
            Assert.Equal( SurveyReccurence.Once, assignationResults[1].Reccurence );
            Assert.Equal( request.StartTime.Date, assignationResults[0].Scheduler.StartTime.Date );
            Assert.Equal( request.ExpireTime.Date, assignationResults[0].Scheduler.ExpireTime.Date );
        }

        [Fact]
        public void AssignSurveyToPatients_Weekly_ConsistencyCheck() {
            User instituteAdmin = null;
            Institute institute = null;
            Project project = null;
            MedicalTeam medicalTeam = null;
            Medic medic = null;
            Patient patient_0 = null;
            Patient patient_1 = null;
            SurveyQuestionsSet questionsSet = null;
            Survey survey = null;

            var servicesProvider = new ProactServicesProvider();
            new DatabaseSnapshotProvider( servicesProvider )
                .AddInstituteWithRandomValues( out institute )
                .AddInstituteAdminWithRandomValues( institute, out instituteAdmin )
                .AddProjectWithRandomValues( institute, out project )
                .AddMedicalTeamWithRandomValues( project, out medicalTeam )
                .AddMedicWithRandomValues( medicalTeam, out medic )
                .AddAdminToMedicalTeam( medic, medicalTeam )
                .AddPatientWithRandomValues( medicalTeam, out patient_0 )
                .AddPatientWithRandomValues( medicalTeam, out patient_1 )
                .AddQuestionsSetWithRandomValues( project, out questionsSet, true )
                .AddSurveyWithRandomValues( project, out survey );

            var request = new CreateScheduledSurveyRequest() {
                StartTime = DateTime.UtcNow,
                ExpireTime = DateTime.UtcNow.AddYears( 1 ),
                Reccurence = SurveyReccurence.Weekly,
                SurveyId = survey.Id,
                UserIds = new List<Guid>() { patient_0.UserId, patient_1.UserId }
            };

            var provider = new SurveyAssignationControllerProvider(
                servicesProvider, medic.User, Roles.MedicalTeamAdmin );
            var apiResult = provider.Controller.AssignSurveyToPatients( survey.Id, request );

            Assert.Equal( 200, ( apiResult as OkResult ).StatusCode );

            var assignationResults = ( provider.Controller
                .GetPatientsAssignedToSurvey( survey.Id ) as OkObjectResult )
                .Value as List<SurveyAssignationModel>;

            Assert.Equal( 2, assignationResults.Count );
            Assert.Equal( SurveyReccurence.Weekly, assignationResults[0].Reccurence );
            Assert.Equal( request.StartTime.Date, assignationResults[0].StartTime.Date );
            Assert.Equal( request.StartTime.Date.AddDays( 7 ), assignationResults[0].StartTime.Date.AddDays( 7 ) );
            Assert.Equal( request.StartTime.Date, assignationResults[0].Scheduler.StartTime.Date );
            Assert.Equal( request.ExpireTime.Date, assignationResults[0].Scheduler.ExpireTime.Date );
        }
    }
}
