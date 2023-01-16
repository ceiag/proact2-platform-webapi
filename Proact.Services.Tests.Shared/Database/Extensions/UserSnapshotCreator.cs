using Proact.Services.Entities;
using Proact.Services.QueriesServices;
using System;

namespace Proact.Services.Tests.Shared {
    public static class UserSnapshotCreator {
        public static DatabaseSnapshotProvider AddUserWithRandomValues(
            this DatabaseSnapshotProvider snapshotProvider, Institute institute, out User user ) {

            user = new User() {
                Id = Guid.NewGuid(),
                InstituteId = institute.Id,
                AccountId = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString()
            };

            snapshotProvider.ServiceProvider
                .GetQueriesService<IUserQueriesService>().Create( user );

            snapshotProvider.ServiceProvider.Database.SaveChanges();

            return snapshotProvider;
        }

        public static DatabaseSnapshotProvider AddAnonymousUserWithRandomValues(
            this DatabaseSnapshotProvider snapshotProvider, Institute institute, out User user ) {

            user = new User() {
                Id = Guid.NewGuid(),
                InstituteId = institute.Id,
                AccountId = Guid.NewGuid().ToString(),
                Name = ""
            };

            snapshotProvider.ServiceProvider
                .GetQueriesService<IUserQueriesService>().Create( user );

            snapshotProvider.ServiceProvider.Database.SaveChanges();

            return snapshotProvider;
        }

        public static DatabaseSnapshotProvider AddSystemAdmin(
            this DatabaseSnapshotProvider snapshotProvider, out User systemAdmin ) {

            systemAdmin = new User() {
                Id = Guid.NewGuid(),
                InstituteId = Guid.Empty,
                AccountId = Guid.NewGuid().ToString(),
                Name = "system admin"
            };

            snapshotProvider.ServiceProvider
                .GetQueriesService<IUserQueriesService>().Create( systemAdmin );

            snapshotProvider.ServiceProvider.Database.SaveChanges();

            return snapshotProvider;
        }
    }
}
