using Proact.Services.Entities;
using Proact.Services.Models;
using Proact.Services.QueriesServices;
using Proact.Services.ServicesProviders;
using System;
using Xunit;

namespace Proact.Services.UnitTests.Patients {
    public class Queries_PatientCreation_UnitTests {

        private void ExecuteTestAsserts( Patient originalPatient, Patient retrievedPatient ) {
            Assert.NotNull( originalPatient );
            Assert.NotNull( retrievedPatient );
            Assert.Equal( originalPatient.User.Id, retrievedPatient.User.Id );
            Assert.Equal( originalPatient.BirthYear, retrievedPatient.BirthYear );
            Assert.Equal( originalPatient.ECode, retrievedPatient.ECode );
            Assert.Equal( originalPatient.TreatmentStartDate, retrievedPatient.TreatmentStartDate );
            Assert.Equal( originalPatient.Gender, retrievedPatient.Gender );
        }

        [Fact]
        public void PatientCreationConsistencyTest() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                //arrange
                var user = mockHelper.CreateDummyUser();

                var patientCreationRequest = new PatientCreateRequest() {
                    BirthYear = 1980,
                    Gender = "M",
                    TreatmentStartDate = DateTime.Now
                };

                //act
                var patient = mockHelper.ServicesProvider
                    .GetQueriesService<IPatientQueriesService>()
                    .Create( user, patientCreationRequest );

                mockHelper.ServicesProvider.SaveChanges();

                //assert
                var createdPatient = mockHelper.ServicesProvider
                    .GetQueriesService<IPatientQueriesService>().Get( user.Id );

                ExecuteTestAsserts( patient, createdPatient );
            }
        }

        [Fact]
        public void PatientCreationConsistencyWithRandomValuesTest() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                for ( int i = 0; i < 10; ++i ) {
                    //arrange
                    var user = mockHelper.CreateDummyUser();
                    var patient = mockHelper.CreateDummyPatient( user );

                    mockHelper.ServicesProvider.SaveChanges();

                    //act
                    var createdPatient = mockHelper.ServicesProvider
                        .GetQueriesService<IPatientQueriesService>().Get( user.Id );

                    //assert
                    ExecuteTestAsserts( patient, createdPatient );
                }
            }
        }
    }
}
