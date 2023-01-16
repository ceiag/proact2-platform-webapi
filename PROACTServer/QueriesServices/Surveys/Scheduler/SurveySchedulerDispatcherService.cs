using Proact.Services.Entities;
using Proact.Services.Models;
using Proact.Services.PushNotifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proact.Services.QueriesServices.Surveys.Scheduler;

public class SurveySchedulerDispatcherService : ISurveySchedulerDispatcherService {
    private readonly ISurveySchedulerQueriesService _surveySchedulerQueriesService;
    private readonly ISurveyAssignationQueriesService _surveyAssignationQueriesService;
    private readonly INotificationProviderService _notificationProviderService;
    private readonly IUserNotificationSettingsQueriesService _userNotificationSettingsQueriesService;

    public SurveySchedulerDispatcherService(
        ISurveySchedulerQueriesService surveySchedulerQueriesService,
        ISurveyAssignationQueriesService surveyAssignationQueriesService,
        INotificationProviderService notificationProviderService,
        IUserNotificationSettingsQueriesService userNotificationSettingsQueriesService ) {
        _surveySchedulerQueriesService = surveySchedulerQueriesService;
        _surveyAssignationQueriesService = surveyAssignationQueriesService;
        _notificationProviderService = notificationProviderService;
        _userNotificationSettingsQueriesService = userNotificationSettingsQueriesService;
    }

    public async Task SendSurveyToPatientsForToday() {
        await ProcessOnceSchedulersThatStartToday();
        await ProcessDailySchedulersThatStartToday();
        await ProcessWeeklySchedulersThatStartToday();
        await ProcessMonthlySchedulersThatStartToday();
    }

    private async Task ProcessOnceSchedulersThatStartToday() {
        var notProcessedSchedulers = _surveySchedulerQueriesService
            .GetNotProcessedOnceSurveyScheduled();

        await SendSurveyToPatientsNow( notProcessedSchedulers );
    }

    private async Task ProcessDailySchedulersThatStartToday() {
        var notProcessedSchedulers = _surveySchedulerQueriesService
            .GetNotProcessedDailySurveyScheduled();

        await SendSurveyToPatientsNow( notProcessedSchedulers );
    }

    private async Task ProcessWeeklySchedulersThatStartToday() {
        var notProcessedSchedulers = _surveySchedulerQueriesService
            .GetNotProcessedWeeklySurveyScheduled();

        await SendSurveyToPatientsNow( notProcessedSchedulers );
    }

    private async Task ProcessMonthlySchedulersThatStartToday() {
        var notProcessedSchedulers = _surveySchedulerQueriesService
            .GetNotProcessedMonthlySurveyScheduled();

        await SendSurveyToPatientsNow( notProcessedSchedulers );
    }

    private List<Guid> GetPlayerIdsFromSchedulers( List<SurveyScheduler> schedulers ) {
        var userNotSettings = _userNotificationSettingsQueriesService
            .GetByUserIds( schedulers.Select( x => x.UserId ).ToList() );

        var playerIds = new List<Guid>();
        foreach ( var notSetting in userNotSettings ) {
            playerIds.AddRange( notSetting.Devices.Select( x => x.PlayerId ) );
        }

        return playerIds;
    }

    public async Task SendSurveyToPatientsNow( List<SurveyScheduler> schedulers ) {
        foreach ( var scheduler in schedulers ) {
            var request = new AssignSurveyToPatientRequest() {
                SurveyId = scheduler.SurveyId,
                UserIds = new List<Guid>() { scheduler.UserId },
                Schedulers = new List<SurveyScheduler>() { scheduler }
            };

            _surveyAssignationQueriesService.AssignSurveyToPatients( request );
        }

        _surveySchedulerQueriesService.SetSchedulersAsProcessed( schedulers );

        try {
            var playerIds = GetPlayerIdsFromSchedulers( schedulers );
            await _notificationProviderService
                .SendSurveyNotificationToDevices( playerIds, "new_survey_to_compile" );
        }
        catch ( Exception e ) { }
    }

    public async Task SendReminderForExpiringSurveys() {
        var expiringAssignations = _surveyAssignationQueriesService.GetExpiresWithinTwoDays();
        var schedulers = expiringAssignations.Select( x => x.Scheduler ).ToList();

        var playerIds = GetPlayerIdsFromSchedulers( schedulers );
        await _notificationProviderService
           .SendSurveyNotificationToDevices( playerIds, "reminder_survey_to_compile" );
    }
}
