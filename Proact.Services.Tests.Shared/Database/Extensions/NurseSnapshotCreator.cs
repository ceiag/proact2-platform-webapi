using Proact.Services.Entities;
using Proact.Services.QueriesServices;

namespace Proact.Services.Tests.Shared {
    public static class NurseSnapshotCreator {
        public static DatabaseSnapshotProvider AddNurseWithRandomValues(
            this DatabaseSnapshotProvider snapshotProvider, MedicalTeam medicalTeam, out Nurse nurse ) {

            User user = null;
            snapshotProvider.AddUserWithRandomValues( medicalTeam.Project.Institute, out user );

            nurse = snapshotProvider.ServiceProvider
                .GetQueriesService<INurseQueriesService>().Create( user.Id );

            snapshotProvider.ServiceProvider.Database.SaveChanges();

            snapshotProvider.ServiceProvider
                .GetQueriesService<INurseQueriesService>().AddToMedicalTeam( user.Id, medicalTeam.Id );

            snapshotProvider.ServiceProvider.Database.SaveChanges();

            return snapshotProvider;
        }

        public static DatabaseSnapshotProvider AddNurseToMedicalTeam(
            this DatabaseSnapshotProvider snapshotProvider, MedicalTeam medicalTeam, Nurse nurse ) {
            snapshotProvider.ServiceProvider
                .GetQueriesService<INurseQueriesService>()
                .AddToMedicalTeam( nurse.UserId, medicalTeam.Id );

            snapshotProvider.ServiceProvider.Database.SaveChanges();

            return snapshotProvider;
        }
    }
}
