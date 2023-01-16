using Proact.Services.Entities.MessageAnalysis;
using Proact.Services.Models;
using System;
using System.Collections.Generic;

namespace Proact.Services.QueriesServices {
    public interface ILexiconLabelQueriesService : IQueriesService {
        public LexiconLabel Create( LexiconCategory lexiconCategory, LexiconLabelCreationRequest request );
        public LexiconLabel Update( Guid labelId, LexiconLabelUpdateRequest request );
        public LexiconLabel Get( Guid lexiconLabelId );
        public LexiconLabel GetByName( Guid lexiconCategoryId, string label );
        public List<LexiconLabel> GetAll( Guid lexiconCategoryId );
        public void Delete( Guid labelId );
    }
}
