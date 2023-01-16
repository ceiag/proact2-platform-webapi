using Proact.Services.Entities;
using Proact.Services.QueriesServices;
using Proact.Services.ServicesProviders;
using System.Linq;
using Xunit;

namespace Proact.Services.UnitTests.MedicalTeams {
    public class Queries_MedicalTeamCreation_UnitTests {
        private void Equal( MedicalTeam original, MedicalTeam created ) {
            Assert.NotNull( original );
            Assert.NotNull( created );
            Assert.NotNull( original.Project );
            Assert.NotNull( created.Project );
            Assert.Equal( original.Project.Id, created.Project.Id );
            Assert.Equal( original.ProjectId, created.ProjectId );
            Assert.Equal( original.Name, created.Name );
            Assert.Equal( original.Phone, created.Phone );
            Assert.Equal( original.AddressLine1, created.AddressLine1 );
            Assert.Equal( original.AddressLine2, created.AddressLine2 );
            Assert.Equal( original.City, created.City );
            Assert.Equal( original.Country, created.Country );
            Assert.Equal( original.PostalCode, created.PostalCode );
            Assert.Equal( original.TimeZone, created.TimeZone );
            Assert.Equal( original.RegionCode, created.RegionCode );
            Assert.Equal( original.StateOrProvince, created.StateOrProvince );
            Assert.Equal( original.Enabled, created.Enabled );
        }

        [Fact]
        public void MedicalTeamCreationConsistencyTest() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                for ( int i = 0; i < 10; ++i ) {
                    //arrange
                    var project = mockHelper.CreateDummyProject();
                    mockHelper.ServicesProvider.SaveChanges();

                    var medicalTeam = mockHelper.CreateDummyMedicalTeam( project );
                    mockHelper.ServicesProvider.SaveChanges();

                    //assert
                    var medicalTeamCreated = mockHelper.ServicesProvider
                        .GetQueriesService<IMedicalTeamQueriesService>().Get( medicalTeam.Id );

                    Equal( medicalTeam, medicalTeamCreated );
                }
            }
        }
    }
}
