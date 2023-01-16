using System;

namespace Proact.Services.Entities.MessageAnalysis {
    public class AnalysisResult : TrackableEntity, IEntity {
        public virtual Guid AnalysisId { get; set; }
        public virtual Analysis Analysis { get; set; }
        public virtual Guid LexiconLabelId { get; set; }
        public virtual LexiconLabel LexiconLabel { get; set; }
    }
}
