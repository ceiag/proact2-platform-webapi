using System;
using System.Collections.Generic;

namespace Proact.Services.Models {
    public class SurveyCompiledModel {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid AssignationId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Version { get; set; }
        public SurveyState SurveyState { get; set; } = SurveyState.DRAW;
        public DateTime StartTime { get; set; }
        public DateTime ExpireTime { get; set; }
        public DateTime? CompletedDateTime { get; set; }
        public List<SurveyCompiledQuestionModel> Questions { get; set; }
    }
}
