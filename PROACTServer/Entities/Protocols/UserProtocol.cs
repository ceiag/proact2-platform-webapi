using System;

namespace Proact.Services.Entities {
    public class UserProtocol : TrackableEntity, IEntity {
        public Guid UserId { get; set; }
        public Guid ProtocolId { get; set; }
        public virtual Protocol Protocol { get; set; }
    }
}
