using Proact.Services.Entities;
using Proact.Services.Models;
using System;

namespace Proact.Services.QueriesServices {
    public interface IProjectHtmlContentsQueriesService : IQueriesService {
        public ProjectHtmlContent Create(
            Guid projectId, ProjectHtmlType type, ProjectHtmlContentCreationRequest request );
        public ProjectHtmlContent GetByProjectId( Guid projectId, ProjectHtmlType type );
        public void DeleteByProjectId( Guid projectId, ProjectHtmlType type );
    }
}
