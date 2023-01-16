using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Proact.Services.Models {
    public class SurveyQuestionCompileRequest {
        [Required]
        public Guid QuestionId { get; set; }
        [Required]
        public List<SurveyAnswerCompileRequest> Answers { get; set; }
    }
}
