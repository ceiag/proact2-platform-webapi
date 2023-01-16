using Proact.Services.Entities;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Proact.Services.Models {
    public class SurveyCompiledQuestionModel {
        public Guid QuestionsSetId { get; set; }
        public Guid QuestionId { get; set; }
        public string Title { get; set; }
        public string Question { get; set; }
        public int Order { get; set; }
        public SurveyQuestionType Type { get; set; }
        [JsonConverter( typeof( SurveyQuestionPropertiesJsonConverter ) )]
        public ISurveyQuestionProperties Properties { get; set; }
        public List<SurveyCompiledQuestionAnswerModel> CompiledAnswers { get; set; }
    }
}
