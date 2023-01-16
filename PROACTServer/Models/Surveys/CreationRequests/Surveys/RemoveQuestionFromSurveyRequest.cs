using System;
using System.ComponentModel.DataAnnotations;

namespace Proact.Services.Models {
    public class RemoveQuestionFromSurveyRequest {
        [Required]
        public Guid SurveyId { get; set; }
        [Required]
        public Guid QuestionsSetId { get; set; }
        [Required]
        public Guid QuestionId { get; set; }
    }
}
