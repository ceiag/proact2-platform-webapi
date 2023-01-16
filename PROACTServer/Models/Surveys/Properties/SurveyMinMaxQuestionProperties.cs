using Proact.Services.Entities;

namespace Proact.Services.Models {
    public class SurveyMinMaxQuestionProperties : ISurveyQuestionProperties {
        public int Min { get; set; }
        public int Max { get; set; }
        public string MinLabel { get; set; }
        public string MaxLabel { get; set; }
        public SurveyQuestionType Type { 
            get { return SurveyQuestionType.RATING; }
        }
    }
}
