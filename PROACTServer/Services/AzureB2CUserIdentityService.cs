using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using Microsoft.Graph.Auth;
using Microsoft.Identity.Client;
using Proact.Services.Models;
using Proact.Services.QueriesServices;

namespace Proact.Services.Services {
    public class AzureB2CUserIdentityService : IUserIdentityService {

        private readonly GraphServiceClient _graphClient;
        private readonly AzureB2CSettingsModel _userSettings;
        private ILogger<IdentityUser> _logger;

        public AzureB2CUserIdentityService( 
            IOptions<AzureB2CSettingsModel> userSettings, ILogger<IdentityUser> logger ) {
            _userSettings = userSettings.Value;
            _logger = logger;

            IConfidentialClientApplication confidentialClientApplication = ConfidentialClientApplicationBuilder
                .Create( _userSettings.ClientId )
                .WithTenantId( _userSettings.Tenant )
                .WithClientSecret( _userSettings.ClientSecret )
                .Build();

            ClientCredentialProvider authProvider = new ClientCredentialProvider( confidentialClientApplication );

            GraphServiceClient graphClient = new GraphServiceClient( authProvider );
            _graphClient = graphClient;
        }

        private string ComputeDisplayName( string firstName, string lastName, string email ) {
            if ( !string.IsNullOrEmpty( firstName ) && !string.IsNullOrEmpty( lastName ) ) {
                return firstName + " " + lastName;
            }
            else {
                return "anonymous user";
            }
        }

        public async Task<UserModel> Create( string firstName, string lastName, string email ) {
            try {
                var azureUser = new User {
                    GivenName = !string.IsNullOrEmpty( firstName ) ? firstName : "anonymous",
                    Surname = !string.IsNullOrEmpty( lastName ) ? firstName : "anonymous",
                    DisplayName = ComputeDisplayName( firstName, lastName, email ),
                    AccountEnabled = true,
                    Identities = new List<ObjectIdentity> {
                            new ObjectIdentity() {
                                SignInType = "emailAddress",
                                Issuer = this._userSettings.Tenant,
                                IssuerAssignedId = email
                            }
                        },
                    PasswordProfile = new PasswordProfile() {
                        Password = _userSettings.UserCommonPassword
                    },
                    PasswordPolicies = "DisablePasswordExpiration",
                };

                var graphUser = await _graphClient.Users.Request().AddAsync( azureUser );
                var resultUser = UserGraphToUserModel( graphUser );

                return resultUser;
            }
            catch ( ServiceException ex ) {
                throw new Exception( ex.StatusCode + " " + ex.Message );
            }
            catch ( Exception ex ) {
                throw new Exception( ex.Message );
            }
        }

        public async Task<bool> Delete( string accountId ) {
            try {
                await _graphClient.Users[accountId].Request().DeleteAsync();

                return true;
            }
            catch ( Exception ex ) {
                throw new Exception( ex.Message );
            }
        }

        public async Task<UserModel> GetByAccountId( string accountId ) {
            try {
                var user = await _graphClient.Users[accountId]
                     .Request()
                     .Select( e => new {
                         e.DisplayName,
                         e.Id,
                         e.Identities
                     } )
                     .GetAsync();

                return UserGraphToUserModel( user );
            }
            catch ( Exception ex ) {
                _logger.LogError( ex.Message );
                return null;
            }
        }

        public async Task<string> GetUserEmail( string accountId ) {
            var user = await _graphClient.Users[accountId]
                .Request()
                .Select( e => new {
                    e.DisplayName,
                    e.Id,
                    e.Identities
                } ).GetAsync();

            return user.Identities.ToList()[0].IssuerAssignedId;
        }

        public async Task<UserModel> GetByEmail( string email ) {
            try {
                var searchResult = await _graphClient.Users.Request()
                    .Filter( $"identities/any(c:c/issuerAssignedId eq '{email}' and c/issuer eq '{this._userSettings.Tenant}')" )
                    .GetAsync();

                if ( searchResult.CurrentPage.Count > 0 ) {
                    return UserGraphToUserModel( searchResult.CurrentPage[0] );
                }
                else {
                    return null;
                }
            }
            catch ( Exception ex ) {
                throw new Exception( ex.Message );
            }
        }

        public async Task<bool> Suspend( string accountId ) {
            return await SetAccountEnabled( accountId, false );
        }

        public async Task<bool> Activate( string accountId ) {
            return await SetAccountEnabled( accountId, true );
        }

        private async Task<bool> SetAccountEnabled( string accountId, bool enabled ) {
            try {
                var user = await _graphClient.Users[accountId]
                     .Request()
                     .GetAsync();

                User updateUser = new User() {
                    AccountEnabled = enabled
                };

                await _graphClient.Users[accountId]
                   .Request()
                   .UpdateAsync( updateUser );

                return true;
            }
            catch ( Exception ex ) {
                _logger.LogError( ex.Message );
                return false;
            }
        }

        private UserModel UserGraphToUserModel( User graphUser ) {
            var resultUser = new UserModel() {
                AccountId = graphUser.Id,
                Name = graphUser.DisplayName
            };

            if ( graphUser.AccountEnabled == null || (bool)graphUser.AccountEnabled ) {
                resultUser.State = Entities.UserSubscriptionState.Active;
            }
            else {
                resultUser.State = Entities.UserSubscriptionState.Suspended;
            }

            return resultUser;
        }
    }
}
