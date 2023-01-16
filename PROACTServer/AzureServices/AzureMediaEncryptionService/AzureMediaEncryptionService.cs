using Microsoft.Azure.Management.Media;
using Microsoft.Azure.Management.Media.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Proact.Services.AzureMediaServices {
    public class AzureMediaEncryptionService : IAzureMediaEncryptionService {
        private byte[] _tokenSigningKey = new byte[40];
        private const string _issuerName = "ProactIssuer";
        private const string _audienceName = "AudienceName";
        private const int _jwtTokenDurationInMinutes = 60;

        private IContentKeyPolicyCreatorService _contentKeyPolicyCreatorService;

        public AzureMediaEncryptionService( 
            IContentKeyPolicyCreatorService contentKeyPolicyCreatorService ) {
            _contentKeyPolicyCreatorService = contentKeyPolicyCreatorService;
        }

        private async Task<ContentKeyPolicy> GetContentKeyPolicyIfExist( 
            IAzureMediaServicesClient azureMediaServicesClient, string contentKeyPolicyName ) {
            ContentKeyPolicy existPolicy = await azureMediaServicesClient.ContentKeyPolicies.GetAsync(
                AzureMediaServicesConfiguration.ResourceGroup,
                AzureMediaServicesConfiguration.AccountName,
                contentKeyPolicyName );

            return existPolicy;
        }

        public async Task<ContentKeyPolicyProperties> GetContentKeyPolicyPropertiesAsync(
            IAzureMediaServicesClient azureMediaServicesClient ) {
            var policyProperties = await azureMediaServicesClient.ContentKeyPolicies
                    .GetPolicyPropertiesWithSecretsAsync(
                        AzureMediaServicesConfiguration.ResourceGroup,
                        AzureMediaServicesConfiguration.AccountName,
                        _contentKeyPolicyCreatorService.GetContentKeyPolicyName() );

            return policyProperties;
        }

        public async Task<ContentKeyPolicy> GetOrCreateContentKeyPolicyAsync( 
            IAzureMediaServicesClient azureMediaServicesClient ) {

            var policy = await GetContentKeyPolicyIfExist( 
                azureMediaServicesClient, _contentKeyPolicyCreatorService.GetContentKeyPolicyName() );

            if ( policy == null ) {
                ContentKeyPolicySymmetricTokenKey primaryKey
                    = new ContentKeyPolicySymmetricTokenKey( _tokenSigningKey );

                policy = await _contentKeyPolicyCreatorService
                    .CreateContentKeyPolicy( 
                        azureMediaServicesClient, _issuerName, _audienceName, primaryKey );
            }
            else {
                var policyProperties = await GetContentKeyPolicyPropertiesAsync( azureMediaServicesClient );

                if ( policyProperties.Options[0].Restriction is ContentKeyPolicyTokenRestriction restriction ) {
                    if ( restriction.PrimaryVerificationKey is ContentKeyPolicySymmetricTokenKey signingKey ) {
                        _tokenSigningKey = signingKey.KeyValue;
                    }
                }
            }

            return policy;
        }

        public async Task<StreamingLocator> CreateStreamingLocatorAsync(
            IAzureMediaServicesClient azureMediaServicesClient, string assetName, string locatorName ) {

            //var streamingLocator = await azureMediaServicesClient
            //    .StreamingLocators.CreateAsync( 
            //        AzureMediaServicesConfiguration.ResourceGroup,
            //        AzureMediaServicesConfiguration.AccountName, 
            //        locatorName,
            //        new StreamingLocator {
            //            AssetName = assetName,
            //            StreamingPolicyName 
            //                = _contentKeyPolicyCreatorService.GetStreamingPolicyName(),
            //            DefaultContentKeyPolicyName 
            //                = _contentKeyPolicyCreatorService.GetContentKeyPolicyName()
            //        } );

            //return streamingLocator;

            StreamingLocator locator = await azureMediaServicesClient.StreamingLocators.CreateAsync(
                AzureMediaServicesConfiguration.ResourceGroup,
                AzureMediaServicesConfiguration.AccountName,
                locatorName,
                new StreamingLocator {
                    AssetName = assetName,
                    StreamingPolicyName = PredefinedStreamingPolicy.ClearStreamingOnly
                } );

            return locator;
        }

        private async Task<StreamingEndpoint> GetStreamingEndpointAsync(
            IAzureMediaServicesClient azureMediaServicesClient ) {
            const string DefaultStreamingEndpointName = "default";

            var streamingEndpoint = await azureMediaServicesClient.StreamingEndpoints.GetAsync(
                    AzureMediaServicesConfiguration.ResourceGroup,
                    AzureMediaServicesConfiguration.AccountName,
                    DefaultStreamingEndpointName );

            if ( streamingEndpoint != null ) {
                if ( streamingEndpoint.ResourceState != StreamingEndpointResourceState.Running ) {
                    await azureMediaServicesClient.StreamingEndpoints.StartAsync(
                        AzureMediaServicesConfiguration.ResourceGroup,
                        AzureMediaServicesConfiguration.AccountName,
                        DefaultStreamingEndpointName );
                }
            }

            return streamingEndpoint;
        }

        public async Task<string> GetDASHStreamingUrlAsync(
            IAzureMediaServicesClient azureMediaServicesClient, string locatorName ) {
            string dashPath = "";
            var streamingEndpoint = await GetStreamingEndpointAsync( azureMediaServicesClient );

            ListPathsResponse paths = await azureMediaServicesClient.StreamingLocators
                .ListPathsAsync(
                    AzureMediaServicesConfiguration.ResourceGroup,
                    AzureMediaServicesConfiguration.AccountName,
                    locatorName );

            foreach ( StreamingPath path in paths.StreamingPaths ) {
                UriBuilder uriBuilder = new UriBuilder {
                    Scheme = "https",
                    Host = streamingEndpoint.HostName
                };

                if ( path.StreamingProtocol == StreamingPolicyStreamingProtocol.Dash ) {
                    uriBuilder.Path = path.Paths[0];
                    dashPath = uriBuilder.ToString();
                }
            }

            //todo: fix this!
            dashPath = dashPath.Replace( "format=mpd-time-csf", "format=m3u8-aapl" );

            return dashPath;
        }

        public string GetToken( string keyIdentifier, byte[] tokenVerificationKey ) {
            var tokenSigningKey = new SymmetricSecurityKey( tokenVerificationKey );

            SigningCredentials cred = new SigningCredentials(
                tokenSigningKey,
                SecurityAlgorithms.HmacSha256,
                SecurityAlgorithms.Sha256Digest );

            Claim[] claims = new Claim[] {
                new Claim( ContentKeyPolicyTokenClaim.ContentKeyIdentifierClaim.ClaimType, keyIdentifier )
            };

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: _issuerName,
                audience: _audienceName,
                claims: claims,
                notBefore: DateTime.Now.AddMinutes( -5 ),
                expires: DateTime.Now.AddMinutes( _jwtTokenDurationInMinutes ),
                signingCredentials: cred );

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

            return handler.WriteToken( token );
        }

        public async Task<string> GetTokenAsync( 
            IAzureMediaServicesClient azureMediaServicesClient, string locatorName ) {
            var streamingLocator = azureMediaServicesClient.StreamingLocators.Get(
                AzureMediaServicesConfiguration.ResourceGroup,
                AzureMediaServicesConfiguration.AccountName,
                locatorName );

            string keyIdentifier = streamingLocator.ContentKeys.First().Id.ToString();

            var policyProperties = await GetContentKeyPolicyPropertiesAsync( azureMediaServicesClient );

            if ( policyProperties.Options[0].Restriction is ContentKeyPolicyTokenRestriction restriction ) {
                if ( restriction.PrimaryVerificationKey is ContentKeyPolicySymmetricTokenKey signingKey ) {
                    _tokenSigningKey = signingKey.KeyValue;
                }
            }

            return GetToken( keyIdentifier, _tokenSigningKey );
        }
    }
}
