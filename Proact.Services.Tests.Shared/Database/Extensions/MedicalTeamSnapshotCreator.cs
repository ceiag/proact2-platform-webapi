using Proact.Services.Entities;
using Proact.Services.Models;
using Proact.Services.QueriesServices;
using System;

namespace Proact.Services.Tests.Shared {
    public static class MedicalTeamSnapshotCreator {
        public static DatabaseSnapshotProvider AddMedicalTeamWithRandomValues(
            this DatabaseSnapshotProvider snapshotProvider, Project project, out MedicalTeam medicalTeam ) {

            var medicalTeamCreateRequest = new MedicalTeamCreateRequest() {
                AddressLine1 = Guid.NewGuid().ToString(),
                AddressLine2 = Guid.NewGuid().ToString(),
                City = Guid.NewGuid().ToString(),
                Country = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString(),
                Phone = Guid.NewGuid().ToString(),
                PostalCode = Guid.NewGuid().ToString(),
                RegionCode = "IT-GE",
                StateOrProvince = "IT",
                TimeZone = "GMT+1"
            };

            medicalTeam = snapshotProvider.ServiceProvider
                .GetQueriesService<IMedicalTeamQueriesService>()
                .Create( project.Id, medicalTeamCreateRequest );

            snapshotProvider.ServiceProvider.Database.SaveChanges();
            return snapshotProvider;
        }

        public static DatabaseSnapshotProvider AddAdminToMedicalTeam(
            this DatabaseSnapshotProvider snapshotProvider, Medic medic, MedicalTeam medicalTeam ) {

            snapshotProvider.ServiceProvider
                .GetQueriesService<IMedicalTeamQueriesService>()
                .AddAdmin( medicalTeam.Id, medic.UserId );

            snapshotProvider.ServiceProvider.Database.SaveChanges();
            return snapshotProvider;
        }
    }
}
