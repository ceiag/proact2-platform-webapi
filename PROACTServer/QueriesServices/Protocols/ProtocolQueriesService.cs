using Proact.Services.Entities;
using Proact.Services.Models;
using System;
using System.Linq;

namespace Proact.Services.QueriesServices {
    public class ProtocolQueriesService : IProtocolQueriesService {
        public readonly ProactDatabaseContext _database;

        public ProtocolQueriesService( ProactDatabaseContext database ) {
            _database = database;
        }

        public Protocol Create( ProtocolCreationRequest request ) {
            var projProtocol = new Protocol() {
                Name = request.Name,
                InternalCode = request.InternalCode,
                InternationalCode = request.InternationalCode,
            };

            return _database.Protocols.Add( projProtocol ).Entity;
        }

        public Protocol Get( Guid protocolId ) {
            return _database.Protocols.FirstOrDefault( x => x.Id == protocolId );
        }

        public Protocol GetByProjectId( Guid projectId ) {
           var projectProtocol = _database.ProjectProtocols
                .FirstOrDefault( x => x.ProjectId == projectId );

            if ( projectProtocol != null ) {
                return projectProtocol.Protocol;
            }

            return null;
        }

        public Protocol GetByUserId( Guid userId ) {
            var projectProtocol = _database.UserProtocols.FirstOrDefault( x => x.UserId == userId );

            if ( projectProtocol != null ) {
                return projectProtocol.Protocol;
            }

            return null;
        }

        public void DeleteByProjectId( Guid projecId ) {
            _database.Protocols.Remove( GetByProjectId( projecId ) );
        }

        public void DeleteByPatientId( Guid patientId ) {
            _database.Protocols.Remove( GetByUserId( patientId ) );
        }

        public ProjectProtocol AssignProtocolToProject( Guid protocolId, Guid projectId ) {
            var projectProtocol = new ProjectProtocol() {
                ProtocolId = protocolId,
                ProjectId = projectId,
            };

            return _database.ProjectProtocols.Add( projectProtocol ).Entity;
        }

        public UserProtocol AssignProtocolToUser( Guid protocolId, Guid userId ) {
            var patientProtocol = new UserProtocol() {
                ProtocolId = protocolId,
                UserId = userId,
            };

            return _database.UserProtocols.Add( patientProtocol ).Entity;
        }
    }
}
