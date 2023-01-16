using Proact.Services.Entities;
using Proact.Services.Models;
using Proact.Services.QueriesServices;
using System;

namespace Proact.Services.Tests.Shared {
    public static class ProjectSnapshotCreator {
        public static DatabaseSnapshotProvider AddProjectWithRandomValues(
            this DatabaseSnapshotProvider snapshotProvider, Institute institute, out Project project ) {

            var projectCreateRequest = new ProjectCreateRequest() {
                Name = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString(),
                SponsorName = Guid.NewGuid().ToString(),
                Properties = new ProjectPropertiesCreateRequest() {
                    IsAnalystConsoleActive = true,
                    IsSurveysSystemActive = true,
                    MedicsCanSeeOtherAnalisys = true,
                    MessageCanBeAnalizedAfterMinutes = 0,
                    MessageCanBeRepliedAfterMinutes = 0,
                    MessageCanNotBeDeletedAfterMinutes = 0,
                }
            };

            project = snapshotProvider.ServiceProvider
                .GetQueriesService<IProjectQueriesService>().Create( institute.Id, projectCreateRequest );

            snapshotProvider.ServiceProvider.Database.SaveChanges();

            snapshotProvider.ServiceProvider
                .GetQueriesService<IProjectPropertiesQueriesService>()
                .Create( project.Id, projectCreateRequest.Properties );

            snapshotProvider.ServiceProvider.Database.SaveChanges();

            return snapshotProvider;
        }
    }
}
