using Proact.Services.Entities;
using Proact.Services.Models;
using System;
using System.Collections.Generic;

namespace Proact.Services.QueriesServices {
    public interface IDocumentsQueriesService : IQueriesService {
        public Document Create( 
            Guid instituteId, DocumentType type, string url, string fileName, DocumentCreationRequest request );
        public List<Document> GetAll( Guid instituteId );
        public Document Get( Guid instituteId, DocumentType type );
        public Document Get( Guid documentId );
        public void Delete( Guid documentId );
    }
}
