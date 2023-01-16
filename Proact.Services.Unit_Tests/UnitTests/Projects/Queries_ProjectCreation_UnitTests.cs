using Proact.Services.QueriesServices;
using Proact.Services.ServicesProviders;
using Xunit;

namespace Proact.Services.UnitTests.Projects {
    public class Queries_ProjectCreation_UnitTests {
        [Fact]
        public void ProjectCreationConsistencyTestWithRandomValues() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                for ( int i = 0; i < 10; ++i ) {
                    //arrange
                    var project = mockHelper.CreateDummyProject();

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
