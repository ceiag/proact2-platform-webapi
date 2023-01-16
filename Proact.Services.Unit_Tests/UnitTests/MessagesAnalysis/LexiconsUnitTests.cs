using Proact.Services.QueriesServices;
using Xunit;

namespace Proact.Services.UnitTests.MessageAnalysis {
    public class LexiconsUnitTests {
        [Fact]
        public void LexiconCreationConsistencyTest() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var dummyLexicon = LexiconCreatorHelper.CreateDummyLexicon( mockHelper );

                mockHelper.ServicesProvider.SaveChanges();

                var lexiconRetrieved = mockHelper.ServicesProvider
                    .GetQueriesService<ILexiconQueriesService>()
                    .Get( dummyLexicon.Id );

                Assert.NotNull( lexiconRetrieved );
                Assert.Equal( dummyLexicon.Name, lexiconRetrieved.Name );
                Assert.Equal( dummyLexicon.Description, lexiconRetrieved.Description );
                Assert.Equal( dummyLexicon.Categories.Count, lexiconRetrieved.Categories.Count );

                foreach ( var category in lexiconRetrieved.Categories ) {
                    Assert.Equal( 3, category.Labels.Count );
                }
            }
        }
    }
}
