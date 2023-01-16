using System;
using System.Collections.Generic;

namespace Proact.Services.Entities.MessageAnalysis {
    public class Analysis : TrackableEntity, IEntity {
        public Analysis() {
            AnalysisResults = new List<AnalysisResult>();
        }

        public AnalysisState State { get; set; } = AnalysisState.Current;
        public virtual Guid MessageId { get; set; }
        public virtual Message Message { get; set; }
        public virtual Guid UserId { get; set; }
        public virtual User User { get; set; }
        public virtual List<AnalysisResult> AnalysisResults { get; set; }
    }
}
