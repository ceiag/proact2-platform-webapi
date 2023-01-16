using System;
using System.ComponentModel.DataAnnotations;

namespace Proact.EncryptionAgentService.Entities {
    public class MessageData {
        public Guid Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Body { get; set; }
    }
}
