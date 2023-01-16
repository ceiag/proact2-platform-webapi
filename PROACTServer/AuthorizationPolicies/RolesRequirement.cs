using System;
using Microsoft.AspNetCore.Authorization;

namespace Proact.Services.AuthorizationPolicies {
 
    public class RolesRequirement : IAuthorizationRequirement {
        public readonly string[] RolesName;

        public RolesRequirement( params string[] rolesName ) {
            RolesName = rolesName;
        }
    }
}
