using System;
using System.ComponentModel.DataAnnotations;

namespace Proact.Services.Entities {
    public class MessageReplies : TrackableEntity, IEntity {
        public virtual Guid OriginalMessageId { get; set; }
        [Required]
        public virtual Message OriginalMessage { get; set; }
        public virtual Guid? ReplyMessageId { get; set; }
        public virtual Message ReplyMessage { get; set; }
    }
}
