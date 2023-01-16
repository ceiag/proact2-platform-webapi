using Proact.Services.Entities;
using Proact.Services.Models;
using Proact.Services.QueriesServices;
using System;

namespace Proact.Services.Tests.Shared {
    public static class InstituteSnapshotCreator {
        public static DatabaseSnapshotProvider AddInstituteWithRandomValues(
            this DatabaseSnapshotProvider snapshotProvider, out Institute institute ) {

            var instituteCreationRequest = new InstituteCreationRequest() {
                Name = Guid.NewGuid().ToString()
            };

            institute = snapshotProvider.ServiceProvider
                .GetQueriesService<IInstitutesQueriesService>()
                .Create( instituteCreationRequest );

            snapshotProvider.ServiceProvider.Database.SaveChanges();

            return snapshotProvider;
        }

        public static DatabaseSnapshotProvider AddInstituteWithRandomValues(
            this DatabaseSnapshotProvider snapshotProvider ) {
            Institute institute = null;

            return AddInstituteWithRandomValues( snapshotProvider, out institute );
        }

        public static DatabaseSnapshotProvider AddInstituteAdminWithRandomValues(
            this DatabaseSnapshotProvider snapshotProvider, Institute institute, out User admin ) {

            admin = new User() {
                Id = Guid.NewGuid(),
                InstituteId = institute.Id,
                AccountId = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString()
            };

            snapshotProvider.ServiceProvider
                .GetQueriesService<IUserQueriesService>().Create( admin );

            snapshotProvider.ServiceProvider.Database.SaveChanges();

            snapshotProvider.ServiceProvider
                .GetQueriesService<IInstitutesQueriesService>()
                .AssignAdmin( admin, institute.Id );

            snapshotProvider.ServiceProvider.Database.SaveChanges();

            return snapshotProvider;
        }
    }
}
