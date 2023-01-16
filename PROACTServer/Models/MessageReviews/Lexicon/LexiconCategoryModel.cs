using System;
using System.Collections.Generic;

namespace Proact.Services.Models {
    public class LexiconCategoryModel {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool MultipleSelection { get; set; }
        public int Order { get; set; }
        public List<LexiconLabelModel> Labels { get; set; }
    }
}
