using Proact.Services.Entities;
using Proact.Services.QueriesServices;
using Proact.Services.ServicesProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Proact.Services.UnitTests.PushNotifications {
    public class PushNotifications_UnitTests {

        [Fact]
        public void CheckAddRegistrationTest() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var user = mockHelper.CreateDummyUser();

                mockHelper.ServicesProvider
                    .GetEditorsService<IUserNotificationsSettingsEditorService>()
                    .AddDevice( user.Id, Guid.NewGuid() );
                mockHelper.ServicesProvider
                    .GetEditorsService<IUserNotificationsSettingsEditorService>()
                    .AddDevice( user.Id, Guid.NewGuid() );

                mockHelper.ServicesProvider.SaveChanges();

                var deviceIds = mockHelper.ServicesProvider
                    .GetQueriesService<IDeviceQueriesService>().GetPlayerIds( user.Id );

                Assert.True( deviceIds.Count == 2 );
            }
        }

        [Fact]
        public void CheckAddSamePlayerIdMustBeIgnored() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var user = mockHelper.CreateDummyUser();

                var playerId = Guid.NewGuid();

                mockHelper.ServicesProvider
                    .GetEditorsService<IUserNotificationsSettingsEditorService>()
                    .AddDevice( user.Id, playerId );

                mockHelper.ServicesProvider.SaveChanges();

                mockHelper.ServicesProvider
                    .GetEditorsService<IUserNotificationsSettingsEditorService>()
                    .AddDevice( user.Id, playerId );

                mockHelper.ServicesProvider.SaveChanges();

                var deviceIds = mockHelper.ServicesProvider
                    .GetQueriesService<IDeviceQueriesService>().GetPlayerIds( user.Id );

                Assert.Single( deviceIds );
            }
        }

        [Fact]
        public void CheckAddExistingPlayerIdToAnotherUserId() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var user_0 = mockHelper.CreateDummyUser();
                var user_1 = mockHelper.CreateDummyUser();

                var playerId = Guid.NewGuid();

                mockHelper.ServicesProvider
                    .GetEditorsService<IUserNotificationsSettingsEditorService>()
                    .AddDevice( user_0.Id, playerId );

                mockHelper.ServicesProvider.SaveChanges();

                mockHelper.ServicesProvider
                    .GetEditorsService<IUserNotificationsSettingsEditorService>()
                    .AddDevice( user_1.Id, playerId );

                mockHelper.ServicesProvider.SaveChanges();

                var playerIdsUser_0 = mockHelper.ServicesProvider
                    .GetEditorsService<IUserNotificationsSettingsEditorService>()
                    .GetPlayerIds( user_0.Id );

                var playerIdsUser_1 = mockHelper.ServicesProvider
                    .GetEditorsService<IUserNotificationsSettingsEditorService>()
                    .GetPlayerIds( user_1.Id );

                Assert.Empty( playerIdsUser_0 );
                Assert.Single( playerIdsUser_1 );
                Assert.Equal( playerId, playerIdsUser_1[0] );
            }
        }

        [Fact]
        public void CheckRemoveRegistrationTest() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var user = mockHelper.CreateDummyUser();

                var playerId_0 = Guid.NewGuid();
                var playerId_1 = Guid.NewGuid();

                mockHelper.ServicesProvider.
                    GetEditorsService<IUserNotificationsSettingsEditorService>()
                    .AddDevice( user.Id, playerId_0 );
                mockHelper.ServicesProvider.
                    GetEditorsService<IUserNotificationsSettingsEditorService>()
                    .AddDevice( user.Id, playerId_1 );

                mockHelper.ServicesProvider.SaveChanges();

                mockHelper.ServicesProvider
                    .GetEditorsService<IUserNotificationsSettingsEditorService>()
                    .RemoveDevice( user.Id, playerId_0 );

                mockHelper.ServicesProvider.SaveChanges();

                var deviceIds = mockHelper.ServicesProvider
                    .GetEditorsService<IUserNotificationsSettingsEditorService>()
                    .GetPlayerIds( user.Id );

                Assert.Single( deviceIds );
            }
        }

        [Fact]
        public void CheckSetRangeTest() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var user = mockHelper.CreateDummyUser();

                var playerId_0 = Guid.NewGuid();

                mockHelper.ServicesProvider.
                    GetEditorsService<IUserNotificationsSettingsEditorService>()
                    .AddDevice( user.Id, playerId_0 );

                mockHelper.ServicesProvider.SaveChanges();

                var startAt = new TimeSpan( 8, 0, 0 );
                var stopAt = new TimeSpan( 10, 0, 0 );

                mockHelper.ServicesProvider
                    .GetEditorsService<IUserNotificationsSettingsEditorService>()
                    .SetRange( user.Id, startAt, stopAt );

                mockHelper.ServicesProvider.SaveChanges();

                var notificationSettings = mockHelper.ServicesProvider
                    .GetEditorsService<IUserNotificationsSettingsEditorService>()
                    .GetNotificationSettingsByUserId( user.Id );

                Assert.False( notificationSettings.AllDay );
                Assert.True( notificationSettings.Active );
                Assert.Equal( startAt.Hours, notificationSettings.StartAtUtc.Hour );
                Assert.Equal( stopAt.Hours, notificationSettings.StopAtUtc.Hour );
            }
        }

        [Fact]
        public void CheckResetNotificationSettingsTest() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var user = mockHelper.CreateDummyUser();

                var playerId_0 = Guid.NewGuid();
                var playerId_1 = Guid.NewGuid();

                mockHelper.ServicesProvider.
                    GetEditorsService<IUserNotificationsSettingsEditorService>()
                    .AddDevice( user.Id, playerId_0 );
                mockHelper.ServicesProvider.
                    GetEditorsService<IUserNotificationsSettingsEditorService>()
                    .AddDevice( user.Id, playerId_1 );

                mockHelper.ServicesProvider.SaveChanges();

                mockHelper.ServicesProvider
                    .GetEditorsService<IUserNotificationsSettingsEditorService>()
                    .SetRange( user.Id, TimeSpan.MinValue, TimeSpan.MaxValue );

                mockHelper.ServicesProvider.SaveChanges();

                mockHelper.ServicesProvider
                    .GetEditorsService<IUserNotificationsSettingsEditorService>()
                    .ResetConfiguration( user.Id );

                mockHelper.ServicesProvider.SaveChanges();

                var notificationSettings = mockHelper.ServicesProvider
                    .GetEditorsService<IUserNotificationsSettingsEditorService>()
                    .GetNotificationSettingsByUserId( user.Id );

                Assert.True( notificationSettings.AllDay );
                Assert.True( notificationSettings.Active );
            }
        }

        [Fact]
        public void CheckGetPlayersIdsWhereAreActiveNowTest() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var user_0 = mockHelper.CreateDummyUser();
                var user_1 = mockHelper.CreateDummyUser();
                var user_2 = mockHelper.CreateDummyUser();
                var user_3 = mockHelper.CreateDummyUser();

                var users = new List<User>();

                users.Add( user_0 );
                users.Add( user_1 );
                users.Add( user_2 );
                users.Add( user_3 );

                var playerId_0 = Guid.NewGuid();
                var playerId_1 = Guid.NewGuid();
                var playerId_2 = Guid.NewGuid();
                var playerId_3 = Guid.NewGuid();

                mockHelper.ServicesProvider
                    .GetEditorsService<IUserNotificationsSettingsEditorService>()
                    .AddDevice( user_0.Id, playerId_0 );
                mockHelper.ServicesProvider
                    .GetEditorsService<IUserNotificationsSettingsEditorService>()
                    .AddDevice( user_0.Id, playerId_1 );
                mockHelper.ServicesProvider
                    .GetEditorsService<IUserNotificationsSettingsEditorService>()
                    .AddDevice( user_1.Id, playerId_2 );
                mockHelper.ServicesProvider
                    .GetEditorsService<IUserNotificationsSettingsEditorService>()
                    .AddDevice( user_2.Id, playerId_3 );

                mockHelper.ServicesProvider.SaveChanges();

                mockHelper.ServicesProvider
                    .GetEditorsService<IUserNotificationsSettingsEditorService>()
                    .SetActive( user_0.Id, true );
                mockHelper.ServicesProvider
                    .GetEditorsService<IUserNotificationsSettingsEditorService>()
                    .SetActive( user_1.Id, true );
                mockHelper.ServicesProvider
                    .GetEditorsService<IUserNotificationsSettingsEditorService>()
                    .SetActive( user_2.Id, true );

                mockHelper.ServicesProvider.SaveChanges();

                var okTimeSpanStartAt = new TimeSpan(
                    DateTime.UtcNow.Hour, DateTime.UtcNow.Minute, 0 ).Subtract( new TimeSpan( 0, 0, 1 ) );
                var okTimeSpanStopAt = new TimeSpan(
                    DateTime.UtcNow.Hour, DateTime.UtcNow.AddMinutes( 10 ).Minute, 0 );

                var koTimeSpanStartAt = new TimeSpan(
                    DateTime.UtcNow.Hour, DateTime.UtcNow.AddMinutes( 10 ).Minute, 0 );
                var koTimeSpanStopAt = new TimeSpan(
                    DateTime.UtcNow.Hour, DateTime.UtcNow.AddMinutes( 20 ).Minute, 0 );

                mockHelper.ServicesProvider
                    .GetEditorsService<IUserNotificationsSettingsEditorService>()
                    .SetRange( user_0.Id, okTimeSpanStartAt, okTimeSpanStopAt );

                mockHelper.ServicesProvider
                    .GetEditorsService<IUserNotificationsSettingsEditorService>()
                    .SetRange( user_1.Id, koTimeSpanStartAt, koTimeSpanStopAt );

                mockHelper.ServicesProvider.SaveChanges();

                var deviceIds = mockHelper.ServicesProvider
                    .GetEditorsService<IUserNotificationsSettingsEditorService>()
                    .GetPlayersIdsActiveNow( users.Select( x => x.Id ).ToList() );

                Assert.Equal( 3, deviceIds.Count );
                Assert.Contains( playerId_0, deviceIds );
                Assert.Contains( playerId_1, deviceIds );
                Assert.Contains( playerId_3, deviceIds );
            }
        }
    }
}
