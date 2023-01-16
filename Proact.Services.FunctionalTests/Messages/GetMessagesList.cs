using Microsoft.AspNetCore.Mvc;
using Proact.Services.AuthorizationPolicies;
using Proact.Services.Entities;
using Proact.Services.Models;
using Proact.Services.Tests.Shared;
using System;
using System.Collections.Generic;
using Xunit;

namespace Proact.Services.FunctionalTests.Messages {
    public class GetMessagesList {
        private Institute institute = null;
        private Project project = null;
        private MedicalTeam medicalTeam = null;
        private Patient patient_0 = null;
        private Patient patient_1 = null;
        private Medic medic = null;
        private Nurse nurse = null;
        private MessageModel messageFromPatient_0 = null;
        private MessageModel messageFromPatient_1 = null;
        private MessageModel replyFromPatientMessageModel = null;
        private MessageModel replyFromMedicMessageModel = null;
        private MessageModel replyFromNurseMessageModel = null;

        private DatabaseSnapshotProvider CreateDatabaseSnapshotForGetMessageListTest(
            ProactServicesProvider servicesProvider ) {
            return new DatabaseSnapshotProvider( servicesProvider )
                .AddInstituteWithRandomValues( out institute )
                .AddProjectWithRandomValues( institute, out project )
                .AddMedicalTeamWithRandomValues( project, out medicalTeam )
                .AddPatientWithRandomValues( medicalTeam, out patient_0 )
                .AddPatientWithRandomValues( medicalTeam, out patient_1 )
                .AddMedicWithRandomValues( medicalTeam, out medic )
                .AddNurseWithRandomValues( medicalTeam, out nurse )
                .AddMessageFromPatientWithRandomValues( patient_0, out messageFromPatient_0 )
                .AddMessageFromPatientWithRandomValues( patient_1, out messageFromPatient_1 )
                .AddReplyFromPatientWithRandomValues(
                    patient_0, (Guid)messageFromPatient_0.MessageId, out replyFromPatientMessageModel )
                .AddReplyFromMedicWithRandomValues(
                    medic, (Guid)messageFromPatient_0.MessageId, out replyFromMedicMessageModel )
                .AddReplyFromNurseWithRandomValues(
                    nurse, (Guid)messageFromPatient_1.MessageId, out replyFromNurseMessageModel );
        }

        [Fact]
        public void _AsPatient_IntegrityCheck() {
            var servicesProvider = new ProactServicesProvider();

            CreateDatabaseSnapshotForGetMessageListTest( servicesProvider );

            var messagesController = new MessagesControllerProvider(
                servicesProvider, patient_0.User, Roles.Patient );
            var result = messagesController.Controller
                .GetMessages( project.Id, medicalTeam.Id, 0 );

            var branchedMessages = ( result as OkObjectResult ).Value as List<BranchedMessagesModel>;

            Assert.NotNull( branchedMessages );
            Assert.Single( branchedMessages );
            Assert.Equal( 2, branchedMessages[0].ReplyMessagesCount );
        }

        [Fact]
        public void _AsMedic_IntegrityCheck() {
            var servicesProvider = new ProactServicesProvider();

            CreateDatabaseSnapshotForGetMessageListTest( servicesProvider );

            var messagesController = new MessagesControllerProvider(
                servicesProvider, medic.User, Roles.MedicalProfessional );
            var result = messagesController.Controller
                .GetMessages( project.Id, medicalTeam.Id, 0 );

            var branchedMessages = ( result as OkObjectResult ).Value as List<BranchedMessagesModel>;

            Assert.NotNull( branchedMessages );
            Assert.Equal( 2, branchedMessages.Count );
            Assert.Equal( 1, branchedMessages[0].ReplyMessagesCount );
            Assert.Equal( 2, branchedMessages[1].ReplyMessagesCount );
        }

        [Fact]
        public void _AsNurse_IntegrityCheck() {
            var servicesProvider = new ProactServicesProvider();

            CreateDatabaseSnapshotForGetMessageListTest( servicesProvider );

            var messagesController = new MessagesControllerProvider(
                servicesProvider, nurse.User, Roles.Nurse );
            var result = messagesController.Controller
                .GetMessages( project.Id, medicalTeam.Id, 0 );

            var branchedMessages = ( result as OkObjectResult ).Value as List<BranchedMessagesModel>;

            Assert.NotNull( branchedMessages );
            Assert.Equal( 2, branchedMessages.Count );
            Assert.Equal( 1, branchedMessages[0].ReplyMessagesCount );
            Assert.Equal( MessageType.Nurse, branchedMessages[0].ReplyMessages[0].MessageType );
            Assert.Equal( 2, branchedMessages[1].ReplyMessagesCount );
            Assert.Equal( MessageType.Patient, branchedMessages[1].ReplyMessages[0].MessageType );
            Assert.Equal( MessageType.Medic, branchedMessages[1].ReplyMessages[1].MessageType );
        }

        [Fact]
        public void _AsResearcher_IntegrityCheck() {
            var servicesProvider = new ProactServicesProvider();

            CreateDatabaseSnapshotForGetMessageListTest( servicesProvider );

            var messagesController = new MessagesControllerProvider(
                servicesProvider, nurse.User, Roles.Researcher );

            var result = messagesController.Controller
                .GetMessages( project.Id, medicalTeam.Id, 0 );

            var branchedMessages = ( result as OkObjectResult ).Value as List<BranchedMessagesModel>;

            Assert.NotNull( branchedMessages );
            Assert.Equal( 2, branchedMessages.Count );
            Assert.Equal( branchedMessages[0].OriginalMessage.AuthorName, patient_0.Code );
            Assert.Equal( branchedMessages[1].OriginalMessage.AuthorName, patient_1.Code );
            Assert.Equal( 1, branchedMessages[0].ReplyMessagesCount );
            Assert.Equal( MessageType.Nurse, branchedMessages[0].ReplyMessages[0].MessageType );
            Assert.Equal( 2, branchedMessages[1].ReplyMessagesCount );
            Assert.Equal( MessageType.Patient, branchedMessages[1].ReplyMessages[0].MessageType );
            Assert.Equal( MessageType.Medic, branchedMessages[1].ReplyMessages[1].MessageType );
        }

        [Fact]
        public void _AsMedic_GetOnlyUnreplied_IntegrityCheck() {
            var servicesProvider = new ProactServicesProvider();

            MessageModel messageWithoutReplies = null;

            CreateDatabaseSnapshotForGetMessageListTest( servicesProvider )
                .AddMessageFromPatientWithRandomValues( patient_0, out messageWithoutReplies );

            var messagesController = new MessagesControllerProvider(
                servicesProvider, medic.User, Roles.MedicalProfessional );
            var result = messagesController.Controller
                .GetUnrepliedMessages( project.Id, medicalTeam.Id, 0 );

            var branchedMessages = ( result as OkObjectResult ).Value as List<BranchedMessagesModel>;

            Assert.NotNull( branchedMessages );
            Assert.Single( branchedMessages );
            Assert.Empty( branchedMessages[0].ReplyMessages );
        }

        [Fact]
        public void _AsMedic_GetOnlyUnrepliedWithAnonymousPatients_IntegrityCheck() {
            var servicesProvider = new ProactServicesProvider();
            Institute institute;
            Project project;
            MedicalTeam medicalTeam;
            Patient anonymousPatient;
            Medic medic;
            MessageModel message_0;
            MessageModel message_1;

            new DatabaseSnapshotProvider( servicesProvider )
                 .AddInstituteWithRandomValues( out institute )
                 .AddProjectWithRandomValues( institute, out project )
                 .AddMedicalTeamWithRandomValues( project, out medicalTeam )
                 .AddPatientAnonymousWithRandomValues( medicalTeam, out anonymousPatient )
                 .AddMedicWithRandomValues( medicalTeam, out medic )
                 .AddMessageFromPatientWithRandomValues( anonymousPatient, out message_0 )
                 .AddMessageFromPatientWithRandomValues( anonymousPatient, out message_1 );

            var messagesController = new MessagesControllerProvider(
                servicesProvider, medic.User, Roles.MedicalProfessional );
            var result = messagesController.Controller
                .GetUnrepliedMessages( project.Id, medicalTeam.Id, 0 );

            var branchedMessages = ( result as OkObjectResult ).Value as List<BranchedMessagesModel>;

            Assert.NotNull( branchedMessages );
            Assert.Equal( anonymousPatient.Code, branchedMessages[0].OriginalMessage.AuthorName );
            Assert.Equal( anonymousPatient.Code, branchedMessages[1].OriginalMessage.AuthorName );
            Assert.Empty( branchedMessages[0].ReplyMessages );
            Assert.Empty( branchedMessages[1].ReplyMessages );
        }

        [Fact]
        public void _AsResearcher_GetOnlyUnreplied_IntegrityCheck() {
            var servicesProvider = new ProactServicesProvider();
            Institute institute;
            Project project;
            MedicalTeam medicalTeam;
            Patient patient;
            Medic medic;
            MessageModel message_0;
            MessageModel message_1;

            new DatabaseSnapshotProvider( servicesProvider )
                 .AddInstituteWithRandomValues( out institute )
                 .AddProjectWithRandomValues( institute, out project )
                 .AddMedicalTeamWithRandomValues( project, out medicalTeam )
                 .AddPatientWithRandomValues( medicalTeam, out patient )
                 .AddMedicWithRandomValues( medicalTeam, out medic )
                 .AddMessageFromPatientWithRandomValues( patient, out message_0 )
                 .AddMessageFromPatientWithRandomValues( patient, out message_1 );

            var messagesController = new MessagesControllerProvider(
                servicesProvider, medic.User, Roles.MedicalProfessional );
            var result = messagesController.Controller
                .GetUnrepliedMessages( project.Id, medicalTeam.Id, 0 );

            var branchedMessages = ( result as OkObjectResult ).Value as List<BranchedMessagesModel>;

            Assert.NotNull( branchedMessages );
            Assert.Equal( patient.Code, branchedMessages[0].OriginalMessage.AuthorName );
            Assert.Equal( patient.Code, branchedMessages[1].OriginalMessage.AuthorName );
            Assert.Empty( branchedMessages[0].ReplyMessages );
            Assert.Empty( branchedMessages[1].ReplyMessages );
        }

        [Fact]
        public void _AsMedic_WithAnonymousPatient_IntegrityCheck() {
            var servicesProvider = new ProactServicesProvider();
            Institute institute;
            Project project;
            MedicalTeam medicalTeam;
            Patient anonymousPatient;
            Medic medic;
            MessageModel message;
            MessageModel reply;

            new DatabaseSnapshotProvider( servicesProvider )
                 .AddInstituteWithRandomValues( out institute )
                 .AddProjectWithRandomValues( institute, out project )
                 .AddMedicalTeamWithRandomValues( project, out medicalTeam )
                 .AddPatientAnonymousWithRandomValues( medicalTeam, out anonymousPatient )
                 .AddMedicWithRandomValues( medicalTeam, out medic )
                 .AddMessageFromPatientWithRandomValues( anonymousPatient, out message )
                 .AddReplyFromPatientWithRandomValues(
                     anonymousPatient, message.MessageId, out reply );

            var messagesController = new MessagesControllerProvider(
                servicesProvider, medic.User, Roles.MedicalProfessional );
            var result = messagesController.Controller
                .GetMessages( project.Id, medicalTeam.Id, 0 );

            var branchedMessages = ( result as OkObjectResult ).Value as List<BranchedMessagesModel>;

            Assert.NotNull( branchedMessages );
            Assert.Equal( anonymousPatient.Code, branchedMessages[0].OriginalMessage.AuthorName );
            Assert.Equal( anonymousPatient.Code, branchedMessages[0].ReplyMessages[0].AuthorName );
        }

        [Fact]
        public void _AsNurse_WithAnonymousPatient_IntegrityCheck() {
            var servicesProvider = new ProactServicesProvider();
            Institute institute;
            Project project;
            MedicalTeam medicalTeam;
            Patient anonymousPatient;
            Nurse nurse;
            MessageModel message;
            MessageModel reply;

            new DatabaseSnapshotProvider( servicesProvider )
                 .AddInstituteWithRandomValues( out institute )
                 .AddProjectWithRandomValues( institute, out project )
                 .AddMedicalTeamWithRandomValues( project, out medicalTeam )
                 .AddPatientAnonymousWithRandomValues( medicalTeam, out anonymousPatient )
                 .AddNurseWithRandomValues( medicalTeam, out nurse )
                 .AddMessageFromPatientWithRandomValues( anonymousPatient, out message )
                 .AddReplyFromPatientWithRandomValues(
                     anonymousPatient, message.MessageId, out reply );

            var messagesController = new MessagesControllerProvider(
                servicesProvider, nurse.User, Roles.Nurse );
            var result = messagesController.Controller
                .GetMessages( project.Id, medicalTeam.Id, 0 );

            var branchedMessages = ( result as OkObjectResult ).Value as List<BranchedMessagesModel>;

            Assert.NotNull( branchedMessages );
            Assert.Equal( anonymousPatient.Code, branchedMessages[0].OriginalMessage.AuthorName );
            Assert.Equal( anonymousPatient.Code, branchedMessages[0].ReplyMessages[0].AuthorName );
        }
    }
}
