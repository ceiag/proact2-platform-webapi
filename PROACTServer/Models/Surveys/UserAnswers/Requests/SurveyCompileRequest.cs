using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Proact.Services.Models {
    public class SurveyCompileRequest {
        [Required]
        public List<SurveyQuestionCompileRequest> QuestionsCompiled { get; set; }
    }
}
