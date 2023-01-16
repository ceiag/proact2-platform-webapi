using System;

namespace Proact.Services.Models {
    public class NotificationSettingsModel {
        public bool Active { get; set; }
        public bool AllDay { get; set; }
        public DateTime StartAtUtc { get; set; }
        public DateTime StopAtUtc { get; set; }
    }
}
