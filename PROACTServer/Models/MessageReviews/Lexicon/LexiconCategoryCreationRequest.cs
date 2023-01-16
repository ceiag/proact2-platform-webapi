using System.Collections.Generic;

namespace Proact.Services.Models {
    public class LexiconCategoryCreationRequest {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool MultipleSelection { get; set; }
        public List<LexiconLabelCreationRequest> Labels { get; set; }
    }
}
