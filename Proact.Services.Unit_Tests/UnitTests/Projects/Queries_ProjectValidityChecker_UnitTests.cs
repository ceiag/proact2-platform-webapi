using Proact.Services.Models;
using Proact.Services.QueriesServices;
using Proact.Services.ServicesProviders;
using Xunit;

namespace Proact.Services.UnitTests.Projects {
    public class Queries_ProjectValidityChecker_UnitTests {
        [Fact]
        public void IsProjectNameAvailableMustReturnTrue() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                //arrange
                //act
                //assert
                bool isProjectAvailable = mockHelper.ServicesProvider
                    .GetQueriesService<IProjectQueriesService>()
                    .IsProjectNameAvailable( "ProjectTest" );

                Assert.True( isProjectAvailable );
            }
        }

        [Fact]
        public void IsProjectNameAvailableMustReturnFalse() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                //arrange
                var projectCreationRequest = new ProjectCreateRequest() {
                    Name = "ProjectTest",
                    Description = "Description",
                    SponsorName = "Sponsor"
                };

                //act
                mockHelper.ServicesProvider
                    .GetQueriesService<IProjectQueriesService>().Create( projectCreationRequest );

                mockHelper.ServicesProvider.SaveChanges();

                //assert
                bool isProjectAvailable = mockHelper.ServicesProvider
                    .GetQueriesService<IProjectQueriesService>()
                    .IsProjectNameAvailable( projectCreationRequest.Name );

                Assert.False( isProjectAvailable );
            }
        }

        [Fact]
        public void IsProjectOpenMustReturnTrue() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                //arrange
                var project = mockHelper.CreateDummyProject();

                //act
                mockHelper.ServicesProvider.SaveChanges();

                var createdProject = mockHelper.ServicesProvider
                    .GetQueriesService<IProjectQueriesService>().Get( project.Id );

                //assert
                bool isProjectAvailable = mockHelper.ServicesProvider
                    .GetQueriesService<IProjectQueriesService>().IsOpened( project.Id );

                Assert.True( isProjectAvailable );
            }
        }

        [Fact]
        public void IsProjectOpenMustReturnFalse() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                //arrange
                var project = mockHelper.CreateDummyProject();
                project.State = Entities.ProjectState.Closed;

                //act
                mockHelper.ServicesProvider.SaveChanges();

                var createdProject = mockHelper.ServicesProvider
                    .GetQueriesService<IProjectQueriesService>().Get( project.Id );

                //assert
                bool isProjectAvailable = mockHelper.ServicesProvider
                    .GetQueriesService<IProjectQueriesService>().IsOpened( project.Id );

                Assert.False( isProjectAvailable );
            }
        }
    }
}
