using System;

namespace Proact.Services.Entities {
    public class MessageData : TrackableEntity, IEntity {
        public string Title { get; set; }
        public string Body { get; set; }
        public virtual Guid MessageId { get; set; }
        public virtual Message Message { get; set; }
    }
}
