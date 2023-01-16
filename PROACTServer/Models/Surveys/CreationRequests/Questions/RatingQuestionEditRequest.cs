using System;
using System.ComponentModel.DataAnnotations;

namespace Proact.Services.Models {
    public class RatingQuestionEditRequest : RatingQuestionCreationRequest {
        [Required]
        public Guid QuestionId { get; set; }
    }
}
