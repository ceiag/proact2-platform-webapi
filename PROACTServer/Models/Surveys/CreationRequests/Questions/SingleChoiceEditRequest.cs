using System;
using System.ComponentModel.DataAnnotations;

namespace Proact.Services.Models {
    public class SingleChoiceEditRequest : SingleChoiceCreationRequest {
        [Required]
        public Guid QuestionId { get; set; }
    }
}
