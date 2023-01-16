using Microsoft.Azure.Management.Media;
using Microsoft.Azure.Management.Media.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Proact.Services.AzureMediaServices {
    public class AES128ContentKeyPolicyCreatorService : IContentKeyPolicyCreatorService {
        private const string _contentKeyPolicyName = "SharedContentKeyPolicyUsedByAllAssets";

        public async Task<ContentKeyPolicy> CreateContentKeyPolicy( 
            IAzureMediaServicesClient azureMediaServicesClient, 
            string issuerName, string audienceName,
            ContentKeyPolicySymmetricTokenKey primaryKey ) {

            List<ContentKeyPolicyRestrictionTokenKey> alternateKeys = null;
            List<ContentKeyPolicyTokenClaim> requiredClaims = new List<ContentKeyPolicyTokenClaim>() {
                    ContentKeyPolicyTokenClaim.ContentKeyIdentifierClaim
                };

            List<ContentKeyPolicyOption> options = new List<ContentKeyPolicyOption>() {
                    new ContentKeyPolicyOption(
                        new ContentKeyPolicyClearKeyConfiguration(),
                        new ContentKeyPolicyTokenRestriction(
                            issuerName, audienceName, primaryKey,
                            ContentKeyPolicyRestrictionTokenType.Jwt,
                            alternateKeys, requiredClaims ) )
                };

            var policy = await azureMediaServicesClient.ContentKeyPolicies
                .CreateOrUpdateAsync(
                    AzureMediaServicesConfiguration.ResourceGroup,
                    AzureMediaServicesConfiguration.AccountName,
                    _contentKeyPolicyName, options );

            return policy;
        }

        public string GetContentKeyPolicyName() {
            return _contentKeyPolicyName;
        }

        public string GetStreamingPolicyName() {
            return PredefinedStreamingPolicy.ClearKey;
        }
    }
}
