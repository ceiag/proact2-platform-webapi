using Microsoft.Extensions.Configuration;

namespace Proact.Services {
    public static class AvatarConfiguration {
        private static IConfiguration _config;

        public static void Init( IConfiguration config ) {
            _config = config;
        }

        public static string MedicAvatarDefaultUrl {
            get { return _config["DefaultAvatars:MedicAvatarDefaultUrl"]; }
        }

        public static string NurseAvatarDefaultUrl {
            get { return _config["DefaultAvatars:NurseAvatarDefaultUrl"]; }
        }

        public static string ResearcherAvatarDefaultUrl {
            get { return _config["DefaultAvatars:ResearcherAvatarDefaultUrl"]; }
        }

        public static string PatientAvatarDefaultUrl {
            get { return _config["DefaultAvatars:PatientAvatarDefaultUrl"]; }
        }
    }
}
