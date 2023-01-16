using System;
using System.Collections.Generic;

namespace Proact.Services.Entities.MessageAnalysis {
    public class LexiconCategory : TrackableEntity, IEntity {
        public LexiconCategory() {
            Labels = new List<LexiconLabel>();
        }

        public string Name { get; set; }
        public bool MultipleSelection { get; set; }
        public int Order { get; set; }
        public virtual Lexicon Lexicon { get; set; }
        public virtual Guid LexiconId { get; set; }
        public virtual List<LexiconLabel> Labels { get; set; }
    }
}
