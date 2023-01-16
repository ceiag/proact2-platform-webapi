using Proact.Services.Entities;
using Proact.Services.Models;
using Proact.Services.QueriesServices;
using Proact.Services.Tests.Shared;
using System;
using System.Collections.Generic;
using Xunit;

namespace Proact.Services.UnitTests.Surveys.Assignations;
public class AssignSurveyToPatients {
    private List<SurveysAssignationRelation> AddSchedulerWithAssignation(
        ProactServicesProvider servicesProvider,
        DateTime startTime,
        DateTime expireTime,
        SurveyReccurence reccurence ) {
        var user = servicesProvider.Database.Users
            .Add( new User() {
                Id = Guid.NewGuid()
            } ).Entity;

        var survey = servicesProvider.Database.Surveys
            .Add( new Survey() {
                Id = Guid.NewGuid()
            } ).Entity;

        var scheduler = servicesProvider.Database.SurveyScheduler
            .Add( new SurveyScheduler() {
                Id = Guid.NewGuid(),
                Reccurence = reccurence,
                StartTime = startTime,
                ExpireTime = expireTime,
                UserId = user.Id
            } ).Entity;

        var request = new AssignSurveyToPatientRequest() {
            Reccurence = SurveyReccurence.Once,
            SurveyId = survey.Id,
            Schedulers = new List<SurveyScheduler> { scheduler },
            UserIds = new List<Guid> { user.Id }
        };

        return servicesProvider
           .GetQueriesService<ISurveyAssignationQueriesService>()
           .AssignSurveyToPatients( request );
    }

    [Fact]
    public void _CheckCorrectness_Once() {
        var servicesProvider = new ProactServicesProvider();

        var assignations = AddSchedulerWithAssignation(
            servicesProvider,
            DateTime.UtcNow,
            DateTime.UtcNow.AddDays( 3 ),
            SurveyReccurence.Once );

        Assert.Equal( DateTime.UtcNow.Date, assignations[0].StartTime.Date );
        Assert.Equal( DateTime.UtcNow.AddDays( 3 ).Date, assignations[0].ExpireTime.Date );
    }

    [Fact]
    public void _CheckCorrectness_Daily() {
        var servicesProvider = new ProactServicesProvider();

        var assignations = AddSchedulerWithAssignation(
            servicesProvider,
            DateTime.UtcNow,
            DateTime.UtcNow.AddDays( 1 ),
            SurveyReccurence.Daily );

        Assert.Equal( DateTime.UtcNow.Date, assignations[0].StartTime.Date );
        Assert.Equal( DateTime.UtcNow.Date, assignations[0].ExpireTime.Date );
    }

    [Fact]
    public void _CheckCorrectness_Weekly() {
        var servicesProvider = new ProactServicesProvider();

        var assignations = AddSchedulerWithAssignation(
            servicesProvider,
            DateTime.UtcNow,
            DateTime.UtcNow.AddDays( 1 ),
            SurveyReccurence.Weekly );

        Assert.Equal( DateTime.UtcNow.Date, assignations[0].StartTime.Date );
        Assert.Equal( DateTime.UtcNow.AddDays( 7 ).Date, assignations[0].ExpireTime.Date );
    }

    [Fact]
    public void _CheckCorrectness_Monthly() {
        var servicesProvider = new ProactServicesProvider();

        var assignations = AddSchedulerWithAssignation(
            servicesProvider,
            DateTime.UtcNow,
            DateTime.UtcNow.AddDays( 1 ),
            SurveyReccurence.Monthly );

        Assert.Equal( DateTime.UtcNow.Date, assignations[0].StartTime.Date );
        Assert.Equal( DateTime.UtcNow.AddMonths( 1 ).Date, assignations[0].ExpireTime.Date );
    }
}
