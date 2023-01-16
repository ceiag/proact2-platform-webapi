using Proact.Comparators;
using Proact.Services;
using Proact.Services.Entities;
using Proact.Services.UnitTests;
using Xunit;

namespace Proact.EntityMappers.UnitTests {
    public class PatientEntityMapperUnitTests {
        [Fact]
        public void MapFromProjectEntityToModel() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var user = mockHelper.CreateDummyUser();
                var patient = mockHelper.CreateDummyPatient( user );

                var patientModel = PatientEntityMapper.Map( patient );

                PatientEqual.AssertEqual( patient, patientModel );
            }
        }

        [Fact]
        public void MapFromProjectEntityNullToModel() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                Patient patient = null;

                var patientModel = PatientEntityMapper.Map( patient );

                PatientEqual.AssertEqual( patient, patientModel );
            }
        }
    }
}
