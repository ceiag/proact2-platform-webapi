using Proact.Services.Entities;
using Proact.Services.QueriesServices;
using Xunit;

namespace Proact.Services.UnitTests.Medics {
    public class MedicCreationUnitTests {
        private void ExecuteTestAsserts( Medic original, Medic created ) {
            Assert.NotNull( original );
            Assert.NotNull( created );
            Assert.Equal( original.User.Id, created.User.Id );
            Assert.Equal( original.Id, created.Id );
        }

        [Fact]
        public void MedicsCreationWithMedicalTeamAdding() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                //arrange
                var user = mockHelper.CreateDummyUser();
                var medic = mockHelper.CreateDummyMedic( user );
                var project_0 = mockHelper.CreateDummyProject();
                var project_1 = mockHelper.CreateDummyProject();
                var medicalTeam_0 = mockHelper.CreateDummyMedicalTeam( project_0 );
                var medicalTeam_1 = mockHelper.CreateDummyMedicalTeam( project_0 );
                var medicalTeam_2 = mockHelper.CreateDummyMedicalTeam( project_1 );

                //act
                mockHelper.ServicesProvider.GetQueriesService<IMedicQueriesService>()
                    .AddToMedicalTeam( user.Id, medicalTeam_0.Id );
                mockHelper.ServicesProvider.GetQueriesService<IMedicQueriesService>()
                    .AddToMedicalTeam( user.Id, medicalTeam_1.Id );
                mockHelper.ServicesProvider.GetQueriesService<IMedicQueriesService>()
                    .AddToMedicalTeam( user.Id, medicalTeam_2.Id );

                var medicCreated = mockHelper.ServicesProvider
                    .GetQueriesService<IMedicQueriesService>().Get( user.Id );

                var medicCreatedModel = MedicEntityMapper.Map( medicCreated );

                //assert
                ExecuteTestAsserts( medic, medicCreated );
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
