using Proact.Services.Models;
using Proact.Services.QueriesServices;
using Proact.Services.ServicesProviders;
using System.Collections.Generic;
using Xunit;

namespace Proact.Services.UnitTests.Surveys {
    public class AnswersCreationUnitTests {
        [Fact]
        public void AnswersBlockCreationCheck() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var request = new AnswersBlockCreationRequest() {
                    Labels = new List<string>() {
                        "nothing",
                        "a little bit",
                        "enough",
                        "very",
                        "very very much"
                    }
                };

                var answersBlock = mockHelper.ServicesProvider
                    .GetQueriesService<ISurveyAnswersBlockQueriesService>().Create( request );

                mockHelper.ServicesProvider.SaveChanges();

                Assert.True( answersBlock.Answers.Count == 5 );

                int i = 0;
                foreach ( var answer in answersBlock.Answers ) {
                    Assert.Equal( request.Labels[i], answer.LabelId );
                    ++i;
                }
            }
        }

        [Fact]
        public void AnswersBlocksRetrieveCheck() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var answersBlocks = SurveyCreatorHelper.CreateDummyAnswersBlocks( mockHelper, 5 );

                mockHelper.ServicesProvider.SaveChanges();

                var retrievedAnswersBlocks = mockHelper.ServicesProvider
                    .GetQueriesService<ISurveyAnswersBlockQueriesService>().GetsAll();

                Assert.True( retrievedAnswersBlocks.Count == 5 );

                foreach ( var block in retrievedAnswersBlocks ) {
                    SurveyCreatorHelper.CheckAnswerBlockValidity( block );
                }
            }
        }
    }
}
