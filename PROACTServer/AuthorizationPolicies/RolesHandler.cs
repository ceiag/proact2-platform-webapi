using Microsoft.AspNetCore.Authorization;
using Proact.Services.Services;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Proact.Services.AuthorizationPolicies {

    public class RolesHandler : AuthorizationHandler<RolesRequirement> {

        private IGroupService _GroupService;

        public RolesHandler( IGroupService groupService ) {
            _GroupService = groupService;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            RolesRequirement requirement ) {

            var userClaim
                = context.User.FindFirst( ClaimTypes.NameIdentifier );

            if ( userClaim != null ) {

                var userAccountId = userClaim.Value;

                    List<string> roles = await _GroupService
                    .GetGroupsAssociatedWithTheUser( userAccountId );

                    List<string> requiredRoles
                        = requirement.RolesName.ToList();

                    bool authorized = false;
                    if ( roles.Any( x => requiredRoles.Any( y => y == x ) ) ) {
                        authorized = true;
                    }

                    if ( userAccountId != null && authorized ) {
                        foreach ( string role in roles ) {
                        context.User.Identities
                          .FirstOrDefault()
                          .AddClaim( new Claim( Roles.ClaimTypeRoles, role ) );
                        }

                        context.Succeed( requirement );
                    }  
            }

            return;
        }
    }
}
