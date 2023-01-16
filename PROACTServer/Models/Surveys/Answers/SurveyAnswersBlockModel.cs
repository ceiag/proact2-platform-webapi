using System;
using System.Collections.Generic;

namespace Proact.Services.Models {
    public class SurveyAnswersBlockModel {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public List<SurveyAnswerModel> Answers { get; set; }
    }
}
