using System.ComponentModel.DataAnnotations;

namespace Proact.Services.Models {
    public class QuestionsSetCreationRequest {
        [Required]
        [MinLength( 1, ErrorMessage = "Name too short" )]
        [MaxLength( 100, ErrorMessage = "Name too long" )]
        public string Title { get; set; }
        [Required]
        [MinLength( 1, ErrorMessage = "Name too short" )]
        [MaxLength( 100, ErrorMessage = "Name too long" )]
        public string Description { get; set; }
        [Required]
        [MinLength( 1, ErrorMessage = "Name too short" )]
        [MaxLength( 100, ErrorMessage = "Name too long" )]
        public string Version { get; set; }
    }
}
