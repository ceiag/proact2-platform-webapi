using Proact.Services.AuthorizationPolicies;
using Proact.Services.Entities;
using Proact.Services.Models;
using Proact.Services.Models.Messages;
using Proact.Services.Models.Stats;
using Proact.Services.QueriesServices;
using Proact.Services.Tests.Shared.Configs;
using System;
using System.Collections.Generic;

namespace Proact.Services.Tests.Shared {
    public static class MessageSnapshotCreator {
        private static MessageCreationParams GetMessageCreationParamsWithRandomValues(
            MedicalTeam medicalTeam, User user, string role ) {
            var messageCreationRequest = new MessageRequestData() {
                Title = "title",
                Body = DefaultConfigs.Message,
                Emotion = PatientMood.Good,
                HasAttachment = false,
                MessageScope = MessageScope.Health
            };

            var messageCreationParams = new MessageCreationParams() {
                MedicalTeam = medicalTeam,
                MessageRequestData = messageCreationRequest,
                ShowAfterCreation = true,
                User = user,
                UserRoles = new UserRoles( new List<string>() { role } )
            };

            return messageCreationParams;
        }

        public static DatabaseSnapshotProvider AddMessageFromPatientWithRandomValues(
            this DatabaseSnapshotProvider snapshotProvider, Patient patient, out MessageModel message ) {
            var messageCreationParams = GetMessageCreationParamsWithRandomValues(
                patient.MedicalTeam, patient.User, Roles.Patient );

            message = snapshotProvider.ServiceProvider
                .GetEditorService<IMessageEditorService>()
                .CreateNewTopicMessage( messageCreationParams );

            snapshotProvider.ServiceProvider.Database.SaveChanges();

            return snapshotProvider;
        }

        public static DatabaseSnapshotProvider AddMessageFromPatientWithRandomValues(
            this DatabaseSnapshotProvider snapshotProvider, Patient patient, int count ) {
            for ( int i = 0; i < count; ++i ) {
                MessageModel message = null;
                AddMessageFromPatientWithRandomValues( snapshotProvider, patient, out message );
            }
            
            return snapshotProvider;
        }

        public static DatabaseSnapshotProvider AddMessageFromPatientWithAttachmentWithRandomValues(
            this DatabaseSnapshotProvider snapshotProvider, 
            Patient patient, AttachmentType type, out MessageModel message ) {
            AddMessageFromPatientWithRandomValues( snapshotProvider, patient, out message );

            snapshotProvider
                .ServiceProvider.Database.MessageAttachments
                .Add( new MessageAttachment() {
                    MessageId = message.MessageId,
                    AttachmentType = type,
                    DurationInMilliseconds = DefaultConfigs.MediaDuration
                } );

            snapshotProvider.ServiceProvider.Database.SaveChanges();
            return snapshotProvider;
        }

        public static DatabaseSnapshotProvider AddMessageFromPatientWithAttachmentWithRandomValues(
            this DatabaseSnapshotProvider snapshotProvider, Patient patient, 
            AttachmentType type, int count ) {
            for ( int i = 0; i < count; ++i ) {
                MessageModel message = null;
                AddMessageFromPatientWithAttachmentWithRandomValues( snapshotProvider, patient, type, out message );
            }
            
            return snapshotProvider;
        }

        public static DatabaseSnapshotProvider AddReplyFromPatientWithAttachmentWithRandomValues(
            this DatabaseSnapshotProvider snapshotProvider,
            Patient patient, Guid originalMessageId, AttachmentType type, out MessageModel reply ) {
            AddReplyFromPatientWithRandomValues( 
                snapshotProvider, patient, originalMessageId, out reply );

            snapshotProvider
                .ServiceProvider.Database.MessageAttachments
                .Add( new MessageAttachment() {
                    MessageId = reply.MessageId,
                    AttachmentType = type,
                    DurationInMilliseconds = DefaultConfigs.MediaDuration
                } );

            snapshotProvider.ServiceProvider.Database.SaveChanges();
            return snapshotProvider;
        }

        public static DatabaseSnapshotProvider AddReplyFromPatientWithAttachmentWithRandomValues(
            this DatabaseSnapshotProvider snapshotProvider, 
            Patient patient, Guid originalMessageId, AttachmentType type, int count ) {
            for ( int i = 0; i < count; ++i ) {
                MessageModel reply = null;
                AddReplyFromPatientWithAttachmentWithRandomValues(
                    snapshotProvider, patient, originalMessageId, type, out reply );
            }
            
            return snapshotProvider;
        }

        public static DatabaseSnapshotProvider AddReplyFromMedicWithAttachmentWithRandomValues(
            this DatabaseSnapshotProvider snapshotProvider, Medic medic, 
            Guid originalMessageId, AttachmentType type, out MessageModel reply ) {
            AddReplyFromMedicWithRandomValues(
                snapshotProvider, medic, originalMessageId, out reply );

            snapshotProvider
                .ServiceProvider.Database.MessageAttachments
                .Add( new MessageAttachment() {
                    MessageId = reply.MessageId,
                    AttachmentType = type,
                    DurationInMilliseconds = DefaultConfigs.MediaDuration
                } );

            snapshotProvider.ServiceProvider.Database.SaveChanges();
            return snapshotProvider;
        }

        public static DatabaseSnapshotProvider AddReplyFromMedicWithAttachmentWithRandomValues(
            this DatabaseSnapshotProvider snapshotProvider, Medic medic,
            Guid originalMessageId, AttachmentType type, int count ) {
            for ( int i = 0; i < count; ++i ) {
                MessageModel reply = null;
                AddReplyFromMedicWithAttachmentWithRandomValues(
                    snapshotProvider, medic, originalMessageId, type, out reply );
            }
            
            return snapshotProvider;
        }

        public static DatabaseSnapshotProvider AddReplyFromNurseWithAttachmentWithRandomValues(
            this DatabaseSnapshotProvider snapshotProvider, Nurse nurse,
            Guid originalMessageId, AttachmentType type, out MessageModel reply ) {
            AddReplyFromNurseWithRandomValues(
                snapshotProvider, nurse, originalMessageId, out reply );

            snapshotProvider
                .ServiceProvider.Database.MessageAttachments
                .Add( new MessageAttachment() {
                    MessageId = reply.MessageId,
                    AttachmentType = type,
                    DurationInMilliseconds = DefaultConfigs.MediaDuration
                } );

            snapshotProvider.ServiceProvider.Database.SaveChanges();
            return snapshotProvider;
        }

        public static DatabaseSnapshotProvider AddReplyFromNurseWithAttachmentWithRandomValues(
            this DatabaseSnapshotProvider snapshotProvider, Nurse nurse,
            Guid originalMessageId, AttachmentType type, int count ) {
            for ( int i = 0; i < count; ++i ) {
                MessageModel reply = null;
                AddReplyFromNurseWithAttachmentWithRandomValues(
                    snapshotProvider, nurse, originalMessageId, type, out reply );
            }
            
            return snapshotProvider;
        }

        public static DatabaseSnapshotProvider AddMessageFromPatient(
            this DatabaseSnapshotProvider snapshotProvider, Patient patient, 
            string messageContent, out MessageModel message ) {
            var messageCreationParams = GetMessageCreationParamsWithRandomValues(
                patient.MedicalTeam, patient.User, Roles.Patient );

            messageCreationParams.MessageRequestData.Body = messageContent;
            message = snapshotProvider.ServiceProvider
                .GetEditorService<IMessageEditorService>()
                .CreateNewTopicMessage( messageCreationParams );

            snapshotProvider.ServiceProvider.Database.SaveChanges();

            return snapshotProvider;
        }

        public static DatabaseSnapshotProvider AddReplyFromPatientWithRandomValues(
           this DatabaseSnapshotProvider snapshotProvider, Patient patient,
           Guid originalMessageId, out MessageModel message ) {
            var messageCreationParams = GetMessageCreationParamsWithRandomValues(
                patient.MedicalTeam, patient.User, Roles.Patient );
            
            message = snapshotProvider.ServiceProvider
                .GetEditorService<IMessageEditorService>()
                .ReplyToMessage( messageCreationParams, originalMessageId );

            snapshotProvider.ServiceProvider.Database.SaveChanges();

            return snapshotProvider;
        }

        public static DatabaseSnapshotProvider AddReplyFromPatientWithRandomValues(
           this DatabaseSnapshotProvider snapshotProvider, 
           Patient patient, Guid originalMessageId, int count ) {

            for ( int i = 0; i < count; ++i ) {
                MessageModel reply = null;
                AddReplyFromPatientWithRandomValues(
                    snapshotProvider, patient, originalMessageId, out reply );
            }

            return snapshotProvider;
        }

        public static DatabaseSnapshotProvider AddReplyFromMedicWithRandomValues(
           this DatabaseSnapshotProvider snapshotProvider, Medic medic,
           Guid originalMessageId, out MessageModel message ) {
            var messageCreationParams = GetMessageCreationParamsWithRandomValues(
                medic.MedicalTeams[0], medic.User, Roles.MedicalProfessional );

            message = snapshotProvider.ServiceProvider
                .GetEditorService<IMessageEditorService>()
                .ReplyToMessage( messageCreationParams, originalMessageId );

            snapshotProvider.ServiceProvider.Database.SaveChanges();
            return snapshotProvider;
        }

        public static DatabaseSnapshotProvider AddReplyFromMedicWithRandomValues(
           this DatabaseSnapshotProvider snapshotProvider, Medic medic, 
           Guid originalMessageId, int count ) {
            for ( int i = 0; i < count; ++i ) {
                MessageModel reply = null;
                AddReplyFromMedicWithRandomValues( snapshotProvider, medic, originalMessageId, out reply );
            }
            
            return snapshotProvider;
        }

        public static DatabaseSnapshotProvider AddReplyFromNurseWithRandomValues(
           this DatabaseSnapshotProvider snapshotProvider, Nurse nurse,
           Guid originalMessageId, out MessageModel message ) {
            var messageCreationParams = GetMessageCreationParamsWithRandomValues(
                nurse.MedicalTeams[0], nurse.User, Roles.Nurse );

            message = snapshotProvider.ServiceProvider
                .GetEditorService<IMessageEditorService>()
                .ReplyToMessage( messageCreationParams, originalMessageId );

            snapshotProvider.ServiceProvider.Database.SaveChanges();

            return snapshotProvider;
        }

        public static DatabaseSnapshotProvider AddReplyFromNurseWithRandomValues(
           this DatabaseSnapshotProvider snapshotProvider, Nurse nurse, 
           Guid originalMessageId, int count ) {
            for ( int i = 0; i < count; ++i ) {
                MessageModel reply = null;
                AddReplyFromNurseWithRandomValues( snapshotProvider, nurse, originalMessageId, out reply );
            }
            
            return snapshotProvider;
        }
    }
}
