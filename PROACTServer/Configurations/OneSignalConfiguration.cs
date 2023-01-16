using Microsoft.Extensions.Configuration;

namespace Proact.Services {
    public static class OneSignalConfiguration {
        private static IConfiguration _config;

        public static void Init( IConfiguration config ) {
            _config = config;
        }

        public static string AppId {
            get { return _config["OneSignal:ApiId"]; }
        }

        public static string AppKey {
            get { return _config["OneSignal:ApiKey"]; }
        }
    }
}
