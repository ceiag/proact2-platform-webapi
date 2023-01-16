using Microsoft.AspNetCore.Http;
using Proact.Services.Models;
using System;
using System.Threading.Tasks;

namespace Proact.Services.QueriesServices {
    public interface IProtocolStorageService : IDataEditorService {
        public Task<ProtocolModel> AddProtocolToProjectOverrideIfExist( 
            Guid projectId, IFormFile pdfFile, ProtocolCreationRequest request );
        public Task<ProtocolModel> AddProtocolToPatientOverrideIfExist(
            Guid userId, IFormFile pdfFile, ProtocolCreationRequest request );
        public ProtocolModel GetByProjectId( Guid projectId );
        public ProtocolModel GetByUserId( Guid userId );
        public ProtocolModel GetById( Guid protocolId );
        public UserProtocolsModel GetUserProtocols( Guid userId );
    }
}
