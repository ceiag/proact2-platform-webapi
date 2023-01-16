using Proact.Services.Entities;
using Proact.Services.Models;
using Xunit;

namespace Proact.Comparators {
    public static class ProjectEqual {
        public static void AssertEqual( Project expected, ProjectModel current ) {
            if ( expected == null || current == null ) {
                Assert.Null( expected );
                Assert.Null( current );
            }
            else {
                Assert.Equal( expected.Id, current.ProjectId );
                Assert.Equal( expected.Description, current.Description );
                Assert.Equal( expected.Name, current.Name );
                Assert.Equal( expected.SponsorName, current.SponsorName );
                Assert.Equal( expected.State, current.Status );
            }
        }
    }
}
