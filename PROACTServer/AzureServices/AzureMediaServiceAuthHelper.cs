using Microsoft.Azure.Management.Media;
using Microsoft.Identity.Client;
using Microsoft.Rest;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Proact.Services.AzureMediaServices {
    public class AzureMediaServiceAuthHelper {
        public static readonly string _tokenType = "Bearer";

        public static async Task<IAzureMediaServicesClient> CreateMediaServicesClientAsync(
            bool interactive = false ) {
            ServiceClientCredentials credentials;

            if ( interactive ) {
                credentials = await GetCredentialsInteractiveAuthAsync();
            }
            else {
                credentials = await GetCredentialsAsync();
            }

            return new AzureMediaServicesClient( 
                AzureMediaServicesConfiguration.ArmEndpoint, credentials ) {
                SubscriptionId = AzureMediaServicesConfiguration.SubscriptionId,
            };
        }
        
        private static async Task<ServiceClientCredentials> GetCredentialsAsync() {
            var scopes = new[] {
                AzureMediaServicesConfiguration.ArmAadAudience + "/.default" 
            };

            var app = ConfidentialClientApplicationBuilder.Create( 
                AzureMediaServicesConfiguration.AadClientId )
                .WithClientSecret( AzureMediaServicesConfiguration.AadSecret )
                .WithAuthority( 
                    AzureCloudInstance.AzurePublic, 
                    AzureMediaServicesConfiguration.AadTenantId )
                .Build();

            var authResult = await app
                .AcquireTokenForClient( scopes )
                .ExecuteAsync()
                .ConfigureAwait( false );

            return new TokenCredentials( authResult.AccessToken, _tokenType );
        }
       
        private static async Task<ServiceClientCredentials> GetCredentialsInteractiveAuthAsync() {
            var scopes = new[] {
                AzureMediaServicesConfiguration.ArmAadAudience + "/user_impersonation" 
            };

            string ClientApplicationId = "04b07795-8ddb-461a-bbee-02f9e1bf7b46";

            AuthenticationResult result = null;

            IPublicClientApplication app = PublicClientApplicationBuilder.Create( ClientApplicationId )
                .WithAuthority( 
                    AzureCloudInstance.AzurePublic, 
                    AzureMediaServicesConfiguration.AadTenantId )
                .WithRedirectUri( "http://localhost" )
                .Build();

            var accounts = await app.GetAccountsAsync();

            try {
                result = await app.AcquireTokenSilent( scopes, accounts.FirstOrDefault() ).ExecuteAsync();
            }
            catch ( MsalUiRequiredException ex ) {
                try {
                    result = await app.AcquireTokenInteractive( scopes ).ExecuteAsync();
                }
                catch ( MsalException maslException ) {
                    Console.Error.WriteLine( $"ERROR: MSAL interactive authentication exception with code '{maslException.ErrorCode}' and message '{maslException.Message}'." );
                }
            }
            catch ( MsalException maslException ) {
                Console.Error.WriteLine( $"ERROR: MSAL silent authentication exception with code '{maslException.ErrorCode}' and message '{maslException.Message}'." );
            }

            return new TokenCredentials( result.AccessToken, _tokenType );
        }
    }
}
