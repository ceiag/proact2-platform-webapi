using System;
using System.ComponentModel.DataAnnotations;

namespace Proact.Services.Models {
    public class BoolQuestionEditRequest : BoolQuestionCreationRequest {
        [Required]
        public Guid QuestionId { get; set; }
    }
}
