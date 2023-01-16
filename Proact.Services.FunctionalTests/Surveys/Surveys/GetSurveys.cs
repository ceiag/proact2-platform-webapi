using Microsoft.AspNetCore.Mvc;
using Proact.Services.AuthorizationPolicies;
using Proact.Services.Entities;
using Proact.Services.FunctionalTests.Surveys.Assignations;
using Proact.Services.Models.SurveyStats;
using Proact.Services.Models;
using Proact.Services.Tests.Shared;
using System;
using System.Collections.Generic;
using Xunit;

namespace Proact.Services.FunctionalTests.Surveys.Surveys;
public class GetSurveys {
    [Fact]
    public void _CheckCorrectness_After_Assignation() {
        User instituteAdmin = null;
        Institute institute = null;
        Project project = null;
        MedicalTeam medicalTeam = null;
        Medic medic = null;
        Patient patient_0 = null;
        SurveyQuestionsSet questionsSet = null;
        Survey survey = null;

        var servicesProvider = new ProactServicesProvider();
        new DatabaseSnapshotProvider( servicesProvider )
            .AddInstituteWithRandomValues( out institute )
            .AddInstituteAdminWithRandomValues( institute, out instituteAdmin )
            .AddProjectWithRandomValues( institute, out project )
            .AddMedicalTeamWithRandomValues( project, out medicalTeam )
            .AddMedicWithRandomValues( medicalTeam, out medic )
            .AddPatientWithRandomValues( medicalTeam, out patient_0 )
            .AddQuestionsSetWithRandomValues( project, out questionsSet, true )
            .AddSurveyWithRandomValues( project, out survey );

        var request = new CreateScheduledSurveyRequest() {
            SurveyId = survey.Id,
            StartTime = DateTime.UtcNow.Date,
            ExpireTime = DateTime.UtcNow.Date.AddDays( 7 ).Date,
            Reccurence = SurveyReccurence.Daily,
            UserIds = new List<Guid> { patient_0.UserId }
        };

        new SurveyAssignationControllerProvider(
            servicesProvider, medic.User, Roles.MedicalProfessional )
                .Controller.AssignSurveyToPatients( survey.Id, request );

        var provider = new SurveyControllerProvider(
            servicesProvider, patient_0.User, Roles.MedicalProfessional );
        var apiResult = provider.Controller.GetSurveys( project.Id );
        var surveysApiResult = ( apiResult as OkObjectResult ).Value as List<SurveyModel>;

        Assert.Single( surveysApiResult );
        Assert.Single( surveysApiResult[0].AssignedPatients );
        Assert.Equal( request.StartTime.Date, surveysApiResult[0].StartTime?.Date );
        Assert.Equal( request.ExpireTime.Date, surveysApiResult[0].ExpireTime?.Date );
        Assert.Equal( request.Reccurence, surveysApiResult[0].Reccurence );
        Assert.Equal( SurveyState.PUBLISHED, surveysApiResult[0].SurveyState );
    }

    [Fact]
    public void _MustReturn_NotFound() {
        User instituteAdmin = null;
        Institute institute = null;
        Project project = null;
        MedicalTeam medicalTeam = null;
        Medic medic = null;
        Patient patient_0 = null;
        SurveyQuestionsSet questionsSet = null;
        Survey survey = null;

        var servicesProvider = new ProactServicesProvider();
        new DatabaseSnapshotProvider( servicesProvider )
            .AddInstituteWithRandomValues( out institute )
            .AddInstituteAdminWithRandomValues( institute, out instituteAdmin )
            .AddProjectWithRandomValues( institute, out project )
            .AddMedicalTeamWithRandomValues( project, out medicalTeam )
            .AddMedicWithRandomValues( medicalTeam, out medic )
            .AddPatientWithRandomValues( medicalTeam, out patient_0 )
            .AddQuestionsSetWithRandomValues( project, out questionsSet, true )
            .AddSurveyWithRandomValues( project, out survey );

        var provider = new SurveyControllerProvider(
            servicesProvider, patient_0.User, Roles.MedicalProfessional );
        var apiResult = provider.Controller.GetSurvey( Guid.NewGuid() );
        var surveysApiResult = ( apiResult as NotFoundObjectResult ).Value;

        Assert.NotNull( surveysApiResult );
    }
}
