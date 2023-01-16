using Proact.Services.AuthorizationPolicies;
using Proact.Services.Entities;
using Proact.Services.Models.Messages;

namespace Proact.Services {
    public class MessageCreationParams {
        public User User { get; set; }
        public UserRoles UserRoles { get; set; }
        public MedicalTeam MedicalTeam { get; set; }
        public MessageRequestData MessageRequestData { get; set; }
        public bool ShowAfterCreation { get; set; }
    }
}
