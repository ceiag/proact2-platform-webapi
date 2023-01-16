using System;
using System.ComponentModel.DataAnnotations;

namespace Proact.Services.Models {
    public class MultipleChoiceEditRequest : MultipleChoiceCreationRequest {
        [Required]
        public Guid QuestionId { get; set; }
    }
}
