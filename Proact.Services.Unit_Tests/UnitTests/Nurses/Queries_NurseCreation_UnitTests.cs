using Proact.Services.Entities;
using Proact.Services.EntitiesMapper;
using Proact.Services.QueriesServices;
using Proact.Services.ServicesProviders;
using Xunit;

namespace Proact.Services.UnitTests.Nurses {
    public class Queries_NurseCreation_UnitTests {
        private void ExecuteTestAsserts( Nurse original, Nurse created ) {
            Assert.NotNull( original );
            Assert.NotNull( created );
            Assert.Equal( original.User.Id, created.User.Id );
            Assert.Equal( original.Id, created.Id );
        }

        [Fact]
        public void NurseCreationConsistencyWithRandomValuesTest() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                for ( int i = 0; i < 10; ++i ) {
                    //arrange
                    var user = mockHelper.CreateDummyUser();
                    var nurse = mockHelper.CreateDummyNurse( user );

                    //act
                    var nurseCreated = mockHelper.ServicesProvider
                        .GetQueriesService<INurseQueriesService>().Get( user.Id );

                    //assert
                    ExecuteTestAsserts( nurse, nurseCreated );
                }
            }
        }

        [Fact]
        public void NurseCreationWithMedicalTeamAdding() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                //arrange
                var user = mockHelper.CreateDummyUser();
                var nurse = mockHelper.CreateDummyNurse( user );
                var project_0 = mockHelper.CreateDummyProject();
                var project_1 = mockHelper.CreateDummyProject();
                var medicalTeam_0 = mockHelper.CreateDummyMedicalTeam( project_0 );
                var medicalTeam_1 = mockHelper.CreateDummyMedicalTeam( project_0 );
                var medicalTeam_2 = mockHelper.CreateDummyMedicalTeam( project_1 );

                //act
                mockHelper.ServicesProvider.GetQueriesService<INurseQueriesService>()
                    .AddToMedicalTeam( user.Id, medicalTeam_0.Id );
                mockHelper.ServicesProvider.GetQueriesService<INurseQueriesService>()
                    .AddToMedicalTeam( user.Id, medicalTeam_1.Id );
                mockHelper.ServicesProvider.GetQueriesService<INurseQueriesService>()
                    .AddToMedicalTeam( user.Id, medicalTeam_2.Id );

                var nurseCreated = mockHelper.ServicesProvider
                    .GetQueriesService<INurseQueriesService>().Get( user.Id );

                var medicCreatedModel = NurseEntityMapper.Map( nurseCreated );

                //assert
                Assert.Equal( 3, medicCreatedModel.MedicalTeams.Count );
                Assert.Equal( medicalTeam_0.Id, medicCreatedModel.MedicalTeams[0].MedicalTeamId );
                Assert.Equal( medicalTeam_1.Id, medicCreatedModel.MedicalTeams[1].MedicalTeamId );
                Assert.Equal( medicalTeam_2.Id, medicCreatedModel.MedicalTeams[2].MedicalTeamId );
                Assert.Equal( project_0.Id, medicCreatedModel.MedicalTeams[0].Project.ProjectId );
                Assert.Equal( project_0.Id, medicCreatedModel.MedicalTeams[1].Project.ProjectId );
                Assert.Equal( project_1.Id, medicCreatedModel.MedicalTeams[2].Project.ProjectId );
            }
        }
    }
}
