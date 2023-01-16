using Proact.Services.AzureMediaServices;
using Proact.Services.Entities;
using Proact.Services.EntitiesMapper;
using Proact.Services.Models;
using System;
using System.Threading.Tasks;

namespace Proact.Services.QueriesServices {
    public class DocumentsStorageService : IDocumentsStorageService {
        private readonly IDocumentsQueriesService _documentsQueriesService;
        private readonly IMediaFilesUploaderService _mediaFilesUploaderService;
        private readonly ProactDatabaseContext _database;

        public DocumentsStorageService( 
            IDocumentsQueriesService documentsQueriesService, 
            IMediaFilesUploaderService mediaFilesUploaderService, ProactDatabaseContext database ) { 
            _documentsQueriesService = documentsQueriesService;
            _mediaFilesUploaderService = mediaFilesUploaderService;
            _database = database;
        }

        private string GetDocumentFileName( Document document ) {
            return document.FileName.ToString();
        }

        private void DeleteDocumentRowFromDatabase( Guid documentId ) {
            _documentsQueriesService.Delete( documentId );
            _database.SaveChanges();
        }

        private async Task DeleteDocumentFileFromStorage( Document document ) {
            var fileInfo = MediaFileUploaderNamingResolver.CreateMediaFileNamingForDocumentPdf( 
                GetDocumentFileName( document ), document.InstituteId );

            await _mediaFilesUploaderService.DeleteMediaFile( fileInfo );
        }

        private async Task DeleteDocumentIfExist( Guid instituteId, DocumentType type ) {
            var existDocument = _documentsQueriesService.Get( instituteId, type );

            if ( existDocument != null ) {
                DeleteDocumentRowFromDatabase( existDocument.Id );
                await DeleteDocumentFileFromStorage( existDocument );
            }
        }

        public async Task<DocumentModel> AddDocument( 
            Guid instituteId, DocumentType type, DocumentCreationRequest request ) {

            await DeleteDocumentIfExist( instituteId, type );

            string fileName = Guid.NewGuid().ToString();
            var uploadResult = await _mediaFilesUploaderService.UploadDocumentPdfIntoContainer(  
                request.pdfFile.OpenReadStream(), fileName, instituteId );

            var documentCreated = _documentsQueriesService.Create(
                instituteId, type, uploadResult.ContentUrl, fileName, request );

            return DocumentEntityMapper.Map( documentCreated );
        }
    }
}
