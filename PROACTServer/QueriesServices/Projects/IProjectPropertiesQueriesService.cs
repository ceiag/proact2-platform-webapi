using Proact.Services.Entities;
using Proact.Services.Models;
using System;

namespace Proact.Services.QueriesServices {
    public interface IProjectPropertiesQueriesService : IQueriesService {
        public ProjectProperties Create( Guid projectId, ProjectPropertiesCreateRequest request );
        public ProjectProperties Update( Guid projectId, ProjectPropertiesUpdateRequest request );
        public ProjectProperties Get( Guid projectPropertiesId );
        public ProjectProperties GetByProjectId( Guid projectId );
        public void AddLexicon( Guid projectId, Guid lexiconId );
    }
}
