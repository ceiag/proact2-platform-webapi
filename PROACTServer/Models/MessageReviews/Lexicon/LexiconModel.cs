using Proact.Services.Entities.MessageAnalysis;
using System;
using System.Collections.Generic;

namespace Proact.Services.Models {
    public class LexiconModel {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public LexiconState State { get; set; }
        public List<LexiconCategoryModel> Categories { get; set; }
    }
}
