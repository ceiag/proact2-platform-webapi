using System;
using System.ComponentModel.DataAnnotations;

namespace Proact.Services.Models.Messages {
    public class ReplyMessageRequest : MessageRequestData {
        [Required]
        public Guid OriginalMessageId { get; set; }
    }
}
