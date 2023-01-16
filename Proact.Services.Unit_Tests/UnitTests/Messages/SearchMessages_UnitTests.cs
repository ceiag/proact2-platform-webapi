using Proact.Services.QueriesServices;
using Proact.Services.ServicesProviders;
using System;
using Xunit;

namespace Proact.Services.UnitTests.Messages {
    public class SearchMessages_UnitTests {
        [Fact]
        public void SearchMessagesByContentAsPatientIntoOriginalMessages() {
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
                var message_0 = mockHelper.CreateNewTopicMessage(
                    user_patient, medicalTeam, "ciccio pasticcio message" );

                var message_1 = mockHelper.CreateNewTopicMessage(
                    user_patient, medicalTeam, "cicciopasticcio" );

                var replyToMessage_0 = mockHelper.CreateDummyReplyToMessage(
                    user_medic, medicalTeam, message_0 );

                //assert
                string url = "?message=ciccio";

                var messagesListForPatient = mockHelper
                    .ServicesProvider.GetEditorsService<IMessageFormatterService>()
                    .SearchMessagesAsPatient( patient, url, 100 );

                Assert.True( messagesListForPatient.Count == 2 );
            }
        }

        [Fact]
        public void SearchMessagesByDateRangeAndContentAsPatient() {
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
                var message_0 = mockHelper.CreateNewTopicMessage(
                    user_patient, medicalTeam, "ciccio pasticcio message" );

                var message_1 = mockHelper.CreateNewTopicMessage(
                    user_patient, medicalTeam, "cicciopasticcio" );

                var replyToMessage_0 = mockHelper.CreateDummyReplyToMessage(
                    user_medic, medicalTeam, message_0 );

                //assert
                string url = $"?fromdate={DateTime.UtcNow}&todate={DateTime.UtcNow.AddDays( 1 )}&message=ciccio";

                var messagesListForPatient = mockHelper
                    .ServicesProvider.GetEditorsService<IMessageFormatterService>()
                    .SearchMessagesAsPatient( patient, url, 100 );

                Assert.True( messagesListForPatient.Count == 2 );
            }
        }

        [Fact]
        public void SearchMessagesByContentAsPatientIntoReplies() {
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
                var message_0 = mockHelper.CreateNewTopicMessage(
                    user_patient, medicalTeam, "ciccio pasticcio message" );

                var message_1 = mockHelper.CreateNewTopicMessage(
                    user_patient, medicalTeam, "cicciopasticcio" );

                var replyToMessage_0 = mockHelper.CreateDummyReplyToMessage(
                    user_medic, medicalTeam, message_0 );

                //assert
                string url = $"?message={replyToMessage_0.Body}";

                var messagesSearchedFromReplies = mockHelper
                    .ServicesProvider.GetEditorsService<IMessageFormatterService>()
                    .SearchMessagesAsPatient( patient, url, 100 );

                Assert.True( messagesSearchedFromReplies.Count == 1 );
            }
        }

        [Fact]
        public void SearchMessagesByContentAsPatientMessageMustReturnZero() {
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

                var user_other = mockHelper.CreateDummyUser();
                var patient_other = mockHelper.CreateDummyPatient( user_other );

                mockHelper.ServicesProvider
                    .GetQueriesService<IPatientQueriesService>()
                    .AddToMedicalTeam( patient.UserId, medicalTeam.Id );

                //act
                var message_0 = mockHelper.CreateNewTopicMessage(
                    user_patient, medicalTeam, "ciccio pasticcio message" );

                var message_1 = mockHelper.CreateNewTopicMessage(
                    user_patient, medicalTeam, "cicciopasticcio" );

                var replyToMessage_0 = mockHelper.CreateDummyReplyToMessage(
                    user_medic, medicalTeam, message_0 );

                //assert
                string url = "?message=ciccio";

                var messagesListForPatient = mockHelper
                    .ServicesProvider.GetEditorsService<IMessageFormatterService>()
                    .SearchMessagesAsPatient( patient_other, url, 100 );

                Assert.True( messagesListForPatient.Count == 0 );
            }
        }

        [Fact]
        public void SearchMessagesByContentAsMedicMustReturnTwo() {
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

                var user_other = mockHelper.CreateDummyUser();
                var patient_other = mockHelper.CreateDummyPatient( user_other );

                mockHelper.ServicesProvider
                    .GetQueriesService<IPatientQueriesService>()
                    .AddToMedicalTeam( patient.UserId, medicalTeam.Id );

                //act
                var message_0 = mockHelper.CreateNewTopicMessage(
                    user_patient, medicalTeam, "ciccio pasticcio message" );

                var message_1 = mockHelper.CreateNewTopicMessage(
                    user_other, medicalTeam, "cicciopasticcio" );

                var replyToMessage_0 = mockHelper.CreateDummyReplyToMessage(
                    user_medic, medicalTeam, message_0 );

                //assert
                string url = "?message=ciccio";

                var messagesListForPatient = mockHelper
                    .ServicesProvider.GetEditorsService<IMessageFormatterService>()
                    .SearchMessagesAsMedic( medicalTeam.Id, url, 100 );

                Assert.True( messagesListForPatient.Count == 2 );
            }
        }
    }
}
