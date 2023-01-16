using Microsoft.Azure.Management.Media;
using Microsoft.Azure.Management.Media.Models;
using System.Threading.Tasks;

namespace Proact.Services.AzureMediaServices {
    public interface IAzureMediaEncryptionService {
        public Task<ContentKeyPolicy> GetOrCreateContentKeyPolicyAsync(
            IAzureMediaServicesClient azureMediaServicesClient );
        public Task<string> GetDASHStreamingUrlAsync(
            IAzureMediaServicesClient azureMediaServicesClient, string locatorName );
        public Task<StreamingLocator> CreateStreamingLocatorAsync(
            IAzureMediaServicesClient azureMediaServicesClient, string assetName, string locatorName );
        public Task<string> GetTokenAsync(
            IAzureMediaServicesClient azureMediaServicesClient, string locatorName );
    }
}
