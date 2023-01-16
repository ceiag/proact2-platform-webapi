using System.ComponentModel.DataAnnotations;

namespace Proact.Services.Models {
    public class InstituteCreationRequest {
        [Required]
        [StringLength( 32, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 1 )]
        public string Name { get; set; }
    }
}
