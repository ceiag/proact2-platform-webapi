using System.ComponentModel.DataAnnotations;

namespace Proact.Services.Models {
    public class SystemInitializationRequest {
        [Required]
        [MaxLength( 32 )]
        public string FirstName { get; set; }

        [Required]
        [MaxLength( 32 )]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
