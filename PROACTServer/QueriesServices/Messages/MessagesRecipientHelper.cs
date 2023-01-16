using Proact.Services.Entities;
using System;
using System.Linq;

namespace Proact.Services {
    public static class MessagesRecipientHelper {
        public static void AddRecipientsToMessage( 
            ProactDatabaseContext database, Message message,
            User currentUser, MedicalTeam medicalTeam ) {

            AddMedicsFromMedicalTeam( message, currentUser, medicalTeam );
            AddNursesFromMedicalTeam( message, currentUser, medicalTeam );

            if ( message.MessageType == MessageType.Broadcast ) {   
                AddPatientsRecipients( message, currentUser, medicalTeam );
            }
            else {
                AddPatientRecipient( message, database, currentUser );
            }
        }

        public static void AddPatientsRecipients( 
            Message newMessage, User currentUser, MedicalTeam medicalTeam ) {
           
            foreach ( var patient in medicalTeam.Patients ) {
                var messageRecipient = new MessageRecipient {
                    MessageId = newMessage.Id,
                    UserId = patient.UserId,
                    IsRead = patient.UserId == currentUser.Id,
                    ReadTime = patient.UserId == currentUser.Id ? DateTime.UtcNow : (DateTime?)null,
                    User = patient.User
                };

                newMessage.Recipients.Add( messageRecipient );
            }
        }

        private static void AddMedicsFromMedicalTeam( 
            Message newMessage, User currentUser, MedicalTeam medicalTeam ) {
            foreach ( var medic in medicalTeam.Medics ) {
                var messageRecipient = new MessageRecipient {
                    MessageId = newMessage.Id,
                    UserId = medic.UserId,
                    User = medic.User,
                    IsRead = medic.UserId == currentUser.Id,
                    ReadTime = medic.UserId == currentUser.Id ? DateTime.UtcNow : (DateTime?)null
                };

                newMessage.Recipients.Add( messageRecipient );
            }
        }

        private static void AddNursesFromMedicalTeam(
           Message newMessage, User currentUser, MedicalTeam medicalTeam ) {
            foreach ( var nurse in medicalTeam.Nurses ) {
                var messageRecipient = new MessageRecipient {
                    MessageId = newMessage.Id,
                    UserId = nurse.UserId,
                    User = nurse.User,
                    IsRead = nurse.UserId == currentUser.Id,
                    ReadTime = nurse.UserId == currentUser.Id ? DateTime.UtcNow : (DateTime?)null
                };

                newMessage.Recipients.Add( messageRecipient );
            }
        }

        private static User GetPatient( 
            Message newMessage, ProactDatabaseContext database, User currentUser ) {
            User user = currentUser;

            var originalMessage = database.Messages.FirstOrDefault(
                    x => x.Id == newMessage.OriginalMessageId );

            if ( originalMessage != null ) { 
                user = originalMessage.Author;
            }

            return user;
        }

        private static void AddPatientRecipient( 
            Message newMessage, ProactDatabaseContext database, User currentUser ) {

            var patient = GetPatient( newMessage, database, currentUser );

            var patientRecipient = new MessageRecipient {
                MessageId = newMessage.Id,
                UserId = patient.Id,
                IsRead = true,
                ReadTime = DateTime.UtcNow,
                User = patient
            };

            newMessage.Recipients.Add( patientRecipient );
        }
    }
}
