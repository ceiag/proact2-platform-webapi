using System;

namespace Proact.Services.Utils {
    public static class TimeCalculatorUtils {
        public static double GetMinutesPassedSinceInUtc( DateTime time ) {
            return ( DateTime.UtcNow - time.ToUniversalTime() ).TotalMinutes;
        }
    }
}
