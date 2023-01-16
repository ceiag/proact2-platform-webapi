using System;

namespace Proact.Services.Models {
    public class SurveyAnswerCompileRequest {
        public Guid AnswerId { get; set; }
        public string Value { get; set; }
    }
}
