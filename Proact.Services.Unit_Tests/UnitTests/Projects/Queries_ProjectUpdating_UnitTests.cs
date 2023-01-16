using Proact.Services.Models;
using Proact.Services.QueriesServices;
using Proact.Services.ServicesProviders;
using Xunit;

namespace Proact.Services.UnitTests.Projects {
    public class Queries_ProjectUpdating_UnitTests {
        [Fact]
        public void ProjectUpdatingConsistencyTestWithRandomValues() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                for ( int i = 0; i < 10; ++i ) {
                    //arrange
                    var project = mockHelper.CreateDummyProject();

                    mockHelper.ServicesProvider.SaveChanges();

                    var projectUpdateRequest = new ProjectUpdateRequest() {
                        Description = mockHelper.GenerateRandomName(),
                        Name = mockHelper.GenerateRandomName(),
                        SponsorName = mockHelper.GenerateRandomName()
                    };

                    mockHelper.ServicesProvider
                        .GetQueriesService<IProjectQueriesService>()
                        .Update( project.Id, projectUpdateRequest );

                    mockHelper.ServicesProvider.SaveChanges();

                    //assert
                    var projectCreated = mockHelper.ServicesProvider
                        .GetQueriesService<IProjectQueriesService>().Get( project.Id );

                    ProjectAssert.Equal( project, projectCreated );
                }
            }
        }
    }
}
