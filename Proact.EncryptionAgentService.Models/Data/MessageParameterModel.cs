using System;
using System.ComponentModel.DataAnnotations;

namespace Proact.EncryptionAgentService.Models {
    public class MessageParameterModel {
        [Required]
        public Guid MessageId { get; set; }
    }
}
