using System;
namespace Proact.Services {
    public class AssignRoleToUserRequest {
        public Guid UserId { get; set; }
        public string Role { get; set; }
    }
}
