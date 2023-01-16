using Proact.Services.Models;
using Proact.Services.QueriesServices;
using Proact.Services.ServicesProviders;
using System.Collections.Generic;
using Xunit;

namespace Proact.Services.UnitTests.Messages {
    public class Queries_MessagesCreation_UnitTests {
        private void Equal( MessageModel original, MessageModel created ) {
            Assert.NotNull( original );
            Assert.NotNull( created );
            Assert.Equal( original.AuthorName, created.AuthorName );
            Assert.Equal( original.MessageId, created.MessageId );
            Assert.Equal( original.MessageType, created.MessageType );
            Assert.Equal( original.Title, created.Title );
            Assert.Equal( original.Body, created.Body );
            Assert.Equal( original.Emotion, created.Emotion );
            Assert.Equal( original.OriginalMessageId, created.OriginalMessageId );
            Assert.Equal( original.RecordedTime, created.RecordedTime );
        }

        [Fact]
        public void GetMessagesCheckConsistencyTest() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                //arrange
                var user_patient = mockHelper.CreateDummyUser();
                var user_medic = mockHelper.CreateDummyUser();
                var user_nurse = mockHelper.CreateDummyUser();

                var patient = mockHelper.CreateDummyPatient( user_patient );
                var medic = mockHelper.CreateDummyMedic( user_medic );
                var nurse = mockHelper.CreateDummyNurse( user_nurse );

                var project = mockHelper.CreateDummyProject();
                var medicalTeam = mockHelper.CreateDummyMedicalTeam( project );

                mockHelper.ServicesProvider
                    .GetQueriesService<IPatientQueriesService>()
                    .AddToMedicalTeam( patient.UserId, medicalTeam.Id );

                //act
                var messageWithReplies = mockHelper
                    .CreateDummyNewTopicMessage( user_patient, medicalTeam );

                var replyToMessage_0 = mockHelper
                    .CreateDummyReplyToMessage( user_medic, medicalTeam, messageWithReplies );
                var replyToMessage_1 = mockHelper
                    .CreateDummyReplyToMessage( user_patient, medicalTeam, messageWithReplies );

                var broadcastMessage = mockHelper.CreateDummyBroadcastMessage( user_medic, medicalTeam );

                //assert
                var createdMessageWithReplies = mockHelper
                    .ServicesProvider.GetEditorsService<IMessageFormatterService>()
                    .GetMessageAsMedic( messageWithReplies.MessageId, 100 );

                var createdReplyFromMedic = mockHelper
                    .ServicesProvider.GetEditorsService<IMessageFormatterService>()
                    .GetMessageAsMedic( replyToMessage_0.MessageId, 100 );

                var createdReplyFromPatient = mockHelper
                    .ServicesProvider.GetEditorsService<IMessageFormatterService>()
                    .GetMessageAsMedic( replyToMessage_1.MessageId, 100 );

                var messagesListForPatient = mockHelper
                    .ServicesProvider.GetEditorsService<IMessageFormatterService>()
                    .GetMessagesAsPatient( patient, 0, 100 );

                var messagesListForMedic = mockHelper
                    .ServicesProvider.GetEditorsService<IMessageFormatterService>()
                    .GetMessagesAsMedic( medicalTeam, 0, 100 );

                var createdbroadcastMessage = mockHelper
                    .ServicesProvider.GetEditorsService<IMessageFormatterService>()
                    .GetMessageAsMedic( broadcastMessage.MessageId, 100 );

                Equal( messageWithReplies, createdMessageWithReplies.OriginalMessage );
                Equal( replyToMessage_0, createdReplyFromMedic.OriginalMessage );
                Equal( replyToMessage_1, createdReplyFromPatient.OriginalMessage );
                Equal( broadcastMessage, createdbroadcastMessage.OriginalMessage );
                Assert.Equal( 2, createdMessageWithReplies.ReplyMessagesCount );
                Assert.Equal( 2, messagesListForPatient.Count );
                Assert.Equal( 2, messagesListForMedic.Count );
            }
        }

        [Fact]
        public void PatientsShouldNotReadOtherPatientsMessages() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var user_patient_0 = mockHelper.CreateDummyUser();
                var patient_0 = mockHelper.CreateDummyPatient( user_patient_0 );

                var user_patient_1 = mockHelper.CreateDummyUser();
                var patient_1 = mockHelper.CreateDummyPatient( user_patient_1 );

                var user_medic = mockHelper.CreateDummyUser();
                var medic = mockHelper.CreateDummyMedic( user_medic );

                var project = mockHelper.CreateDummyProject();
                var medicalTeam = mockHelper.CreateDummyMedicalTeam( project );

                var message_from_patient_0 = mockHelper
                    .CreateDummyNewTopicMessage( user_patient_0, medicalTeam );

                var message_from_patient_1 = mockHelper
                    .CreateDummyNewTopicMessage( user_patient_1, medicalTeam );

                var messagesListForPatient0 = mockHelper
                    .ServicesProvider.GetEditorsService<IMessageFormatterService>()
                    .GetMessagesAsPatient( patient_0, 0, 100 );

                var messagesListForPatient1 = mockHelper
                    .ServicesProvider.GetEditorsService<IMessageFormatterService>()
                    .GetMessagesAsPatient( patient_1, 0, 100 );

                Assert.Single( messagesListForPatient0 );
                Assert.Single( messagesListForPatient1 );
                Equal( message_from_patient_0, messagesListForPatient0[0].OriginalMessage );
                Equal( message_from_patient_1, messagesListForPatient1[0].OriginalMessage );
            }
        }

        [Fact]
        public void GetUnreadMessagesForMedicalTeam() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                //arrange
                var user_patient = mockHelper.CreateDummyUser();
                var user_medic = mockHelper.CreateDummyUser();

                var patient = mockHelper.CreateDummyPatient( user_patient );
                var medic = mockHelper.CreateDummyMedic( user_medic );

                var project = mockHelper.CreateDummyProject();
                var medicalTeam = mockHelper.CreateDummyMedicalTeam( project );

                mockHelper.ServicesProvider
                    .GetQueriesService<IMedicQueriesService>()
                    .AddToMedicalTeam( medic.UserId, medicalTeam.Id );

                //act
                var messagesCreated = new List<MessageModel>();
                for ( int i = 0; i < 5; ++i ) {
                    var messageWithoutReplies = mockHelper
                        .CreateDummyNewTopicMessage( user_patient, medicalTeam );

                    var createdMessageWithoutReplies = mockHelper
                    .ServicesProvider.GetEditorsService<IMessageFormatterService>()
                    .GetMessageAsMedic( messageWithoutReplies.MessageId, 100 );

                    messagesCreated.Add( messageWithoutReplies );

                    Equal( messageWithoutReplies, createdMessageWithoutReplies.OriginalMessage );
                }

                mockHelper.CreateDummyReplyToMessage( medic, medicalTeam, messagesCreated[0] );
                mockHelper.CreateDummyReplyToMessage( medic, medicalTeam, messagesCreated[1] );
                mockHelper.CreateDummyReplyToMessage( patient, medicalTeam, messagesCreated[2] );
                mockHelper.CreateDummyReplyToMessage( patient, medicalTeam, messagesCreated[3] );

                var messagesWithoutReplies = mockHelper
                    .ServicesProvider.GetEditorsService<IMessageFormatterService>()
                    .GetMessagesAsMedicUnreplied( medicalTeam.Id, 0, 100 );

                Assert.True( messagesWithoutReplies.Count == 3 );
            }
        }

        [Fact]
        public void GetMessagesBroadcast() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                //arrange
                var user_medic = mockHelper.CreateDummyUser();
                var medic = mockHelper.CreateDummyMedic( user_medic );
                var user_patient = mockHelper.CreateDummyUser();
                var patient = mockHelper.CreateDummyPatient( user_patient );

                var project = mockHelper.CreateDummyProject();
                var medicalTeam = mockHelper.CreateDummyMedicalTeam( project );

                mockHelper.ServicesProvider
                    .GetQueriesService<IMedicQueriesService>()
                    .AddToMedicalTeam( medic.UserId, medicalTeam.Id );

                mockHelper.ServicesProvider
                    .GetQueriesService<IPatientQueriesService>()
                    .AddToMedicalTeam( patient.UserId, medicalTeam.Id );

                mockHelper.ServicesProvider.SaveChanges();

                //act
                for ( int i = 0; i < 5; ++i ) {
                    var message = mockHelper
                        .CreateDummyBroadcastMessage( user_medic, medicalTeam );

                    var createdMessage = mockHelper
                        .ServicesProvider.GetEditorsService<IMessageFormatterService>()
                        .GetMessageAsMedic( message.MessageId, 100 );

                    Equal( message, createdMessage.OriginalMessage );
                }

                var broadcastMessages = mockHelper
                    .ServicesProvider.GetEditorsService<IMessageFormatterService>()
                    .GetMessagesAsMedic( medicalTeam, 0, 100 );

                Assert.True( broadcastMessages.Count == 5 );
            }
        }

        [Fact]
        public void BroadcastMessageMustBeVisibleOnlyInOneMedicalTeam() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                //arrange
                var user_medic_0 = mockHelper.CreateDummyUser();
                var user_medic_1 = mockHelper.CreateDummyUser();
                var user_patient_0 = mockHelper.CreateDummyUser();
                var user_patient_1 = mockHelper.CreateDummyUser();

                var medic_0 = mockHelper.CreateDummyMedic( user_medic_0 );
                var medic_1 = mockHelper.CreateDummyMedic( user_medic_1 );
                var patient_0 = mockHelper.CreateDummyPatient( user_patient_0 );
                var patient_1 = mockHelper.CreateDummyPatient( user_patient_1 );

                var project_0 = mockHelper.CreateDummyProject();
                var project_1 = mockHelper.CreateDummyProject();

                var medicalTeam_0 = mockHelper.CreateDummyMedicalTeam( project_0 );
                var medicalTeam_1 = mockHelper.CreateDummyMedicalTeam( project_1 );

                mockHelper.ServicesProvider
                    .GetQueriesService<IMedicQueriesService>()
                    .AddToMedicalTeam( medic_0.UserId, medicalTeam_0.Id );

                mockHelper.ServicesProvider
                    .GetQueriesService<IMedicQueriesService>()
                    .AddToMedicalTeam( medic_1.UserId, medicalTeam_1.Id );

                mockHelper.ServicesProvider
                     .GetQueriesService<IPatientQueriesService>()
                     .AddToMedicalTeam( patient_0.UserId, medicalTeam_0.Id );

                mockHelper.ServicesProvider
                    .GetQueriesService<IPatientQueriesService>()
                    .AddToMedicalTeam( patient_1.UserId, medicalTeam_1.Id );

                mockHelper.ServicesProvider.SaveChanges();

                //act
                var message_0 = mockHelper.CreateDummyBroadcastMessage( user_medic_0, medicalTeam_0 );
                var message_1 = mockHelper.CreateDummyBroadcastMessage( user_medic_0, medicalTeam_1 );
                var message_2 = mockHelper.CreateDummyNewTopicMessage( user_patient_0, medicalTeam_0 );
                var message_3 = mockHelper.CreateDummyNewTopicMessage( user_patient_0, medicalTeam_0 );
                var message_4 = mockHelper.CreateDummyNewTopicMessage( user_patient_1, medicalTeam_1 );

                var messagesFromMedicalTeam_0 = mockHelper
                    .ServicesProvider.GetEditorsService<IMessageFormatterService>()
                    .GetMessagesAsPatient( patient_0, 0, 100 );

                var messagesFromMedicalTeam_1 = mockHelper
                    .ServicesProvider.GetEditorsService<IMessageFormatterService>()
                    .GetMessagesAsPatient( patient_1, 0, 100 );

                Assert.Equal( 3, messagesFromMedicalTeam_0.Count );
                Assert.Equal( 2, messagesFromMedicalTeam_1.Count );
            }
        }

        [Fact]
        public void PatientMessagesMustBeVisibleOnlyToPatientHimSelf() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                //arrange
                var user_medic = mockHelper.CreateDummyUser();
                var user_patient_0 = mockHelper.CreateDummyUser();
                var user_patient_1 = mockHelper.CreateDummyUser();

                var medic = mockHelper.CreateDummyMedic( user_medic );
                var patient_0 = mockHelper.CreateDummyPatient( user_patient_0 );
                var patient_1 = mockHelper.CreateDummyPatient( user_patient_1 );

                var project_0 = mockHelper.CreateDummyProject();
                var project_1 = mockHelper.CreateDummyProject();

                var medicalTeam_0 = mockHelper.CreateDummyMedicalTeam( project_0 );

                mockHelper.ServicesProvider
                    .GetQueriesService<IPatientQueriesService>()
                    .AddToMedicalTeam( patient_0.UserId, medicalTeam_0.Id );

                mockHelper.ServicesProvider
                     .GetQueriesService<IPatientQueriesService>()
                     .AddToMedicalTeam( patient_1.UserId, medicalTeam_0.Id );

                mockHelper.ServicesProvider.SaveChanges();

                //act
                var message_0 = mockHelper.CreateDummyBroadcastMessage( user_medic, medicalTeam_0 );
                var message_1 = mockHelper.CreateDummyBroadcastMessage( user_medic, medicalTeam_0 );
                var message_2 = mockHelper.CreateDummyNewTopicMessage( user_patient_0, medicalTeam_0 );
                var message_3 = mockHelper.CreateDummyNewTopicMessage( user_patient_0, medicalTeam_0 );
                var message_4 = mockHelper.CreateDummyNewTopicMessage( user_patient_1, medicalTeam_0 );

                var messagesFromMedicalTeam_0 = mockHelper
                    .ServicesProvider.GetEditorsService<IMessageFormatterService>()
                    .GetMessagesAsPatient( patient_0, 0, 100 );

                var messagesFromMedicalTeam_1 = mockHelper
                    .ServicesProvider.GetEditorsService<IMessageFormatterService>()
                    .GetMessagesAsPatient( patient_1, 0, 100 );

                Assert.Equal( 4, messagesFromMedicalTeam_0.Count );
                Assert.Equal( 3, messagesFromMedicalTeam_1.Count );
            }
        }
    }
}