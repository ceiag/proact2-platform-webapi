using Proact.Services.Entities;
using Xunit;

namespace Proact.Services.UnitTests.Projects {
    public static class ProjectAssert {
        public static void Equal( Project original, Project created ) {
            Assert.NotNull( original );
            Assert.NotNull( created );
            Assert.Equal( original.Name, created.Name );
            Assert.Equal( original.Description, created.Description );
            Assert.Equal( original.SponsorName, created.SponsorName );
        }
    }
}
