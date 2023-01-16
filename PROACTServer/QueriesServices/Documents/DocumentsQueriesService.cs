using Proact.Services.Entities;
using Proact.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Proact.Services.QueriesServices {
    public class DocumentsQueriesService : IDocumentsQueriesService {
        private readonly ProactDatabaseContext _database;

        public DocumentsQueriesService( ProactDatabaseContext database ) {
            _database = database;
        }

        public Document Create( 
            Guid instituteId, DocumentType type, string url, string fileName, DocumentCreationRequest request ) {
            var document = new Document() {
                InstituteId = instituteId,
                Type = type,
                Url = url,
                Title = request.Title,
                Description = request.Description,
                FileName = fileName,
            };

            return _database.Documents.Add( document ).Entity;
        }

        public Document Get( Guid documentId ) {
            return _database.Documents.FirstOrDefault( x => x.Id == documentId );
        }

        public Document Get( Guid instituteId, DocumentType type ) {
            return _database.Documents
                .FirstOrDefault( x => x.InstituteId == instituteId && x.Type == type );
        }

        public void Delete( Guid documentId ) {
            _database.Documents.Remove( Get( documentId ) );
        }

        public List<Document> GetAll( Guid instituteId ) {
            return _database.Documents.Where( x => x.InstituteId == instituteId ).ToList();
        }
    }
}
