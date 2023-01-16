using System.ComponentModel.DataAnnotations;

namespace Proact.Services.Models {
    public class RatingQuestionCreationRequest : QuestionCreationRequest {
        [Required]
        public int Min { get; set; }
        [Required]
        public int Max { get; set; }
        [Required]
        public string MinLabel { get; set; }
        [Required]
        public string MaxLabel { get; set; }
    }
}
