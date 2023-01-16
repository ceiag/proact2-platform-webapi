using System;

namespace Proact.Services.Entities.MessageAnalysis {
    public class LexiconLabel : TrackableEntity, IEntity {
        public string Label { get; set; }
        public string GroupName { get; set; }
        public virtual Guid LexiconCategoryId { get; set; }
        public virtual LexiconCategory LexiconCategory { get; set; }
    }
}
