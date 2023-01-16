using System;
using System.Collections.Generic;

namespace Proact.Services.Models {
    public class AnalysisCreationRequest {
        public Guid MessageId { get; set; }
        public List<AnalysisResultCreationRequest> AnalysisResults { get; set; }
    }
}
