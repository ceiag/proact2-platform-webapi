using Proact.Services.Entities;
using System;

namespace Proact.Services.Models {
    public class SurveyRatingAnswerContainer : ISurveyAnswersContainer {
        public SurveyQuestionType Type { get { return SurveyQuestionType.RATING; } }
        public Guid Id { get; set; }
        public int Value { get; set; }
    }
}
