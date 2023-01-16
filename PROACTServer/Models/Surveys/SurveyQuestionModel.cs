using Proact.Services.Entities;
using System;
using System.Text.Json.Serialization;

namespace Proact.Services.Models {
    public class SurveyQuestionModel {
        public Guid Id { get; set; }
        public Guid QuestionsSetId { get; set; }
        public string Title { get; set; }
        public string Question { get; set; }
        public SurveyQuestionType Type { get; set; }
        public int Order { get; set; }
        [JsonConverter( typeof( SurveyQuestionAnswersJsonConverter ) )]
        public ISurveyAnswersContainer AnswersContainer { get; set; }
        [JsonConverter( typeof( SurveyQuestionPropertiesJsonConverter ) )]
        public ISurveyQuestionProperties Properties { get; set; }
    }
}
