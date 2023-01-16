using Microsoft.Extensions.Configuration;

namespace Proact.Services.Configurations {
    public static class EmailSenderConfiguration {
        private static IConfiguration _config;

        public static void Init( IConfiguration config ) {
            _config = config;
        }

        public static string RequestUrl {
            get {
                return _config["EmailSender:RequestUrl"];
            }
        }
    }
}
