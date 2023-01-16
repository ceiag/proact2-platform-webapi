using System.Collections.Generic;

namespace Proact.Services.Models {
    public class LexiconCreationRequest {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<LexiconCategoryCreationRequest> Categories { get; set; }
    }
}
