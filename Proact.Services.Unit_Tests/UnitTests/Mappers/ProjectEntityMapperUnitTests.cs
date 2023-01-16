using Proact.Comparators;
using Proact.Services;
using Proact.Services.Entities;
using Proact.Services.UnitTests;
using Xunit;

namespace Proact.EntityMappers.UnitTests {
    public class ProjectEntityMapperUnitTests {
        [Fact]
        public void MapFromProjectEntityToModel() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var project = mockHelper.CreateDummyProject();
                var projectModel = ProjectEntityMapper.Map( project );

                ProjectEqual.AssertEqual( project, projectModel );
            }
        }

        [Fact]
        public void MapFromProjectEntityNullToModel() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                Project project = null;

                var projectModel = ProjectEntityMapper.Map( project );

                ProjectEqual.AssertEqual( project, projectModel );
            }
        }
    }
}
