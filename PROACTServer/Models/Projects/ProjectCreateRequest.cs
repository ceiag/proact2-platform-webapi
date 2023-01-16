using System.ComponentModel.DataAnnotations;

namespace Proact.Services.Models {
    public class ProjectCreateRequest {
        [Required]
        [MaxLength(100, ErrorMessage = "Project Name too long" )]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string SponsorName { get; set; }
        [Required]
        public ProjectPropertiesCreateRequest Properties { get; set; }
    }
}
