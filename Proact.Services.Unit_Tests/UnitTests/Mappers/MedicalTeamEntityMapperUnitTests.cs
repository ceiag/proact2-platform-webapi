using Proact.Comparators;
using Proact.Services;
using Proact.Services.Entities;
using Proact.Services.UnitTests;
using Xunit;

namespace Proact.EntityMappers.UnitTests {
    public class MedicalTeamEntityMapperUnitTests {
        [Fact]
        public void MapFromMedicalTeamEntityToModel() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var project = mockHelper.CreateDummyProject();
                var medicalTeam = mockHelper.CreateDummyMedicalTeam( project );

                var medicalTeamModel = MedicalTeamEntityMapper.Map( medicalTeam );

                MedicalTeamEqual.AssertEqual( medicalTeam, medicalTeamModel );
            }
        }

        [Fact]
        public void MapFromMedicalTeamEntityNullToModel() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                MedicalTeam medicalTeam = null;

                var medicalTeamModel = MedicalTeamEntityMapper.Map( medicalTeam );

                MedicalTeamEqual.AssertEqual( medicalTeam, medicalTeamModel );
            }
        }
    }
}
