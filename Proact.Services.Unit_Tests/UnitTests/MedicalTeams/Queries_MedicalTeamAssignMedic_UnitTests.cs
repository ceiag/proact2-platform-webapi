using Proact.Services.Entities;
using Proact.Services.QueriesServices;
using System;
using System.Collections.Generic;
using Xunit;

namespace Proact.Services.UnitTests.MedicalTeams {
    public class Queries_MedicalTeamAssignMedic_UnitTests {
        [Fact]
        public void MedicalTeamAssignMedicConsistencyTest() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                //arrange
                var project = mockHelper.CreateDummyProject();
                var medicalTeam = mockHelper.CreateDummyMedicalTeam( project );

                for ( int i = 0; i < 10; ++i ) {    
                    var user = mockHelper.CreateDummyUser();
                    mockHelper.CreateDummyMedic( user );

                    mockHelper.ServicesProvider
                        .GetQueriesService<IMedicQueriesService>()
                        .AddToMedicalTeam( user.Id, medicalTeam.Id );

                    mockHelper.ServicesProvider.SaveChanges();
                }

                mockHelper.ServicesProvider.SaveChanges();

                //assert
                var medicalTeamCreated = mockHelper.ServicesProvider
                    .GetQueriesService<IMedicalTeamQueriesService>().Get( medicalTeam.Id );

                Assert.Equal( 10, medicalTeamCreated.Medics.Count );
            }
        }

        [Fact]
        public void MedicalTeamRemoveMedicConsistencyTest() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                //arrange
                var project = mockHelper.CreateDummyProject();
                var medicalTeam = mockHelper.CreateDummyMedicalTeam( project );

                var medicAssigned = new List<Medic>();

                //act
                for ( int i = 0; i < 10; ++i ) {
                    var user = mockHelper.CreateDummyUser();
                    var medic = mockHelper.CreateDummyMedic( user );

                    mockHelper.ServicesProvider
                        .GetQueriesService<IMedicQueriesService>()
                        .AddToMedicalTeam( medic.UserId, medicalTeam.Id );

                    medicAssigned.Add( medic );
                }

                mockHelper.ServicesProvider.SaveChanges();

                for ( int i = 0; i < 5; ++i ) {
                    mockHelper.ServicesProvider
                        .GetQueriesService<IMedicQueriesService>()
                        .RemoveFromMedicalTeam( medicAssigned[i].UserId, medicalTeam );
                }

                mockHelper.ServicesProvider.SaveChanges();

                //assert
                var medicalTeamCreated = mockHelper.ServicesProvider
                        .GetQueriesService<IMedicalTeamQueriesService>()
                        .Get( medicalTeam.Id );

                Assert.True( medicalTeamCreated.Medics.Count == 5 );
            }
        }
    }
}
