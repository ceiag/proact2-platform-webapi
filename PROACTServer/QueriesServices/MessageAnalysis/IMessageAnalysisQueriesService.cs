using Proact.Services.Entities.MessageAnalysis;
using Proact.Services.Models;
using System;
using System.Collections.Generic;

namespace Proact.Services.QueriesServices {
    public interface IMessageAnalysisQueriesService : IQueriesService {
        public Analysis Get( Guid analysisId );
        public IEnumerable<Analysis> GetAllAnalysisFromPatient( Guid userId );
        public Analysis Create( Guid authorUserId, AnalysisCreationRequest request );
        public void Delete( Guid analysisId );
        public Analysis Update( Guid analysisId, Guid authorUserId, AnalysisCreationRequest request );
        public void Deprecate( Guid analysisId );
    }
}
