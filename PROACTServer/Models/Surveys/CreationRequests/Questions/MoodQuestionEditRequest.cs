using System;
using System.ComponentModel.DataAnnotations;

namespace Proact.Services.Models {
    public class MoodQuestionEditRequest : MoodQuestionCreationRequest {
        [Required]
        public Guid QuestionId { get; set; }
    }
}
