using Proact.Services.Models;

namespace Proact.Services.QueriesServices {
    public interface IMobileAppsInfoQueriesService {
        public MobileAppsInfoModel Get();
        public void Set( MobileAppsInfoCreationRequest request );
    }
}
