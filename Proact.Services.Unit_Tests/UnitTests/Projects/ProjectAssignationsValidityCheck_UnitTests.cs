using Proact.Services.QueriesServices;
using Proact.Services.ServicesProviders;
using Xunit;

namespace Proact.Services.UnitTests.Projects {
    public class ProjectAssignationsValidityCheck_UnitTests {
        [Fact]
        public void ProjectAssignAdminConsistencyTest() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                //arrange
                var project = mockHelper.CreateDummyProject();
                var user = mockHelper.CreateDummyUser();
                var medic = mockHelper.CreateDummyMedic( user );

                mockHelper.ServicesProvider.SaveChanges();

                //act
                mockHelper.ServicesProvider
                    .GetQueriesService<IProjectQueriesService>()
                    .AssignAdmin( project.Id, medic );

                mockHelper.ServicesProvider.SaveChanges();

                //assert
                var createdProject = mockHelper.ServicesProvider
                    .GetQueriesService<IProjectQueriesService>().Get( project.Id );

                Assert.NotNull( createdProject );
                Assert.Equal( user.Id, createdProject.Admin.Id );
            }
        }

        [Fact]
        public void ProjectAssignToPatientTest() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                //arrange
                var project = mockHelper.CreateDummyProject();
                var userMedic = mockHelper.CreateDummyUser();
                var medic = mockHelper.CreateDummyMedic( userMedic );
                var medicalTeam = mockHelper.CreateDummyMedicalTeam( project );

                var userPatient = mockHelper.CreateDummyUser();
                var patient = mockHelper.CreateDummyPatient( userPatient );
                
                mockHelper.ServicesProvider.SaveChanges();

                //act
                mockHelper.ServicesProvider
                    .GetQueriesService<IPatientQueriesService>()
                    .AddToMedicalTeam( patient.UserId, medicalTeam.Id );

                mockHelper.ServicesProvider.SaveChanges();

                //assert
                var associatedProjects = mockHelper.ServicesProvider
                    .GetQueriesService<IProjectQueriesService>()
                    .GetProjectsWhereUserIsAssociated( patient.UserId );

                Assert.NotNull( associatedProjects );
                Assert.Single( associatedProjects );
            }
        }
    }
}
