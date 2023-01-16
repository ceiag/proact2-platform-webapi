using System;
using System.Collections.Generic;

namespace Proact.Services.Entities.MessageAnalysis {
    public class Lexicon : TrackableEntity, IEntity {
        public Lexicon() {
            Categories = new List<LexiconCategory>();
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public LexiconState State { get; set; }
        public virtual List<LexiconCategory> Categories { get; set; }
        public virtual Guid? InstituteId { get; set; }
    }
}
