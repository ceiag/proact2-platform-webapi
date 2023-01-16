using System;

namespace Proact.Services.Models {
    public class SetNotificationRangeRequest {
        public DateTime StartAtUtc { get; set; }
        public DateTime StopAtUtc { get; set; }
    }
}
