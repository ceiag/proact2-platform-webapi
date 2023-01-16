using System;
using System.ComponentModel.DataAnnotations;

namespace Proact.EncryptionAgentService.Models {
    public class CreateMessageDataRequest {
        [Required]
        public Guid MessageId { get; set; }

        public string EncryptedTitle { get; set; }

        public string EncryptedBody { get; set; }

        public byte[] EncryptedKey { get; set; }

        public byte[] EncryptedIV { get; set; }
    }
}
