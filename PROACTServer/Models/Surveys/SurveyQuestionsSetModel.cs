using System;
using System.Collections.Generic;

namespace Proact.Services.Models {
    public class SurveyQuestionsSetModel {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Version { get; set; }
        public QuestionsSetsState State { get; set; }
        public List<SurveyQuestionModel> Questions { get; set; }
    }
}
