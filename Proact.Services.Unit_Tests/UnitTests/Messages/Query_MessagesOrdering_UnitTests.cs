using Proact.Services.QueriesServices;
using Proact.Services.ServicesProviders;
using Xunit;

namespace Proact.Services.UnitTests.Messages {
    public class Query_MessagesOrdering_UnitTests {
        [Fact]
        public void MessagesMustBeOrderedByLastReply() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                //arrange
                var user_patient = mockHelper.CreateDummyUser();
                var user_medic = mockHelper.CreateDummyUser();
                var patient = mockHelper.CreateDummyPatient( user_patient );
                var medic = mockHelper.CreateDummyMedic( user_medic );
                var project = mockHelper.CreateDummyProject();
                var medicalTeam = mockHelper.CreateDummyMedicalTeam( project );

                mockHelper.ServicesProvider
                    .GetQueriesService<IPatientQueriesService>()
                    .AddToMedicalTeam( patient.UserId, medicalTeam.Id );

                //act
                var messageWithReplies = mockHelper
                    .CreateDummyNewTopicMessage( user_patient, medicalTeam );

                var messageWithoutReplies = mockHelper
                    .CreateDummyNewTopicMessage( user_patient, medicalTeam );

                var replyToMessage_0 = mockHelper
                    .CreateDummyReplyToMessage( user_medic, medicalTeam, messageWithReplies );
                var replyToMessage_1 = mockHelper
                    .CreateDummyReplyToMessage( user_patient, medicalTeam, messageWithReplies );

                var messageWithoutReplies_1 = mockHelper
                    .CreateDummyNewTopicMessage( user_patient, medicalTeam );

                //assert
                var messagesListForPatient = mockHelper
                    .ServicesProvider.GetEditorsService<IMessageFormatterService>()
                    .GetMessagesAsPatient( patient, 0, 100 );

                Assert.Equal(
                    messagesListForPatient[0].OriginalMessage.MessageId, messageWithoutReplies_1.MessageId );
                Assert.Equal(
                    messagesListForPatient[1].OriginalMessage.MessageId, messageWithReplies.MessageId );
                Assert.Equal(
                    messagesListForPatient[2].OriginalMessage.MessageId, messageWithoutReplies.MessageId );

                var messagesListForMedic = mockHelper
                    .ServicesProvider.GetEditorsService<IMessageFormatterService>()
                    .GetMessagesAsMedic( medicalTeam, 0, 100 );

                Assert.Equal(
                    messagesListForMedic[0].OriginalMessage.MessageId, messageWithoutReplies_1.MessageId );
                Assert.Equal(
                    messagesListForMedic[1].OriginalMessage.MessageId, messageWithReplies.MessageId );
                Assert.Equal(
                    messagesListForMedic[2].OriginalMessage.MessageId, messageWithoutReplies.MessageId );
            }
        }
    }
}
