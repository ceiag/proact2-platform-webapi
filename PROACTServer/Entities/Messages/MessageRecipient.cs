using System;
using System.ComponentModel.DataAnnotations;

namespace Proact.Services.Entities {
    public class MessageRecipient {
        public Guid MessageId { get; set; }

        [Required]
        public virtual Message Message { get; set; }

        public virtual Guid UserId { get; set; }

        public virtual User User { get; set; }

        public bool IsRead { get; set; }

        public DateTime? ReadTime { get; set; }
    }
}
