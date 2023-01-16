using Proact.Services.Models;
using Proact.Services.QueriesServices;
using System;
using System.Collections.Generic;
using Xunit;

namespace Proact.Services.UnitTests.SurveyScheduler {
    public class SurveySchedulerUnitTests {
        [Fact]
        public void CreateSchedulerConsistencyCheck() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var user = mockHelper.CreateDummyUser();
                var patient = mockHelper.CreateDummyPatient( user );
                var survey = SurveyCreatorHelper.CreateDummySurvey( mockHelper );

                var assignationRequest = new AssignSurveyToPatientsRequest() {
                    StartTime = SurveyAssignationDataUtils.GetStartTimeFormat( DateTime.UtcNow.AddMinutes( 1 ) ),
                    ExpireTime = SurveyAssignationDataUtils.GetExpireTimeFormat( DateTime.UtcNow.AddYears( 1 ) ),
                    Reccurence = Entities.SurveyReccurence.Weekly,
                    SurveyId = survey.Id,
                    UserIds = new List<Guid>() { patient.UserId }
                };

                mockHelper.ServicesProvider
                    .GetEditorsService<ISurveySchedulerEditorService>()
                    .CreateSchedulers( assignationRequest );

                mockHelper.ServicesProvider.SaveChanges();

                var schedulerRetrived = mockHelper.ServicesProvider
                    .GetEditorsService<ISurveySchedulerEditorService>()
                    .GetSchedulerByUserId( user.Id );

                Assert.NotNull( schedulerRetrived );
                Assert.True( schedulerRetrived.Count == 1 );
                Assert.Equal( assignationRequest.Reccurence, schedulerRetrived[0].Reccurence );
                Assert.Equal( assignationRequest.StartTime.Date, schedulerRetrived[0].StartTime.Date );
                Assert.Equal( assignationRequest.ExpireTime.Date, schedulerRetrived[0].ExpireTime.Date );
                Assert.Equal( assignationRequest.SurveyId, schedulerRetrived[0].SurveyId );
                Assert.Equal( assignationRequest.UserIds[0], schedulerRetrived[0].UserId );
                Assert.Equal( 7, schedulerRetrived[0].DaysInterval );
            }
        }

        [Fact]
        public void GetTodayPendingSurveysSchedulersCheckConsistency() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var user_0 = mockHelper.CreateDummyUser();
                var user_1 = mockHelper.CreateDummyUser();

                var survey_0 = SurveyCreatorHelper.CreateDummySurvey( mockHelper );
                var survey_1 = SurveyCreatorHelper.CreateDummySurvey( mockHelper );

                var assignationRequestMustStartNow = new AssignSurveyToPatientsRequest() {
                    StartTime = SurveyAssignationDataUtils.GetStartTimeFormat( DateTime.UtcNow ),
                    ExpireTime = SurveyAssignationDataUtils.GetExpireTimeFormat( DateTime.UtcNow.AddYears( 1 ) ),
                    Reccurence = Entities.SurveyReccurence.Weekly,
                    SurveyId = survey_0.Id,
                    UserIds = new List<Guid>() { user_0.Id, user_1.Id }
                };

                mockHelper.ServicesProvider
                    .GetEditorsService<ISurveySchedulerEditorService>()
                    .CreateSchedulers( assignationRequestMustStartNow );

                mockHelper.ServicesProvider.SaveChanges();

                var assignationRequestMustStartTomorrow = new AssignSurveyToPatientsRequest() {
                    StartTime = SurveyAssignationDataUtils.GetStartTimeFormat( DateTime.UtcNow.AddDays( 1 ) ),
                    ExpireTime = SurveyAssignationDataUtils.GetExpireTimeFormat( DateTime.UtcNow.AddYears( 1 ) ),
                    Reccurence = Entities.SurveyReccurence.Weekly,
                    SurveyId = survey_1.Id,
                    UserIds = new List<Guid>() { user_0.Id, user_1.Id }
                };

                mockHelper.ServicesProvider
                    .GetEditorsService<ISurveySchedulerEditorService>()
                    .CreateSchedulers( assignationRequestMustStartTomorrow );

                mockHelper.ServicesProvider.SaveChanges();

                var schedulersPerUsers = mockHelper.ServicesProvider
                    .GetQueriesService<ISurveySchedulerQueriesService>()
                    .GetTodayPendingSurveysSchedulers();

                Assert.Equal( 2, schedulersPerUsers.Count );
            }
        }

        [Fact]
        public void ProcessPendingSurveysNotificationsCheckConsistency() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var user_0 = mockHelper.CreateDummyUser();
                var user_1 = mockHelper.CreateDummyUser();

                var device_0_user_0 = mockHelper.AddDummyDeviceToUser( user_0.Id );
                var device_1_user_0 = mockHelper.AddDummyDeviceToUser( user_0.Id );
                var device_0_user_1 = mockHelper.AddDummyDeviceToUser( user_1.Id );

                var survey_0 = SurveyCreatorHelper.CreateDummySurvey( mockHelper );
                var survey_1 = SurveyCreatorHelper.CreateDummySurvey( mockHelper );

                var assignationRequestMustStartNow = new AssignSurveyToPatientsRequest() {
                    StartTime = SurveyAssignationDataUtils.GetStartTimeFormat( DateTime.UtcNow ),
                    ExpireTime = SurveyAssignationDataUtils.GetExpireTimeFormat( DateTime.UtcNow.AddYears( 1 ) ),
                    Reccurence = Entities.SurveyReccurence.Weekly,
                    SurveyId = survey_0.Id,
                    UserIds = new List<Guid>() { user_0.Id, user_1.Id }
                };

                var todayCreatedSchedulers = mockHelper.ServicesProvider
                    .GetEditorsService<ISurveySchedulerEditorService>()
                    .CreateSchedulers( assignationRequestMustStartNow );

                mockHelper.ServicesProvider.SaveChanges();

                var assignationRequestMustStartTomorrow = new AssignSurveyToPatientsRequest() {
                    StartTime = SurveyAssignationDataUtils.GetStartTimeFormat( DateTime.UtcNow.AddDays( 1 ) ),
                    ExpireTime = SurveyAssignationDataUtils.GetExpireTimeFormat( DateTime.UtcNow.AddYears( 1 ) ),
                    Reccurence = Entities.SurveyReccurence.Weekly,
                    SurveyId = survey_1.Id,
                    UserIds = new List<Guid>() { user_0.Id, user_1.Id }
                };

                mockHelper.ServicesProvider
                    .GetEditorsService<ISurveySchedulerEditorService>()
                    .CreateSchedulers( assignationRequestMustStartTomorrow );

                mockHelper.ServicesProvider.SaveChanges();

                var schedulers = mockHelper.ServicesProvider
                    .GetQueriesService<ISurveySchedulerQueriesService>()
                    .GetTodayPendingSurveysSchedulers();

                mockHelper.ServicesProvider.SaveChanges();

                var notificationResultsFirtTime = mockHelper.ServicesProvider
                    .GetEditorsService<ISurveySchedulerEditorService>()
                    .ProcessPendingSurveysNotifications()
                    .Result;

                mockHelper.ServicesProvider.SaveChanges();

                var createdSchedulerSent = mockHelper.ServicesProvider
                    .GetEditorsService<ISurveySchedulerEditorService>()
                    .GetScheduler( todayCreatedSchedulers[0].Id );

                Assert.Equal(
                    assignationRequestMustStartNow.ExpireTime, createdSchedulerSent.ExpireTime );
                Assert.Equal(
                    assignationRequestMustStartNow.StartTime, createdSchedulerSent.StartTime );
                Assert.Equal( 2, schedulers.Count );
                Assert.Equal( 2, notificationResultsFirtTime.SurveySchedulersExecuted.Count );
                Assert.Equal( 3, notificationResultsFirtTime.PlayerIds.Count );
                Assert.Contains<Guid>( device_0_user_0.PlayerId, notificationResultsFirtTime.PlayerIds );
                Assert.Contains<Guid>( device_1_user_0.PlayerId, notificationResultsFirtTime.PlayerIds );
                Assert.Contains<Guid>( device_0_user_1.PlayerId, notificationResultsFirtTime.PlayerIds );

                var notificationResultsSecondTime = mockHelper.ServicesProvider
                    .GetEditorsService<ISurveySchedulerEditorService>()
                    .ProcessPendingSurveysNotifications()
                    .Result;

                Assert.Empty( notificationResultsSecondTime.SurveySchedulersExecuted );
            }
        }

        [Fact]
        public void ProcessSurveyShedulerImmediateForUsersCheckConsistency() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var user_0 = mockHelper.CreateDummyUser();
                var user_1 = mockHelper.CreateDummyUser();

                var device_0_user_0 = mockHelper.AddDummyDeviceToUser( user_0.Id );
                var device_1_user_0 = mockHelper.AddDummyDeviceToUser( user_0.Id );
                var device_0_user_1 = mockHelper.AddDummyDeviceToUser( user_1.Id );

                var survey_0 = SurveyCreatorHelper.CreateDummySurvey( mockHelper );
                var survey_1 = SurveyCreatorHelper.CreateDummySurvey( mockHelper );

                var assignationRequestMustStartNow = new AssignSurveyToPatientsRequest() {
                    StartTime = SurveyAssignationDataUtils.GetStartTimeFormat( DateTime.UtcNow ),
                    ExpireTime = SurveyAssignationDataUtils.GetExpireTimeFormat( DateTime.UtcNow.AddYears( 1 ) ),
                    Reccurence = Entities.SurveyReccurence.Once,
                    SurveyId = survey_0.Id,
                    UserIds = new List<Guid>() { user_0.Id, user_1.Id }
                };

                var createdSchedulers = mockHelper.ServicesProvider
                    .GetEditorsService<ISurveySchedulerEditorService>()
                    .CreateSchedulers( assignationRequestMustStartNow );

                mockHelper.ServicesProvider.SaveChanges();

                var notificationSendingResume = mockHelper.ServicesProvider
                    .GetEditorsService<ISurveySchedulerEditorService>()
                    .ProcessSurveyShedulerImmediateForUsers( assignationRequestMustStartNow.UserIds )
                    .Result;

                var createdSchedulerSent = mockHelper.ServicesProvider
                    .GetEditorsService<ISurveySchedulerEditorService>()
                    .GetScheduler( createdSchedulers[0].Id );

                Assert.Equal( 
                    assignationRequestMustStartNow.ExpireTime, createdSchedulerSent.ExpireTime );
                Assert.Equal(
                    assignationRequestMustStartNow.StartTime, createdSchedulerSent.StartTime );
                Assert.Equal( 2, notificationSendingResume.SurveySchedulersExecuted.Count );
                Assert.Equal( 3, notificationSendingResume.PlayerIds.Count );
                Assert.Contains<Guid>( device_0_user_0.PlayerId, notificationSendingResume.PlayerIds );
                Assert.Contains<Guid>( device_1_user_0.PlayerId, notificationSendingResume.PlayerIds );
                Assert.Contains<Guid>( device_0_user_1.PlayerId, notificationSendingResume.PlayerIds );

                var notificationResultsSecondTime = mockHelper.ServicesProvider
                    .GetEditorsService<ISurveySchedulerEditorService>()
                    .ProcessPendingSurveysNotifications()
                    .Result;

                Assert.Empty( notificationResultsSecondTime.SurveySchedulersExecuted );
            }
        }
    }
}
