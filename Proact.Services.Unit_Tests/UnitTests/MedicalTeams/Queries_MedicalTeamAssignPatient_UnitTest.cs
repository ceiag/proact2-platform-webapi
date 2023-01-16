using Proact.Services.Entities;
using Proact.Services.QueriesServices;
using Proact.Services.ServicesProviders;
using System.Collections.Generic;
using Xunit;

namespace Proact.Services.UnitTests.MedicalTeams {
    public class Queries_MedicalTeamAssignPatient_UnitTest {
        [Fact]
        public void MedicalTeamAssignPatientConsistencyTest() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                //arrange
                var project = mockHelper.CreateDummyProject();
                var medicalTeam = mockHelper.CreateDummyMedicalTeam( project );

                for ( int i = 0; i < 10; ++i ) {
                    var user = mockHelper.CreateDummyUser();
                    var patient = mockHelper.CreateDummyPatient( user );

                    //act
                    mockHelper.ServicesProvider
                        .GetQueriesService<IPatientQueriesService>()
                        .AddToMedicalTeam( patient.UserId, medicalTeam.Id );
                }
                
                mockHelper.ServicesProvider.SaveChanges();

                //assert
                var medicalTeamCreated = mockHelper.ServicesProvider
                    .GetQueriesService<IMedicalTeamQueriesService>().Get( medicalTeam.Id );

                Assert.True( medicalTeamCreated.Patients.Count == 10 );
            }
        }

        [Fact]
        public void MedicalTeamRemovePatientConsistencyTest() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                //arrange
                var project = mockHelper.CreateDummyProject();
                var medicalTeam = mockHelper.CreateDummyMedicalTeam( project );

                var patientsAssigned = new List<Patient>();

                //act
                for ( int i = 0; i < 10; ++i ) {
                    var user = mockHelper.CreateDummyUser();
                    var patient = mockHelper.CreateDummyPatient( user );

                    mockHelper.ServicesProvider
                        .GetQueriesService<IPatientQueriesService>()
                        .AddToMedicalTeam( patient.UserId, medicalTeam.Id );

                    patientsAssigned.Add( patient );
                }

                mockHelper.ServicesProvider.SaveChanges();

                for ( int i = 0; i < 5; ++i ) {
                    mockHelper.ServicesProvider
                        .GetQueriesService<IPatientQueriesService>()
                        .RemoveFromMedicalTeam( patientsAssigned[i].UserId );
                }

                mockHelper.ServicesProvider.SaveChanges();

                //assert
                var medicalTeamCreated = mockHelper.ServicesProvider
                    .GetQueriesService<IMedicalTeamQueriesService>()
                    .Get( medicalTeam.Id );

                Assert.True( medicalTeamCreated.Patients.Count == 5 );
            }
        }
    }
}
