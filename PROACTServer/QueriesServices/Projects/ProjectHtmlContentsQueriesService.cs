using Proact.Services.Entities;
using Proact.Services.Models;
using System;
using System.Linq;

namespace Proact.Services.QueriesServices {
    public class ProjectHtmlContentsQueriesService : IProjectHtmlContentsQueriesService {
        private readonly ProactDatabaseContext _database;

        public ProjectHtmlContentsQueriesService( ProactDatabaseContext database ) {
            _database = database;
        }

        public ProjectHtmlContent Create( 
            Guid projectId, ProjectHtmlType type, ProjectHtmlContentCreationRequest request ) {
            var projectContacts = new ProjectHtmlContent() {
                ProjectId = projectId,
                HtmlContent = request.HtmlContent,
                Type = type
            };

            return _database.ProjectHtmlContents.Add( projectContacts ).Entity;
        }

        public ProjectHtmlContent GetByProjectId( Guid projectId, ProjectHtmlType type ) {
            return _database.ProjectHtmlContents
                .FirstOrDefault( x => x.ProjectId == projectId && x.Type == type );
        }

        public void DeleteByProjectId( Guid projectId, ProjectHtmlType type ) {
            var projectContacts = GetByProjectId( projectId, type );

            if ( projectContacts != null ) {
                _database.ProjectHtmlContents.Remove( GetByProjectId( projectId, type ) );
            }
        }
    }
}
