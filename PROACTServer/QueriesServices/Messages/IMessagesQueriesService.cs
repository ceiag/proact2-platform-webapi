using Proact.Services.Entities;
using System;
using System.Collections.Generic;

namespace Proact.Services.QueriesServices {
    public interface IMessagesQueriesService : IQueriesService {
        public List<Message> GetMessagesAsPatient( Patient patient, int pagingCount );
        public List<Message> GetAllMessagesWithAnalysisAsPatient( Guid userId );
        public List<Message> SearchMessagesAsPatient( 
            Patient patient, string rawUrlWithSearchParams, int maxRows );
        public List<Message> SearchMessagesAsMedic( 
            Guid medicalTeamId, string rawUrlWithSearchParams, int maxRows );
        public List<Message> GetMessagesAsMedicUnreplied( Guid medicalTeamId, int pagingCount );
        public List<Message> GetMessagesAsMedic( MedicalTeam medicalTeam, int pagingCount );
        public List<Message> GetAllMessagesForMedicalTeam( Guid medicalTeamId );
        public Message GetMessage( Guid messageId );
        public List<User> GetRecipients( Guid messageId );
        public List<User> GetRecipientsExceptUser( Guid userId, Guid messageId );
        public bool IsMessageAReplyToMyTopicMessage( Guid authorId, Message message );
        public void SetAsVisibile( Guid messageId );
    }
}
