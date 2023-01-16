using Microsoft.AspNetCore.Mvc;
using Proact.Services.AuthorizationPolicies;
using Proact.Services.Entities;
using Proact.Services.Models;
using Proact.Services.Models.Messages;
using Proact.Services.Tests.Shared;
using Xunit;

namespace Proact.Services.FunctionalTests.Messages {
    public class MessagesCreationTests {
        private MessageRequestData _messageCreationRequest = new MessageRequestData() {
            Title = "title",
            Body = "body",
            Emotion = PatientMood.Good,
            HasAttachment = false,
            MessageScope = MessageScope.Health
        };

        [Fact]
        public void CreateNewTopicMessage_WithoutAttached_IntegrityCheck() {
            var servicesProvider = new ProactServicesProvider();
            Institute institute = null;
            Project project = null;
            MedicalTeam medicalTeam = null;
            Patient patient = null;
            Medic medic = null;
            Nurse nurse = null;

            new DatabaseSnapshotProvider( servicesProvider )
                .AddInstituteWithRandomValues( out institute )
                .AddProjectWithRandomValues( institute, out project )
                .AddMedicalTeamWithRandomValues( project, out medicalTeam )
                .AddPatientWithRandomValues( medicalTeam, out patient )
                .AddMedicWithRandomValues( medicalTeam, out medic )
                .AddNurseWithRandomValues( medicalTeam, out nurse );

            var messagesController = new MessagesControllerProvider( 
                servicesProvider, patient.User, Roles.Patient );
            var result = messagesController.Controller.CreateNewTopicMessage(
                project.Id, medicalTeam.Id, _messageCreationRequest );

            var messageModel = ( result as OkObjectResult ).Value as MessageModel;

            MessageEqual.Equal( _messageCreationRequest, messageModel );
            Assert.Equal( medicalTeam.Id, messageModel.MedicalTeamId );
            Assert.Contains( patient.User.Name, messageModel.AuthorName );
            Assert.Equal( MessageState.Active, messageModel.State );
            Assert.Equal( MessageType.Patient, messageModel.MessageType );
            Assert.False( messageModel.HasAnalysis );
        }

        [Fact]
        public void CreateNewBroadcastMessage_IntegrityCheck() {
            var servicesProvider = new ProactServicesProvider();
            Institute institute = null;
            Project project = null;
            MedicalTeam medicalTeam = null;
            Medic medic = null;

            new DatabaseSnapshotProvider( servicesProvider )
                .AddInstituteWithRandomValues( out institute )
                .AddProjectWithRandomValues( institute, out project )
                .AddMedicalTeamWithRandomValues( project, out medicalTeam )
                .AddMedicWithRandomValues( medicalTeam, out medic );

            var messagesController = new MessagesControllerProvider(
                servicesProvider, medic.User, Roles.Patient );
            var result = messagesController.Controller.CreateBroadcastMessage(
                project.Id, medicalTeam.Id, _messageCreationRequest );

            var messageModel = ( result as OkObjectResult ).Value as MessageModel;

            MessageEqual.Equal( _messageCreationRequest, messageModel );
            Assert.NotEqual( medic.User.Name, messageModel.AuthorName );
            Assert.Equal( medicalTeam.Id, messageModel.MedicalTeamId );
            Assert.Equal( MessageState.Active, messageModel.State );
            Assert.Equal( MessageType.Broadcast, messageModel.MessageType );
            Assert.False( messageModel.HasAnalysis );
        }

        [Fact]
        public void CreateReply_FromPatient_WithoutAttached_IntegrityCheck() {
            var servicesProvider = new ProactServicesProvider();
            Institute institute = null;
            Project project = null;
            MedicalTeam medicalTeam = null;
            Patient patient = null;
            MessageModel messageModel = null;

            new DatabaseSnapshotProvider( servicesProvider )
                .AddInstituteWithRandomValues( out institute )
                .AddProjectWithRandomValues( institute, out project )
                .AddMedicalTeamWithRandomValues( project, out medicalTeam )
                .AddPatientWithRandomValues( medicalTeam, out patient )
                .AddMessageFromPatientWithRandomValues( patient, out messageModel );

            var messagesController = new MessagesControllerProvider(
                servicesProvider, patient.User, Roles.Patient );
            var result = messagesController.Controller.ReplyMessage(
                project.Id, medicalTeam.Id, messageModel.MessageId, _messageCreationRequest );

            var resultMessageModel = ( result as OkObjectResult ).Value as MessageModel;

            MessageEqual.Equal( _messageCreationRequest, resultMessageModel );
            Assert.Equal( medicalTeam.Id, resultMessageModel.MedicalTeamId );
            Assert.Contains( patient.User.Name, resultMessageModel.AuthorName );
            Assert.Equal( MessageState.Active, resultMessageModel.State );
            Assert.Equal( MessageType.Patient, resultMessageModel.MessageType );
        }

        [Fact]
        public void CreateReply_FromMedic_WithoutAttached_IntegrityCheck() {
            var servicesProvider = new ProactServicesProvider();
            Institute institute = null;
            Project project = null;
            MedicalTeam medicalTeam = null;
            Patient patient = null;
            Medic medic = null;
            MessageModel messageModel = null;

            new DatabaseSnapshotProvider( servicesProvider )
                .AddInstituteWithRandomValues( out institute )
                .AddProjectWithRandomValues( institute, out project )
                .AddMedicalTeamWithRandomValues( project, out medicalTeam )
                .AddPatientWithRandomValues( medicalTeam, out patient )
                .AddMedicWithRandomValues( medicalTeam, out medic )
                .AddMessageFromPatientWithRandomValues( patient, out messageModel );

            var messagesController = new MessagesControllerProvider(
                servicesProvider, medic.User, Roles.MedicalProfessional );
            var result = messagesController.Controller.ReplyMessage(
                project.Id, medicalTeam.Id, messageModel.MessageId, _messageCreationRequest );

            var resultMessageModel = ( result as OkObjectResult ).Value as MessageModel;

            MessageEqual.Equal( _messageCreationRequest, resultMessageModel );
            Assert.Equal( medicalTeam.Id, resultMessageModel.MedicalTeamId );
            Assert.Contains( medic.User.Name, resultMessageModel.AuthorName );
            Assert.Equal( MessageState.Active, resultMessageModel.State );
            Assert.Equal( MessageType.Medic, resultMessageModel.MessageType );
        }

        [Fact]
        public void CreateReply_FromNurse_WithoutAttached_IntegrityCheck() {
            var servicesProvider = new ProactServicesProvider();
            Institute institute = null;
            Project project = null;
            MedicalTeam medicalTeam = null;
            Patient patient = null;
            Nurse nurse = null;
            MessageModel messageModel = null;

            new DatabaseSnapshotProvider( servicesProvider )
                .AddInstituteWithRandomValues( out institute )
                .AddProjectWithRandomValues( institute, out project )
                .AddMedicalTeamWithRandomValues( project, out medicalTeam )
                .AddPatientWithRandomValues( medicalTeam, out patient )
                .AddNurseWithRandomValues( medicalTeam, out nurse)
                .AddMessageFromPatientWithRandomValues( patient, out messageModel );

            var messagesController = new MessagesControllerProvider(
                servicesProvider, nurse.User, Roles.Nurse );
            var result = messagesController.Controller.ReplyMessage(
                project.Id, medicalTeam.Id, messageModel.MessageId, _messageCreationRequest );

            var resultMessageModel = ( result as OkObjectResult ).Value as MessageModel;

            MessageEqual.Equal( _messageCreationRequest, resultMessageModel );
            Assert.Equal( medicalTeam.Id, resultMessageModel.MedicalTeamId );
            Assert.Contains( nurse.User.Name, resultMessageModel.AuthorName );
            Assert.Equal( MessageState.Active, resultMessageModel.State );
            Assert.Equal( MessageType.Nurse, resultMessageModel.MessageType );
        }
    }
}
