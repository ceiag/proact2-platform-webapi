using System.ComponentModel.DataAnnotations;

namespace Proact.Services.Models {
    public class QuestionCreationRequest {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Question { get; set; }
    }
}
