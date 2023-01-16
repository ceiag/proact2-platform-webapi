using Proact.Services.Entities;
using Proact.Services.Models;
using System;

namespace Proact.Services.QueriesServices {
    public interface IProtocolQueriesService : IQueriesService {
        public Protocol Create( ProtocolCreationRequest request );
        public Protocol Get( Guid protocolId );
        public Protocol GetByProjectId( Guid projectId );
        public Protocol GetByUserId( Guid patientId );
        public void DeleteByProjectId( Guid projecId );
        public void DeleteByPatientId( Guid patientId );
        public ProjectProtocol AssignProtocolToProject( Guid protocolId, Guid projectId );
        public UserProtocol AssignProtocolToUser( Guid protocolId, Guid userId );
    }
}
