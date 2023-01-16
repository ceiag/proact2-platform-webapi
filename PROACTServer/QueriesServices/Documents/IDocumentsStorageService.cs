using Proact.Services.Entities;
using Proact.Services.Models;
using System;
using System.Threading.Tasks;

namespace Proact.Services.QueriesServices {
    public interface IDocumentsStorageService : IDataEditorService {
        public Task<DocumentModel> AddDocument( 
            Guid instituteId, DocumentType type, DocumentCreationRequest request );
    }
}
