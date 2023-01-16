using Microsoft.Azure.Management.Media;
using Microsoft.Azure.Management.Media.Models;
using System.Threading.Tasks;

namespace Proact.Services.AzureMediaServices {
    public interface IContentKeyPolicyCreatorService {
        public Task<ContentKeyPolicy> CreateContentKeyPolicy(
            IAzureMediaServicesClient azureMediaServicesClient,
            string issuerName, string audienceName,
            ContentKeyPolicySymmetricTokenKey primaryKey );
        public string GetContentKeyPolicyName();
        public string GetStreamingPolicyName();
    }
}
