using Microsoft.AspNetCore.Http;
using Proact.Services.AzureMediaServices;
using Proact.Services.Entities;
using Proact.Services.EntitiesMapper;
using Proact.Services.Models;
using Proact.Services.Models.Messages;
using System;
using System.Threading.Tasks;

namespace Proact.Services.QueriesServices {
    public class ProtocolStorageService : IProtocolStorageService {
        private readonly IProjectQueriesService _projectQueriesService;
        private readonly IProtocolQueriesService _protocolQueriesService;
        private readonly IFilesStorageService _mediaStorageService;
        private readonly ProactDatabaseContext _database;
        private readonly string _containerNamePrefix = "protocols-for-project-";

        public ProtocolStorageService(
            IProjectQueriesService projectQueriesService,
            IProtocolQueriesService projectProtocolQueriesService,
            IFilesStorageService mediaStorageService,
            ProactDatabaseContext database ) {
            _projectQueriesService = projectQueriesService;
            _protocolQueriesService = projectProtocolQueriesService;
            _mediaStorageService = mediaStorageService;
            _database = database;
        }

        private MediaFileStoringInfoModel GetMediaFileNameResolverForPatient( Guid projectId, Guid userId ) {
            return new MediaFileStoringInfoModel() {
                AttachmentType = AttachmentType.DOCUMENT_PDF,
                ContainerName = $"{_containerNamePrefix}{projectId}",
                FileName = $"protocol-for-user-{userId}{MediaFilesUploaderSettings.PdfExtensionFormat}",
                ContentType = MediaFilesUploaderSettings.PdfContentType,
            };
        }

        private MediaFileStoringInfoModel GetMediaFileNameResolverForProject( Guid projectId ) {
            return new MediaFileStoringInfoModel() {
                AttachmentType = AttachmentType.DOCUMENT_PDF,
                ContainerName = $"{_containerNamePrefix}{projectId}",
                FileName = $"protocol-for-project{MediaFilesUploaderSettings.PdfExtensionFormat}",
                ContentType = MediaFilesUploaderSettings.PdfContentType,
            };
        }

        private Project GetProjectAssociatedToPatient( Guid userId ) {
            return _projectQueriesService.GetProjectsWhereUserIsAssociated( userId )[0];
        }

        private void DeleteProjectProtocolEntityIfExist( Guid projectId ) {
            var protocol = _protocolQueriesService.GetByProjectId( projectId );

            if ( protocol != null ) {
                _protocolQueriesService.DeleteByProjectId( projectId );
            }
        }

        private void DeletePatientProtocolEntityIfExist( Guid patientId ) {
            var protocol = _protocolQueriesService.GetByUserId( patientId );

            if ( protocol != null ) {
                _protocolQueriesService.DeleteByPatientId( patientId );
            }
        }

        private Protocol CreateEntityOnDatabase( ProtocolCreationRequest request ) {
            return _protocolQueriesService.Create( request );
        }

        public async Task<ProtocolModel> AddProtocolToProjectOverrideIfExist( 
            Guid projectId, IFormFile pdfFile, ProtocolCreationRequest request ) {
            DeleteProjectProtocolEntityIfExist( projectId );
            var projectProtocol = CreateEntityOnDatabase( request );

            var projectProtocolMediaFileInfo = GetMediaFileNameResolverForProject( projectId );

            await _mediaStorageService.UploadMediaFile(
                pdfFile.OpenReadStream(), AccessFolderType.PRIVATE, projectProtocolMediaFileInfo );
            _database.SaveChanges();

            _protocolQueriesService.AssignProtocolToProject( projectProtocol.Id, projectId );
            _database.SaveChanges();

            return ProtocolEntityMapper.Map( projectProtocol );
        }

        public async Task<ProtocolModel> AddProtocolToPatientOverrideIfExist(
             Guid userId, IFormFile pdfFile, ProtocolCreationRequest request ) {
            DeletePatientProtocolEntityIfExist( userId );
            var projectProtocol = CreateEntityOnDatabase( request );
            var project = GetProjectAssociatedToPatient( userId );

            var projectProtocolMediaFileInfo = 
                GetMediaFileNameResolverForPatient( project.Id, userId );

            await _mediaStorageService.UploadMediaFile(
                pdfFile.OpenReadStream(), AccessFolderType.PRIVATE, projectProtocolMediaFileInfo );
            _database.SaveChanges(); 

            _protocolQueriesService.AssignProtocolToUser( projectProtocol.Id, userId );
            _database.SaveChanges();

            return ProtocolEntityMapper.Map( projectProtocol );
        }

        public ProtocolModel GetById( Guid protocolId ) {
            return ProtocolEntityMapper.Map( _protocolQueriesService.Get( protocolId ) );
        }

        private ProtocolModel GetProtocolWithSASUri( MediaFileStoringInfoModel fileInfo, Protocol protocol ) {
            var uri = _mediaStorageService.GetMediaFileSASUri(
                fileInfo.ContainerName, fileInfo.FileName,
                MediaFilesUploaderSettings.SASUriDurationInMinutes );

            var protocolModel = ProtocolEntityMapper.Map( protocol );
            protocolModel.Url = uri.ToString();

            return protocolModel;
        }

        public ProtocolModel GetByProjectId( Guid projectId ) {
            var protocol = _protocolQueriesService.GetByProjectId( projectId );
            
            if ( protocol == null ) { return null; }

            return GetProtocolWithSASUri( GetMediaFileNameResolverForProject( projectId ), protocol );
        }

        public ProtocolModel GetByUserId( Guid userId ) {
            var protocol = _protocolQueriesService.GetByUserId( userId );
            var project = GetProjectAssociatedToPatient( userId );

            if ( protocol == null ) { return null; }

            return GetProtocolWithSASUri( 
                GetMediaFileNameResolverForPatient( project.Id, userId ), protocol );
        }

        public UserProtocolsModel GetUserProtocols( Guid userId ) {
            var userProtocolModel = new UserProtocolsModel();

            try {
                userProtocolModel.UserProtocol = GetByUserId( userId );
            }
            catch { userProtocolModel.UserProtocol = null; }

            try {
                var project = GetProjectAssociatedToPatient( userId );
                userProtocolModel.ProjectProtocol = GetByProjectId( project.Id );
            }
            catch { userProtocolModel.ProjectProtocol = null;  }

            return userProtocolModel;
        }
    }
}
