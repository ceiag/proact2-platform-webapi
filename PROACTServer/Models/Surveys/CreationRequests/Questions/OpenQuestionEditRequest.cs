using System;
using System.ComponentModel.DataAnnotations;

namespace Proact.Services.Models {
    public class OpenQuestionEditRequest : OpenQuestionCreationRequest {
        [Required]
        public Guid QuestionId { get; set; }
    }
}
