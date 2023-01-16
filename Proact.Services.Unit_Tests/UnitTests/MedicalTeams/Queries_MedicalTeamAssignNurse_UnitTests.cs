using Proact.Services.Entities;
using Proact.Services.QueriesServices;
using Proact.Services.ServicesProviders;
using System.Collections.Generic;
using Xunit;

namespace Proact.Services.UnitTests.MedicalTeams {
    public class Queries_MedicalTeamAssignNurse_UnitTests {
        [Fact]
        public void MedicalTeamAssignNurseConsistencyTest() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                //arrange
                var project = mockHelper.CreateDummyProject();
                var medicalTeam = mockHelper.CreateDummyMedicalTeam( project );

                for ( int i = 0; i < 10; ++i ) {
                    var user = mockHelper.CreateDummyUser();
                    var nurse = mockHelper.CreateDummyNurse( user );

                    //act
                    mockHelper.ServicesProvider
                        .GetQueriesService<INurseQueriesService>()
                        .AddToMedicalTeam( nurse.UserId, medicalTeam.Id );
                }

                mockHelper.ServicesProvider.SaveChanges();

                //assert
                var medicalTeamCreated = mockHelper.ServicesProvider
                        .GetQueriesService<IMedicalTeamQueriesService>()
                        .Get( medicalTeam.Id );

                Assert.Equal( 10, medicalTeamCreated.Nurses.Count );
            }
        }

        [Fact]
        public void MedicalTeamRemoveNursConsistencyTest() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                //arrange
                var project = mockHelper.CreateDummyProject();
                var medicalTeam = mockHelper.CreateDummyMedicalTeam( project );

                var nursesAssigned = new List<Nurse>();

                //act
                for ( int i = 0; i < 10; ++i ) {
                    var user = mockHelper.CreateDummyUser();
                    var nurse = mockHelper.CreateDummyNurse( user );

                    mockHelper.ServicesProvider
                        .GetQueriesService<INurseQueriesService>()
                        .AddToMedicalTeam( nurse.UserId, medicalTeam.Id );

                    nursesAssigned.Add( nurse );
                }

                mockHelper.ServicesProvider.SaveChanges();

                for ( int i = 0; i < 5; ++i ) {
                    mockHelper.ServicesProvider
                        .GetQueriesService<INurseQueriesService>()
                        .RemoveFromMedicalTeam( nursesAssigned[i].UserId, medicalTeam );
                }

                mockHelper.ServicesProvider.SaveChanges();

                //assert
                var medicalTeamCreated = mockHelper.ServicesProvider
                    .GetQueriesService<IMedicalTeamQueriesService>().Get( medicalTeam.Id );

                Assert.True( medicalTeamCreated.Nurses.Count == 5 );
            }
        }
    }
}
