using Proact.Services.Entities;
using Proact.Services.Models;
using System;

namespace Proact.Services.EntitiesMapper {
    public static class NotificationSettingsMapper {
        public static NotificationSettingsModel Map( NotificationSettings notificationSettings ) {
            if ( notificationSettings == null ) {
                return new NotificationSettingsModel();
            }

            return new NotificationSettingsModel() {
                Active = notificationSettings.Active,
                AllDay = notificationSettings.AllDay,
                StartAtUtc = new DateTime( notificationSettings.StartAt.Ticks ),
                StopAtUtc = new DateTime( notificationSettings.StopAt.Ticks ),
            };
        }
    }
}
