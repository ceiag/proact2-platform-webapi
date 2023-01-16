using Proact.Services.Entities;
using Proact.Services.Models;
using Proact.Services.QueriesServices;

namespace Proact.Services.Tests.Shared {
    public static class SurveySchedulerSnapshotCreator {
        public static DatabaseSnapshotProvider AddSurveyScheduler(
            this DatabaseSnapshotProvider snapshotProvider,
            CreateScheduledSurveyRequest request ) {

            snapshotProvider.ServiceProvider
                .GetQueriesService<ISurveySchedulerQueriesService>()
                .Create( request );

            snapshotProvider.ServiceProvider.Database.SaveChanges();
            return snapshotProvider;
        }

        public static DatabaseSnapshotProvider AddSurveyScheduler(
            this DatabaseSnapshotProvider snapshotProvider, SurveyScheduler scheduler ) {

            snapshotProvider.ServiceProvider
                .GetQueriesService<ISurveySchedulerQueriesService>()
                .Create( scheduler );

            snapshotProvider.ServiceProvider.Database.SaveChanges();
            return snapshotProvider;
        }
    }
}
