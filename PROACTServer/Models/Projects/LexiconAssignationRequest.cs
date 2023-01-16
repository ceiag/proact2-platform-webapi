using System;
using System.ComponentModel.DataAnnotations;

namespace Proact.Services.Models {
    public class LexiconAssignationRequest {
        [Required]
        public Guid LexiconId { get; set; }
    }
}
