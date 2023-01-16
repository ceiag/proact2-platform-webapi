using Proact.Services.Entities;
using Proact.Services.QueriesServices;

namespace Proact.Services.Tests.Shared {
    public static class ResearcherSnapshotCreator {
        public static DatabaseSnapshotProvider AddResearcherWithRandomValues(
            this DatabaseSnapshotProvider snapshotProvider, 
            MedicalTeam medicalTeam, out Researcher researcher ) {

            User user = null;
            snapshotProvider.AddUserWithRandomValues( medicalTeam.Project.Institute, out user );

            researcher = snapshotProvider.ServiceProvider
                .GetQueriesService<IResearcherQueriesService>().Create( user.Id );

            snapshotProvider.ServiceProvider.Database.SaveChanges();

            snapshotProvider.ServiceProvider
                .GetQueriesService<IResearcherQueriesService>()
                .AddToMedicalTeam( user.Id, medicalTeam.Id );

            snapshotProvider.ServiceProvider.Database.SaveChanges();

            return snapshotProvider;
        }

        public static DatabaseSnapshotProvider AddResearcherToMedicalTeam(
            this DatabaseSnapshotProvider snapshotProvider, MedicalTeam medicalTeam, Researcher researcher ) {
            snapshotProvider.ServiceProvider
                .GetQueriesService<IResearcherQueriesService>()
                .AddToMedicalTeam( researcher.UserId, medicalTeam.Id );

            snapshotProvider.ServiceProvider.Database.SaveChanges();

            return snapshotProvider;
        }
    }
}
