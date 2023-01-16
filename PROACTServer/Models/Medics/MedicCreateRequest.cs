using System;
using System.ComponentModel.DataAnnotations;

namespace Proact.Services.Models {
    public class MedicCreateRequest {
        [Required]
        public Guid UserId { get; set; }
    }
}
