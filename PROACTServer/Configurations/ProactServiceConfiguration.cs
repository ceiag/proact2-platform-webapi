namespace Proact.Services.Configurations {
    public class ProactServiceConfiguration {
        private static readonly string _baseAgentEndpoint = "https://proactencryptionagent.azurewebsites.net";
        private static readonly string _azureMediaServiceSettingsFileName = "AzureMediaServiceSettings.json";
        private static readonly string _avatarSettingsFileName = "AvatarConfigurationSettings.json";
        private static readonly string _ffmpegSettingsFile = "MediaFilesUploaderSettings.json";
        private static readonly string _oneSignalSettingsFileName = "OneSignalConfiguration.json";

        public static string EncryptionAgentContentEndpoint {
            get { return GetAgentEndpoint( "/api/content/" ); }
        }

        public static string EncryptionAgentMessageDataEndpoint {
            get { return GetAgentEndpoint( "/api/MessageData/" ); }
        }

        public static string EncryptionAgentPublicKeyEndpoint {
            get { return GetAgentEndpoint( "/api/PublicKeyForMsgEncryption" ); }
        }

        public static string AzureMediaServiceSettingsFileName {
           get { return _azureMediaServiceSettingsFileName; }
        }

        public static string AvatarSettingsFileName {
            get { return _avatarSettingsFileName; }
        }

        public static string MediaFilesUploaderSettingsFileName {
            get { return _ffmpegSettingsFile; }
        }

        public static string OneSignalConfigurationFileName {
            get { return _oneSignalSettingsFileName; }
        }

        private static string GetAgentEndpoint( string action ) {
            return _baseAgentEndpoint + action;
        }
    }
}
