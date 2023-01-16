using Proact.Services.Entities;
using Proact.Services.Models;
using Proact.Services.QueriesServices;
using Proact.Services.Tests.Shared;
using System;
using System.Collections.Generic;
using Xunit;

namespace Proact.Services.UnitTests.Surveys.Assignations;
public class GetExpiresWithinTwoDays_Assignation_Once {
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
    public void _Expire_Today_Must_Return_One() {
        var servicesProvider = new ProactServicesProvider();
        var assignations = AddSchedulerWithAssignation(
            servicesProvider, DateTime.UtcNow, DateTime.UtcNow, SurveyReccurence.Once );

        var expiringAssignations = servicesProvider
            .GetQueriesService<ISurveyAssignationQueriesService>()
            .GetExpiresWithinTwoDays();

        Assert.Single( expiringAssignations );
    }

    [Fact]
    public void _Expire_Today_Completed_Must_Return_Zero() {
        var servicesProvider = new ProactServicesProvider();
        var assignations = AddSchedulerWithAssignation(
            servicesProvider, DateTime.UtcNow, DateTime.UtcNow, SurveyReccurence.Once );

        assignations[0].Completed = true;

        servicesProvider.Database.SurveysAssignationsRelations
            .Update( assignations[0] );

        servicesProvider.Database.SaveChanges();

        var expiringAssignations = servicesProvider
            .GetQueriesService<ISurveyAssignationQueriesService>()
            .GetExpiresWithinTwoDays();

        Assert.Empty( expiringAssignations );
    }

    [Fact]
    public void _Expire_Tomorrow_Must_Return_One() {
        var servicesProvider = new ProactServicesProvider();
        var assignations = AddSchedulerWithAssignation(
            servicesProvider, DateTime.UtcNow, DateTime.UtcNow.AddDays( 1 ), SurveyReccurence.Once );

        var expiringAssignations = servicesProvider
            .GetQueriesService<ISurveyAssignationQueriesService>()
            .GetExpiresWithinTwoDays();

        Assert.Single( expiringAssignations );
    }

    [Fact]
    public void _Expire_InTwoDays_Must_Return_One() {
        var servicesProvider = new ProactServicesProvider();
        var assignations = AddSchedulerWithAssignation(
            servicesProvider, DateTime.UtcNow, DateTime.UtcNow.AddDays( 2 ), SurveyReccurence.Once );

        var expiringAssignations = servicesProvider
            .GetQueriesService<ISurveyAssignationQueriesService>()
            .GetExpiresWithinTwoDays();

        Assert.Single( expiringAssignations );
    }

    [Fact]
    public void _Expire_InThreeDays_Must_Return_Zero() {
        var servicesProvider = new ProactServicesProvider();
        var assignations = AddSchedulerWithAssignation(
            servicesProvider, DateTime.UtcNow, DateTime.UtcNow.AddDays( 3 ), SurveyReccurence.Once );

        var expiringAssignations = servicesProvider
            .GetQueriesService<ISurveyAssignationQueriesService>()
            .GetExpiresWithinTwoDays();

        Assert.Empty( expiringAssignations );
    }

    [Fact]
    public void _Expired_Tomorrow_Must_Return_Zero() {
        var servicesProvider = new ProactServicesProvider();
        var assignations = AddSchedulerWithAssignation(
            servicesProvider, DateTime.UtcNow.AddDays( -2 ), 
            DateTime.UtcNow.AddDays( -1 ), SurveyReccurence.Once );

        var expiringAssignations = servicesProvider
            .GetQueriesService<ISurveyAssignationQueriesService>()
            .GetExpiresWithinTwoDays();

        Assert.Empty( expiringAssignations );
    }
}
