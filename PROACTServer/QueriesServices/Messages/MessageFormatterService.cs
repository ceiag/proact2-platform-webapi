using Proact.Services.AuthorizationPolicies;
using Proact.Services.Entities;
using Proact.Services.Messages;
using Proact.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Proact.Services.QueriesServices {
    public class MessageFormatterService : IMessageFormatterService {
        private readonly IMessagesQueriesService _messageQueriesHelper;
        private readonly IOrganizedMessagesProvider _branchedMessageProvider;
        private readonly IPatientQueriesService _patientQueriesService;

        public MessageFormatterService( 
            IMessagesQueriesService messageQueriesHelper, 
            IOrganizedMessagesProvider brancheMessageProvider,
            IPatientQueriesService patientQueriesService ) {
            _messageQueriesHelper = messageQueriesHelper;
            _branchedMessageProvider = brancheMessageProvider;
            _patientQueriesService = patientQueriesService;
        }

        public BranchedMessagesModel GetMessageAsMedic( Guid messageId, int limitRepliesCount ) {
            var messages = _messageQueriesHelper.GetMessage( messageId );
            var branchedMessages = _branchedMessageProvider
                .GetBranchedMessage( messages, limitRepliesCount );

            ChangeAuthNameWithCodeForAnonymousPatientMessage( branchedMessages );
            return branchedMessages;
        }

        public BranchedMessagesModel GetMessageAsPatient( Guid messageId, int limitRepliesCount ) {
            var messages = _messageQueriesHelper.GetMessage( messageId );
            var branchedMessage = _branchedMessageProvider
                .GetBranchedMessage( messages, limitRepliesCount );

            RemoveAnalysisFromBranchedMessage( branchedMessage );
            ChangeAuthNameWithCodeForAnonymousPatientMessage( branchedMessage );

            return branchedMessage;
        }

        public BranchedMessagesModel GetPatientMessageForAnalystConsole(
            Guid messageId, UserRoles requesterRoles, int limitRepliesCount ) {
            var message = _messageQueriesHelper.GetMessage( messageId );
            var branchedMessage = _branchedMessageProvider
                .GetBranchedMessage( message, limitRepliesCount );

            if ( requesterRoles.HasRoleOf( Roles.Researcher ) ) {
                AnonimizePatientMessage(
                    branchedMessage, _patientQueriesService.Get( (Guid)message.AuthorId ) );
            }
            else {
                ChangeAuthNameWithCodeForAnonymousPatientMessage( branchedMessage );
            }

            return branchedMessage;
        }

        public List<BranchedMessagesModel> GetPatientMessagesForAnalystConsole(
            Patient patient, UserRoles requesterRoles, int pagingCount, int limitRepliesCount ) {
            var messages = _messageQueriesHelper.GetMessagesAsPatient( patient, pagingCount );
            var branchedMessages = _branchedMessageProvider
                .GetBranchedMessages( messages, limitRepliesCount );

            if ( requesterRoles.HasRoleOf( Roles.Researcher ) ) {
                AnonimizeBranchedPatientsMessages( branchedMessages );
            }
            else {
                ChangeAuthNameWithCodeForAnonymousPatientsMessages( branchedMessages );
            }

            return branchedMessages;
        }

        private List<BranchedMessagesModel> GetBranchedMessagesRemovingAnalysis( 
            List<BranchedMessagesModel> breanchedMessages ) {

            foreach ( var message in breanchedMessages ) {
                RemoveAnalysisFromBranchedMessage( message );
            }

            return breanchedMessages;
        }

        private void RemoveAnalysisFromBranchedMessage( BranchedMessagesModel branchedMessage ) {
            branchedMessage.OriginalMessage.AnalysisCount = 0;
        }

        private List<MessageModel> GetMessagesListRemovingAnalysis( List<MessageModel> messages ) {
            foreach ( var message in messages ) {
                message.AnalysisCount = 0;
            }

            return messages;
        }

        public List<BranchedMessagesModel> GetMessagesAsPatient( 
            Patient patient, int pagingCount, int limitRepliesCount ) {
            var messages = _messageQueriesHelper.GetMessagesAsPatient( patient, pagingCount );
            var branchedMessages = _branchedMessageProvider
                .GetBranchedMessages( messages, limitRepliesCount );

            ChangeAuthNameWithCodeForAnonymousPatientsMessages( branchedMessages );
            return GetBranchedMessagesRemovingAnalysis( branchedMessages );
        }

        public List<MessageModel> SearchMessagesAsPatient( Patient patient, string searchParams, int maxRows ) {
            var messages = _messageQueriesHelper.SearchMessagesAsPatient( patient, searchParams, maxRows );
            var messagesAsList = _branchedMessageProvider.GetAsMonodimensionalList( messages );

            ChangeAuthNameWithCodeForAnonymousPatientMessage( messagesAsList );
            return GetMessagesListRemovingAnalysis( messagesAsList );
        }

        public List<MessageModel> SearchMessagesAsMedic( Guid medicalTeamId, string searchParams, int maxRows ) {
            var searchedMessages = _messageQueriesHelper.SearchMessagesAsMedic( medicalTeamId, searchParams, maxRows );
            var listOfMessages = _branchedMessageProvider.GetAsMonodimensionalList( searchedMessages );

            ChangeAuthNameWithCodeForAnonymousPatientMessage( listOfMessages );
            return listOfMessages;
        }

        public List<MessageModel> SearchMessagesAsResearcher( Guid medicalTeamId, string searchParams, int maxRows ) {
            var messages = _messageQueriesHelper.SearchMessagesAsMedic( medicalTeamId, searchParams, maxRows );
            var messagesOrderAsList = _branchedMessageProvider.GetAsMonodimensionalList( messages );
            
            AnonimizePatientsMessages( messagesOrderAsList );
            return messagesOrderAsList;
        }

        public List<BranchedMessagesModel> GetMessagesAsMedic(
            MedicalTeam medicalTeam, int pagingCount, int limitRepliesCount ) {

            var messages = _messageQueriesHelper.GetMessagesAsMedic( medicalTeam, pagingCount );
            var branchedMessages = _branchedMessageProvider
                .GetBranchedMessages( messages, limitRepliesCount );

            ChangeAuthNameWithCodeForAnonymousPatientsMessages( branchedMessages );
            return branchedMessages;
        }

        public List<BranchedMessagesModel> GetAllMessagesForMedicalTeam( Guid medicalTeamId ) {
            var messages = _messageQueriesHelper
                .GetAllMessagesForMedicalTeam( medicalTeamId );
            var branchedMessages = _branchedMessageProvider
                .GetBranchedMessages( messages, int.MaxValue );

            AnonimizeBranchedPatientsMessages( branchedMessages );
            return branchedMessages;
        }

        public List<BranchedMessagesModel> GetMessagesAsResearcher(
            MedicalTeam medicalTeam, int pagingCount, int limitRepliesCount ) {

            var messages = _messageQueriesHelper.GetMessagesAsMedic( medicalTeam, pagingCount );
            var branchedMessages = _branchedMessageProvider
                .GetBranchedMessages( messages, limitRepliesCount );

            AnonimizeBranchedPatientsMessages( branchedMessages );
            return branchedMessages;
        }

        public List<BranchedMessagesModel> GetMessagesAsMedicUnreplied(
            Guid medicalTeamId, int pagingCount, int limitRepliesCount ) {

            var messages = _messageQueriesHelper.GetMessagesAsMedicUnreplied( medicalTeamId, pagingCount );
            var branchedMessages = _branchedMessageProvider
                .GetBranchedMessages( messages, limitRepliesCount );

            AnonimizeBranchedPatientsMessages( branchedMessages );
            return branchedMessages;
        }

        public List<BranchedMessagesModel> GetMessagesAsResearcher(
            Guid medicalTeamId, int pagingCount, int limitRepliesCount ) {
            var messages = _messageQueriesHelper.GetMessagesAsMedicUnreplied( medicalTeamId, pagingCount );
            
            var branchedMessages = _branchedMessageProvider
                .GetBranchedMessages( messages, limitRepliesCount );

            AnonimizeBranchedPatientsMessages( branchedMessages );
            return branchedMessages;
        }

        public List<User> GetMessageRecipients( Guid messageId ) {
            return _messageQueriesHelper.GetRecipients( messageId );
        }

        public List<User> GetMessageRecipientsExceptUser( Guid userId, Guid messageId ) {
            return _messageQueriesHelper.GetRecipientsExceptUser( userId, messageId );
        }

        private void AnonimizeBranchedPatientsMessages( List<BranchedMessagesModel> messages ) {
            foreach ( var message in messages ) {
                if ( message.OriginalMessage.MessageType == MessageType.Patient ) {
                    var patient = _patientQueriesService
                        .Get( (Guid)message.OriginalMessage.AuthorId );
                    message.OriginalMessage.AuthorName = patient.Code;

                    AnonimizePatientsMessages( message.ReplyMessages, patient );
                }
            }
        }

        private void ChangeAuthNameWithCodeForAnonymousPatientsMessages( List<BranchedMessagesModel> messages ) {
            foreach ( var message in messages ) {
                ChangeAuthNameWithCodeForAnonymousPatientMessage( message );
            }
        }

        private void ChangeAuthNameWithCodeForAnonymousPatientMessage( BranchedMessagesModel message ) {
            if ( message.OriginalMessage.MessageType != MessageType.Patient )
                return;

            var patient = _patientQueriesService.Get( (Guid)message.OriginalMessage.AuthorId );

            if ( string.IsNullOrWhiteSpace( patient.User.Name ) ) {
                message.OriginalMessage.AuthorName = patient.Code;
                AnonimizePatientsMessages( message.ReplyMessages, patient );
            }
        }

        private void ChangeAuthNameWithCodeForAnonymousPatientMessage( List<MessageModel> messages ) {
            foreach ( var message in messages ) {
                ChangeAuthNameWithCodeForAnonymousPatientMessage( message );
            }
        }

        private void ChangeAuthNameWithCodeForAnonymousPatientMessage( MessageModel message ) {
            if ( message.MessageType != MessageType.Patient )
                return;

            var patient = _patientQueriesService.Get( (Guid)message.AuthorId );

            if ( string.IsNullOrWhiteSpace( patient.User.Name ) ) {
                message.AuthorName = patient.Code;
            }
        }

        private void AnonimizePatientsMessages( List<MessageModel> messages, Patient authorPatient ) {
            foreach ( var message in messages ) {
                if ( message.MessageType == MessageType.Patient ) {
                    message.AuthorName = authorPatient.Code;
                }
            }
        }

        private void AnonimizePatientsMessages( List<MessageModel> messages ) {
            foreach ( var message in messages ) {
                if ( message.MessageType == MessageType.Patient ) {
                    message.AuthorName = _patientQueriesService.Get( (Guid)message.AuthorId ).Code;
                }
            }
        }

        private void AnonimizePatientMessage( BranchedMessagesModel message, Patient authorPatient ) {
            message.OriginalMessage.AuthorName = authorPatient.Code;
            AnonimizePatientsMessages( message.ReplyMessages );
        }
    }
}
