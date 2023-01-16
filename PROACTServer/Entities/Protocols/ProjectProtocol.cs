using System;

namespace Proact.Services.Entities {
    public class ProjectProtocol : TrackableEntity, IEntity {
        public virtual Guid ProjectId { get; set; }
        public virtual Guid ProtocolId { get; set; }
        public virtual Protocol Protocol { get; set; }
    }
}
