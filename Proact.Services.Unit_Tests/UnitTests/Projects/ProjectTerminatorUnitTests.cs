using Proact.Services.Entities;
using Proact.Services.QueriesServices;
using Xunit;

namespace Proact.Services.UnitTests.Projects {
    public class ProjectTerminatorUnitTests {
        [Fact]
        public void ProjectAndMedicalTeamClosingConsistency() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var project = mockHelper.CreateDummyProject();
                var medicalTeam_0 = mockHelper.CreateDummyMedicalTeam( project );
                var medicalTeam_1 = mockHelper.CreateDummyMedicalTeam( project );

                mockHelper.ServicesProvider.SaveChanges();

                mockHelper.ServicesProvider
                    .GetEditorsService<IProjectStateEditorService>().CloseProject( project.Id );

                mockHelper.ServicesProvider.SaveChanges();

                var projectRetrieved = mockHelper.ServicesProvider
                    .GetQueriesService<IProjectQueriesService>().Get( project.Id );

                Assert.NotNull( projectRetrieved );
                Assert.Equal( ProjectState.Closed, projectRetrieved.State );
                Assert.Equal( MedicalTeamState.ClosedByProject, projectRetrieved.MedicalTeams[0].State );
                Assert.Equal( MedicalTeamState.ClosedByProject, projectRetrieved.MedicalTeams[1].State );
            }
        }
    }
}
