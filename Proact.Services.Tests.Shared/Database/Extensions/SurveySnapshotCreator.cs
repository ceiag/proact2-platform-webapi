using Proact.Services.Entities;
using Proact.Services.QueriesServices;
using System;

namespace Proact.Services.Tests.Shared {
    public static class SurveySnapshotCreator {
        public static DatabaseSnapshotProvider AddSurveyWithRandomValues(
            this DatabaseSnapshotProvider snapshotProvider, Project project, out Survey survey ) {

            var request = new SurveyCreationRequest() {
                Title = $"survey-title-{Guid.NewGuid()}",
                Description = $"survey-descr-{Guid.NewGuid()}",
                Version = $"v-{Guid.NewGuid()}"
            };

            survey = snapshotProvider.ServiceProvider
                .GetQueriesService<ISurveyQueriesService>()
                .Create( project.Id, request );

            snapshotProvider.ServiceProvider.Database.SaveChanges();

            return snapshotProvider;
        }
    }
}
