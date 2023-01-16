using System.Collections.Generic;

namespace Proact.Services.AuthorizationPolicies {
    public class UserRoles {
        private List<string> _roles;

        public UserRoles( List<string> roles ) {
            _roles = roles;
        }

        public bool HasRoleOf( string role ) {
            return _roles.Contains( role );
        }
    }
}
