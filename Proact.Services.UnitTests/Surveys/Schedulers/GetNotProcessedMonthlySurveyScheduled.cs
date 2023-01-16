using Proact.Services.Entities;
using Proact.Services.QueriesServices;
using Proact.Services.Tests.Shared;
using System;
using Xunit;

namespace Proact.Services.UnitTests.Surveys.Schedulers;
public class GetNotProcessedMonthlySurveyScheduled {
    [Fact]
    public void _MustReturnTwoSchedulers() {
        var servicesProvider = new ProactServicesProvider();

        var user = servicesProvider.Database.Users
            .Add( new User() {
                Id = Guid.NewGuid()
            } ).Entity;

        var survey = servicesProvider.Database.Surveys
            .Add( new Survey() {
                Id = Guid.NewGuid()
            } ).Entity;

        var onceSchedulerMustBeExecuted_0 = servicesProvider.Database.SurveyScheduler
            .Add( new SurveyScheduler() {
                Id = Guid.NewGuid(),
                StartTime = DateTime.UtcNow.AddMonths( -1 ),
                ExpireTime = DateTime.UtcNow.AddYears( 1 ),
                Reccurence = SurveyReccurence.Monthly,
                LastSubmission = DateTime.MinValue,
                User = user,
                Survey = survey
            } ).Entity;

        var onceSchedulerMustBeExecuted_1 = servicesProvider.Database.SurveyScheduler
            .Add( new SurveyScheduler() {
                Id = Guid.NewGuid(),
                StartTime = DateTime.UtcNow.AddMonths( -2 ),
                ExpireTime = DateTime.UtcNow.AddYears( 1 ),
                Reccurence = SurveyReccurence.Monthly,
                LastSubmission = DateTime.UtcNow.AddMonths( -1 ),
                User = user,
                Survey = survey
            } ).Entity;

        var onceSchedulerAlreadyExecuted_0 = servicesProvider.Database.SurveyScheduler
            .Add( new SurveyScheduler() {
                Id = Guid.NewGuid(),
                StartTime = DateTime.UtcNow.AddMonths( -2 ),
                ExpireTime = DateTime.UtcNow.AddYears( 1 ),
                Reccurence = SurveyReccurence.Monthly,
                LastSubmission = DateTime.UtcNow.AddMinutes( -2 ),
                User = user,
                Survey = survey
            } ).Entity;

        var onceSchedulerAlreadyExecuted_1 = servicesProvider.Database.SurveyScheduler
            .Add( new SurveyScheduler() {
                Id = Guid.NewGuid(),
                StartTime = DateTime.UtcNow.AddMonths( -2 ).AddDays( 1 ),
                ExpireTime = DateTime.UtcNow.AddYears( 1 ),
                Reccurence = SurveyReccurence.Monthly,
                LastSubmission = DateTime.UtcNow.AddMonths( -1 ),
                User = user,
                Survey = survey
            } ).Entity;

        var onceSchedulerExpiredAndExecuted = servicesProvider.Database.SurveyScheduler
            .Add( new SurveyScheduler() {
                Id = Guid.NewGuid(),
                StartTime = DateTime.UtcNow.AddMonths( -2 ),
                ExpireTime = DateTime.UtcNow.AddMonths( -1 ),
                Reccurence = SurveyReccurence.Monthly,
                LastSubmission = DateTime.UtcNow.AddMonths( -1 ),
                User = user,
                Survey = survey
            } ).Entity;

        var onceSchedulerExpiredAndNotExecuted = servicesProvider.Database.SurveyScheduler
            .Add( new SurveyScheduler() {
                Id = Guid.NewGuid(),
                StartTime = DateTime.UtcNow.AddMonths( -2 ),
                ExpireTime = DateTime.UtcNow.AddMonths( -1 ),
                Reccurence = SurveyReccurence.Monthly,
                LastSubmission = DateTime.MinValue,
                User = user,
                Survey = survey
            } ).Entity;

        servicesProvider.Database.SaveChanges();

        var schedulersResult = servicesProvider
            .GetQueriesService<ISurveySchedulerQueriesService>()
            .GetNotProcessedMonthlySurveyScheduled();

        Assert.Equal( 2, schedulersResult.Count );
    }
}
