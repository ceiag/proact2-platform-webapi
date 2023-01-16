using Proact.Services.Entities.MessageAnalysis;
using Proact.Services.Models;
using System;
using System.Collections.Generic;

namespace Proact.Services.QueriesServices {
    public interface ILexiconQueriesService : IQueriesService {
        public Lexicon Create( Guid instituteId, LexiconCreationRequest request );
        public Lexicon Get( Guid lexiconId );
        public Lexicon GetByName( string name );
        public List<Lexicon> GetAll( Guid instituteId );
        public void PublishLexicon( Guid lexiconId );
        public void Delete( Guid lexiconId );
    }
}
