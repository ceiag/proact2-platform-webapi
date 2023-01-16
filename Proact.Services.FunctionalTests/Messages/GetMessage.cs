using Microsoft.AspNetCore.Mvc;
using Proact.Services.AuthorizationPolicies;
using Proact.Services.Entities;
using Proact.Services.Entities.MessageAnalysis;
using Proact.Services.Models;
using Proact.Services.Tests.Shared;
using System;
using System.Collections.Generic;
using Xunit;

namespace Proact.Services.FunctionalTests.Messages {
    public class GetMessage {
        [Fact]
        public void _IntegrityCheck() {
            var servicesProvider = new ProactServicesProvider();
            Institute institute = null;
            Project project = null;
            MedicalTeam medicalTeam = null;
            Patient patient = null;
            Medic medic = null;
            MessageModel messageModel = null;
            MessageModel replyFromPatientMessageModel = null;
            MessageModel replyFromMedicMessageModel = null;

            var dbSnapshot = new DatabaseSnapshotProvider( servicesProvider );
            dbSnapshot
                .AddInstituteWithRandomValues( out institute )
                .AddProjectWithRandomValues( institute, out project )
                .AddMedicalTeamWithRandomValues( project, out medicalTeam )
                .AddPatientWithRandomValues( medicalTeam, out patient )
                .AddMedicWithRandomValues( medicalTeam, out medic )
                .AddMessageFromPatientWithRandomValues( patient, out messageModel )
                .AddReplyFromPatientWithRandomValues(
                    patient, (Guid)messageModel.MessageId, out replyFromPatientMessageModel )
                .AddReplyFromMedicWithRandomValues(
                    medic, (Guid)messageModel.MessageId, out replyFromMedicMessageModel );

            var messagesController = new MessagesControllerProvider(
                servicesProvider, patient.User, Roles.Patient );
            var result = messagesController.Controller
                .GetMessage( project.Id, medicalTeam.Id, messageModel.MessageId );

            var branchedMessage = ( result as OkObjectResult ).Value as BranchedMessagesModel;

            Assert.NotNull( branchedMessage );
            Assert.Equal( 2, branchedMessage.ReplyMessages.Count );
            MessageEqual.Equal( messageModel, branchedMessage.OriginalMessage );
        }

        [Fact]
        public void _FromNotAuthorPatient_Return_NotAuthorized() {
            var servicesProvider = new ProactServicesProvider();
            Institute institute = null;
            Project project = null;
            MedicalTeam medicalTeam = null;
            Patient patient_0 = null;
            Patient patient_1 = null;
            MessageModel messageModel = null;

            var dbSnapshot = new DatabaseSnapshotProvider( servicesProvider );
            dbSnapshot
                .AddInstituteWithRandomValues( out institute )
                .AddProjectWithRandomValues( institute, out project )
                .AddMedicalTeamWithRandomValues( project, out medicalTeam )
                .AddPatientWithRandomValues( medicalTeam, out patient_0 )
                .AddPatientWithRandomValues( medicalTeam, out patient_1 )
                .AddMessageFromPatientWithRandomValues( patient_0, out messageModel );

            var messagesController = new MessagesControllerProvider(
                servicesProvider, patient_1.User, Roles.Patient );
            var result = messagesController.Controller
                .GetMessage( project.Id, medicalTeam.Id, messageModel.MessageId );

            Assert.Equal( 401, ( result as UnauthorizedObjectResult ).StatusCode );
        }

        [Fact]
        public void _FromPatient_Return_Ok_WithAnalysisHidden() {
            var servicesProvider = new ProactServicesProvider();
            Institute institute = null;
            Project project = null;
            MedicalTeam medicalTeam = null;
            Patient patient = null;
            Medic medic = null;
            MessageModel messageModel = null;
            Lexicon lexicon = null;
            Analysis analysis = null;

            var dbSnapshot = new DatabaseSnapshotProvider( servicesProvider );
            dbSnapshot
                .AddInstituteWithRandomValues( out institute )
                .AddProjectWithRandomValues( institute, out project )
                .AddMedicalTeamWithRandomValues( project, out medicalTeam )
                .AddPatientWithRandomValues( medicalTeam, out patient )
                .AddMessageFromPatientWithRandomValues( patient, out messageModel )
                .AddMedicWithRandomValues( medicalTeam, out medic )
                .AddLexiconWithRandomValues( institute, out lexicon )
                .AddAnalysisWithRandomValues( medic, messageModel.MessageId, lexicon, out analysis );

            var messagesController = new MessagesControllerProvider(
                servicesProvider, patient.User, Roles.Patient );
            var result = messagesController.Controller
                .GetMessage( project.Id, medicalTeam.Id, messageModel.MessageId );

            var branchedMessage = ( result as OkObjectResult ).Value as BranchedMessagesModel;

            Assert.NotNull( branchedMessage );
            Assert.False( branchedMessage.OriginalMessage.HasAnalysis );
        }

        [Fact]
        public void _AsMedic_Return_Ok_WithAnalysisVisible() {
            var servicesProvider = new ProactServicesProvider();
            Institute institute = null;
            Project project = null;
            MedicalTeam medicalTeam = null;
            Patient patient = null;
            Medic medic = null;
            MessageModel messageModel = null;
            Lexicon lexicon = null;
            Analysis analysis = null;

            var dbSnapshot = new DatabaseSnapshotProvider( servicesProvider );
            dbSnapshot
                .AddInstituteWithRandomValues( out institute )
                .AddProjectWithRandomValues( institute, out project )
                .AddMedicalTeamWithRandomValues( project, out medicalTeam )
                .AddPatientWithRandomValues( medicalTeam, out patient )
                .AddMessageFromPatientWithRandomValues( patient, out messageModel )
                .AddMedicWithRandomValues( medicalTeam, out medic )
                .AddLexiconWithRandomValues( institute, out lexicon )
                .AddAnalysisWithRandomValues( medic, messageModel.MessageId, lexicon, out analysis );

            var messagesController = new MessagesControllerProvider(
                servicesProvider, patient.User, Roles.MedicalProfessional );
            var result = messagesController.Controller
                .GetMessage( project.Id, medicalTeam.Id, messageModel.MessageId );

            var branchedMessage = ( result as OkObjectResult ).Value as BranchedMessagesModel;

            Assert.NotNull( branchedMessage );
            Assert.True( branchedMessage.OriginalMessage.HasAnalysis );
            Assert.Equal( 1, branchedMessage.OriginalMessage.AnalysisCount );
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
                .GetMessage( project.Id, medicalTeam.Id, message.MessageId );

            var branchedMessage = ( result as OkObjectResult ).Value as BranchedMessagesModel;

            Assert.NotNull( branchedMessage );
            Assert.Equal( anonymousPatient.Code, branchedMessage.OriginalMessage.AuthorName );
            Assert.Equal( anonymousPatient.Code, branchedMessage.ReplyMessages[0].AuthorName );
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
                .GetMessage( project.Id, medicalTeam.Id, message.MessageId );

            var branchedMessage = ( result as OkObjectResult ).Value as BranchedMessagesModel;

            Assert.NotNull( branchedMessage );
            Assert.Equal( anonymousPatient.Code, branchedMessage.OriginalMessage.AuthorName );
            Assert.Equal( anonymousPatient.Code, branchedMessage.ReplyMessages[0].AuthorName );
        }
    }
}
