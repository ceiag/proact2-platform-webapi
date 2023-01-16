using Proact.Services.Entities;
using Proact.Services.QueriesServices;
using Proact.Services.Tests.Shared;
using System;
using System.Collections.Generic;
using Xunit;

namespace Proact.Services.UnitTests.Notifications.NotificationSettingsQueries;
public class GetByUserIds {
    [Fact]
    public void _MustReturnFourUserIds() {
        var servicesProvider = new ProactServicesProvider();

        var user_0 = servicesProvider.Database.Users.Add( new User() {
            Id = Guid.NewGuid(),
        } ).Entity;
        var user_1 = servicesProvider.Database.Users.Add( new User() {
            Id = Guid.NewGuid(),
        } ).Entity;
        var user_2 = servicesProvider.Database.Users.Add( new User() {
            Id = Guid.NewGuid(),
        } ).Entity;

        var device_0 = servicesProvider.Database.Devices.Add( new Device() {
            Id = Guid.NewGuid(),
            PlayerId = Guid.NewGuid()
        } ).Entity;
        var device_1 = servicesProvider.Database.Devices.Add( new Device() {
            Id = Guid.NewGuid(),
            PlayerId = Guid.NewGuid()
        } ).Entity;
        var device_2 = servicesProvider.Database.Devices.Add( new Device() {
            Id = Guid.NewGuid(),
            PlayerId = Guid.NewGuid()
        } ).Entity;
        var device_3 = servicesProvider.Database.Devices.Add( new Device() {
            Id = Guid.NewGuid(),
            PlayerId = Guid.NewGuid()
        } ).Entity;

        var notSet_0 = servicesProvider.Database.NotificationSettings.Add( new NotificationSettings() {
            Active = true,
            Id = Guid.NewGuid(),
            User = user_0,
            Devices = new List<Device>() { device_0, device_1 }
        } ).Entity;
        var notSet_1 = servicesProvider.Database.NotificationSettings.Add( new NotificationSettings() {
            Active = true,
            Id = Guid.NewGuid(),
            User = user_1,
            Devices = new List<Device>() { device_2 }
        } ).Entity;
        var notSet_2 = servicesProvider.Database.NotificationSettings.Add( new NotificationSettings() {
            Active = true,
            Id = Guid.NewGuid(),
            User = user_2,
            Devices = new List<Device>() { device_3 }
        } ).Entity;

        servicesProvider.Database.SaveChanges();

        var devices = servicesProvider
            .GetQueriesService<IUserNotificationSettingsQueriesService>()
            .GetDevicesOfUsers( new List<Guid> { user_0.Id, user_1.Id, user_2.Id } );

        Assert.Equal( 4, devices.Count );
        Assert.Equal( device_0.PlayerId, devices[0].PlayerId );
        Assert.Equal( device_1.PlayerId, devices[1].PlayerId );
        Assert.Equal( device_2.PlayerId, devices[2].PlayerId );
        Assert.Equal( device_3.PlayerId, devices[3].PlayerId );
    }
}
