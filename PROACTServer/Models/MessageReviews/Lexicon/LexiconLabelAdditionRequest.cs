using System;

namespace Proact.Services.Models {
    public class LexiconLabelAdditionRequest {
        public Guid LexiconCategoryId { get; set; }
        public string Label { get; set; }
    }
}
