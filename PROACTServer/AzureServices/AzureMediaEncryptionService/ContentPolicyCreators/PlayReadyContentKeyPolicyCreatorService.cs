using Microsoft.Azure.Management.Media;
using Microsoft.Azure.Management.Media.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Proact.Services.AzureMediaServices {
    public class PlayReadyContentKeyPolicyCreatorService : IContentKeyPolicyCreatorService {
        private const string _contentKeyPolicyName = "DRMContentKeyPolicy";
        private const string _streamingKeyPolicyName = "Predefined_MultiDrmCencStreaming";

        private ContentKeyPolicyPlayReadyConfiguration ConfigurePlayReadyLicenseTemplate() {
            var objContentKeyPolicyPlayReadyLicense = new ContentKeyPolicyPlayReadyLicense {
                AllowTestDevices = true,
                BeginDate = new DateTime( 2016, 1, 1 ),
                ContentKeyLocation = new ContentKeyPolicyPlayReadyContentEncryptionKeyFromHeader(),
                ContentType = ContentKeyPolicyPlayReadyContentType.UltraVioletStreaming,
                LicenseType = ContentKeyPolicyPlayReadyLicenseType.Persistent,
                PlayRight = new ContentKeyPolicyPlayReadyPlayRight {
                    ImageConstraintForAnalogComponentVideoRestriction = true,
                    ExplicitAnalogTelevisionOutputRestriction
                        = new ContentKeyPolicyPlayReadyExplicitAnalogTelevisionRestriction( true, 2 ),
                    AllowPassingVideoContentToUnknownOutput
                        = ContentKeyPolicyPlayReadyUnknownOutputPassingOption.Allowed
                }
            };

            var objContentKeyPolicyPlayReadyConfiguration = new ContentKeyPolicyPlayReadyConfiguration {
                Licenses = new List<ContentKeyPolicyPlayReadyLicense> {
                    objContentKeyPolicyPlayReadyLicense
                }
            };

            return objContentKeyPolicyPlayReadyConfiguration;
        }

        public async Task<ContentKeyPolicy> CreateContentKeyPolicy( 
            IAzureMediaServicesClient azureMediaServicesClient, 
            string issuerName, string audienceName, ContentKeyPolicySymmetricTokenKey primaryKey ) {
            List<ContentKeyPolicyTokenClaim> requiredClaims
                = new List<ContentKeyPolicyTokenClaim>() {
                        ContentKeyPolicyTokenClaim.ContentKeyIdentifierClaim
                };

            List<ContentKeyPolicyRestrictionTokenKey> alternateKeys = null;
            ContentKeyPolicyTokenRestriction restriction
                = new ContentKeyPolicyTokenRestriction(
                    issuerName, audienceName, primaryKey,
                    ContentKeyPolicyRestrictionTokenType.Jwt,
                    alternateKeys, requiredClaims );

            ContentKeyPolicyPlayReadyConfiguration playReadyConfig
                = ConfigurePlayReadyLicenseTemplate();

            List<ContentKeyPolicyOption> options = new List<ContentKeyPolicyOption>();

            options.Add(
                new ContentKeyPolicyOption() {
                    Configuration = playReadyConfig,
                    Restriction = restriction
                } );

            var policy = await azureMediaServicesClient.ContentKeyPolicies.CreateOrUpdateAsync(
                AzureMediaServicesConfiguration.ResourceGroup,
                AzureMediaServicesConfiguration.AccountName,
                _contentKeyPolicyName, options );

            return policy;
        }

        public string GetContentKeyPolicyName() {
            return _contentKeyPolicyName;
        }

        public string GetStreamingPolicyName() {
            return _streamingKeyPolicyName;
        }
    }
}
