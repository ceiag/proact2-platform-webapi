using Proact.Services.Entities;
using Proact.Services.Models;

namespace Proact.Services.EntitiesMapper {
    public static class MobileAppsInfoEntityMapper {
        public static MobileAppsInfoModel Map( MobileAppsInfo mobileAppsInfo ) {
            return new MobileAppsInfoModel() {
                AndroidLastBuildRequired = mobileAppsInfo.AndroidLastBuildRequired,
                AndroidStoreUrl = mobileAppsInfo.AndroidStoreUrl,
                iOSLastBuildRequired = mobileAppsInfo.iOSLastBuildRequired,
                iOSStoreUrl = mobileAppsInfo.iOSStoreUrl
            };
        }
    }
}
