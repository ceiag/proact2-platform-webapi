using Proact.Services.Entities;
using System.ComponentModel.DataAnnotations;

namespace Proact.Services.Models {
    public class ProjectUpdateRequest : ProjectCreateRequest {
        [Required]
        public ProjectState Status { get; set; }
    }
}
