using Microsoft.EntityFrameworkCore;
using Proact.Services.Entities;
using Proact.Services.Models;
using System;
using System.Linq;

namespace Proact.Services.QueriesServices {
    public class ProjectPropertiesQueriesService : IProjectPropertiesQueriesService {
        private readonly ProactDatabaseContext _database;

        public ProjectPropertiesQueriesService( ProactDatabaseContext database ) {
            _database = database;
        }

        public ProjectProperties Create( Guid projectId, ProjectPropertiesCreateRequest request ) {
            var projectProperties = new ProjectProperties() {
                ProjectId = projectId,
                MedicsCanSeeOtherAnalisys = request.MedicsCanSeeOtherAnalisys,
                MessageCanNotBeDeletedAfterMinutes = request.MessageCanNotBeDeletedAfterMinutes,
                MessageCanBeAnalizedAfterMinutes = request.MessageCanBeAnalizedAfterMinutes,
                MessageCanBeRepliedAfterMinutes = request.MessageCanBeRepliedAfterMinutes,
                IsAnalystConsoleActive = request.IsAnalystConsoleActive,
                IsSurveysSystemActive = request.IsSurveysSystemActive,
                IsMessagingActive = request.IsMessagingActive
            };

            return _database.ProjectProperties.Add( projectProperties ).Entity;
        }

        public ProjectProperties Update( Guid projectId, ProjectPropertiesUpdateRequest request ) {
            var projectProperties = GetByProjectId( projectId );

            projectProperties.MedicsCanSeeOtherAnalisys = request.MedicsCanSeeOtherAnalisys;
            projectProperties.MessageCanNotBeDeletedAfterMinutes = request.MessageCanNotBeDeletedAfterMinutes;
            projectProperties.MessageCanBeAnalizedAfterMinutes = request.MessageCanBeAnalizedAfterMinutes;
            projectProperties.MessageCanBeRepliedAfterMinutes = request.MessageCanBeRepliedAfterMinutes;
            projectProperties.IsAnalystConsoleActive = request.IsAnalystConsoleActive;
            projectProperties.IsSurveysSystemActive = request.IsSurveysSystemActive;
            projectProperties.IsMessagingActive = request.IsMessagingActive;

            return projectProperties;
        }

        public ProjectProperties Get( Guid projectPropertiesId ) {
            return _database.ProjectProperties
                .Include( x => x.Project )
                .Include( x => x.Lexicon )
                .FirstOrDefault( x => x.Id == projectPropertiesId );
        }

        public ProjectProperties GetByProjectId( Guid projectId ) {
            return _database.ProjectProperties
                 .Include( x => x.Project )
                 .Include( x => x.Lexicon )
                 .FirstOrDefault( x => x.ProjectId == projectId );
        }

        public void AddLexicon( Guid projectId, Guid lexiconId ) {
            GetByProjectId( projectId ).LexiconId = lexiconId;
        }
    }
}
