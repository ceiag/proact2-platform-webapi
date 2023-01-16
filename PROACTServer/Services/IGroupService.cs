using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Graph;

namespace Proact.Services.Services {
    public interface IGroupService {
        Task<bool> CreateSecurityGroup( string groupName );
        Task<bool> CheckForMembershipInGroupList( string accountId, List<string> groupIds );
        Task<List<string>> GetGroupsAssociatedWithTheUser( string accountId );
        Task<string> GetGroupIdByName( string groupName );
        Task<List<string>> GetGroupIdsByNames( List<string> groupNames );
        Task<bool> GroupExistByName( string groupName );
        Task AddMember( string groupId, string userAccountId );
    }
}
