using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using Microsoft.Graph.Auth;
using Microsoft.Identity.Client;
using Proact.Services.Models;

namespace Proact.Services.Services {
    public class GroupService : IGroupService {

        private readonly GraphServiceClient _graphClient;
        private readonly AzureB2CSettingsModel _userSettings;
        private ILogger<IdentityUser> _logger;

        private readonly string _filterFormat
            = "startsWith(displayName,'{0}')";
        private readonly string _groupDescriptionFormat
            = "Proact {0} security role";

        public GroupService( IOptions<AzureB2CSettingsModel> userSettings,
                             ILogger<IdentityUser> logger ) {

            _userSettings = userSettings.Value;
            _logger = logger;

            IConfidentialClientApplication confidentialClientApplication
                = ConfidentialClientApplicationBuilder
                .Create( _userSettings.ClientId )
                .WithTenantId( _userSettings.Tenant )
                .WithClientSecret( _userSettings.ClientSecret )
                .Build();

            ClientCredentialProvider authProvider
                = new ClientCredentialProvider( confidentialClientApplication );
            GraphServiceClient graphClient
                = new GraphServiceClient( authProvider );

            _graphClient = graphClient;
        }

        public async Task<bool> CreateSecurityGroup( string groupName ) {
            try {

                if ( await GroupExistByName( groupName ) ) {
                    return false;
                }

                Group groupToAdd = new Group() {
                    DisplayName = groupName,
                    Description
                    = string.Format( _groupDescriptionFormat, groupName ),
                    SecurityEnabled = true,
                    MailEnabled = false,
                    MailNickname = groupName
                };

                var groupAdded
                    = await _graphClient.Groups
                    .Request()
                    .AddAsync( groupToAdd );

                if ( groupAdded == null ) {
                    return false;
                }
            }

            catch ( Microsoft.Graph.ServiceException ex ) {
                _logger.LogError( ex.Message );
                throw new Exception( ex.Message );
            }

            return true;
        }

        public async Task<bool> CheckForMembershipInGroupList( string accountId,
            List<string> groupIds ) {

            var groupChecked
                = await _graphClient.Users[accountId]
                .CheckMemberObjects( groupIds )
                .Request()
                .PostAsync();

            return groupChecked.Count > 0;
        }

        public async Task<List<string>> GetGroupsAssociatedWithTheUser( string accountId ) {
            List<string> groupNames = new List<string>();
            var groupsResult
                = await _graphClient.Users[accountId]
                .MemberOf
                .Request()
                .GetAsync();

            foreach ( Group group in groupsResult.CurrentPage ) {
                groupNames.Add( group.DisplayName );
            }
            return groupNames;
        }

        public async Task<List<string>> GetGroupIdsByNames( List<string> groupNames ) {
            try {

                if ( groupNames.Count == 0 ) {
                    return null;
                }

                string filter = string.Format( _filterFormat, groupNames[0] );
                for ( int i = 1; i < groupNames.Count; i++ ) {
                    filter += " or " + string.Format( _filterFormat, groupNames[i] );
                }

                var searchResult = await _graphClient.Groups
                    .Request()
                    .Filter( filter )
                    .GetAsync();

                List<string> idsList = new List<string>();
                foreach ( Group group in searchResult.CurrentPage ) {
                    idsList.Add( group.Id );
                }

                return idsList;
            }
            catch ( Exception ex ) {
                _logger.LogError( ex.Message );
                return null;
            }
        }

        public async Task<string> GetGroupIdByName( string groupName ) {
            var searchResult = await _graphClient.Groups
                .Request()
                .Filter( string.Format( _filterFormat, groupName ) )
                .GetAsync();

            if ( searchResult.CurrentPage.Count > 0 ) {
                return searchResult.CurrentPage[0].Id;
            }
            else {
                return null;
            }
        }

        public async Task<bool> GroupExistByName( string groupName ) {
            return await GetGroupIdByName( groupName ) != null;
        }

        public async Task AddMember( string groupId, string userAccountId ) {
            var directoryObject = new DirectoryObject {
                Id = userAccountId
            };

            await _graphClient.Groups[groupId].Members.References
                .Request().AddAsync( directoryObject );
        }
    }
}
