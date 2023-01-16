using System;

namespace Proact.Services.Models {
    public class AnalysisResultModel {
        public int Order { get; set; }
        public string CategoryName { get; set; }
        public string ResultLabel { get; set; }
        public Guid LabelId { get; set; }
        public Guid CategoryId { get; set; }
    }
}
