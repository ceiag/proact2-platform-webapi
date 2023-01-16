using Proact.Services.Entities;
using Proact.Services.QueriesServices;

namespace Proact.Services.Tests.Shared {
    public static class MedicSnapshotCreator {
        public static DatabaseSnapshotProvider AddMedicWithRandomValues(
            this DatabaseSnapshotProvider snapshotProvider, MedicalTeam medicalTeam, out Medic medic ) {

            User user = null;
            snapshotProvider.AddUserWithRandomValues( medicalTeam.Project.Institute, out user );

            medic = snapshotProvider.ServiceProvider
                .GetQueriesService<IMedicQueriesService>().Create( user.Id );

            snapshotProvider.ServiceProvider.Database.SaveChanges();

            snapshotProvider.ServiceProvider
                .GetQueriesService<IMedicQueriesService>().AddToMedicalTeam( user.Id, medicalTeam.Id );

            snapshotProvider.ServiceProvider.Database.SaveChanges();

            return snapshotProvider;
        }

        public static DatabaseSnapshotProvider AddMedicToMedicalTeam(
            this DatabaseSnapshotProvider snapshotProvider, MedicalTeam medicalTeam, Medic medic ) {

            snapshotProvider.ServiceProvider
                .GetQueriesService<IMedicQueriesService>()
                .AddToMedicalTeam( medic.UserId, medicalTeam.Id );

            snapshotProvider.ServiceProvider.Database.SaveChanges();

            return snapshotProvider;
        }
    }
}
