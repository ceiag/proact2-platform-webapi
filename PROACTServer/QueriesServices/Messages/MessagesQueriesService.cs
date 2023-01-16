using Proact.Services.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Proact.Services.QueriesServices {
    public class MessagesQueriesService : IMessagesQueriesService {
        private ProactDatabaseContext _database;
        private const int _pagingSize = 5;

        public MessagesQueriesService( ProactDatabaseContext database ) {
            _database = database;
        }

        private int SkipElements( int pagingCount ) {
            return pagingCount * _pagingSize;
        }

        public List<Message> SearchMessagesAsPatient( 
            Patient patient, string rawUrlWithSearchParams, int maxRows ) {
            return _database.Messages
                .IncludeMessagesCommonTables()
                .WhereMessageIsShowable()
                .WhereMessageIsForPatient( patient )
                .FilterByDateTimeRange( rawUrlWithSearchParams )
                .FilterByMessageContent( rawUrlWithSearchParams )
                .Where( x => x.MessageType == MessageType.Patient )
                .OrderByDescendingLastReply()
                .Take( maxRows )
                .ToList();
        }

        public List<Message> SearchMessagesAsMedic( 
            Guid medicalTeamId, string rawUrlWithSearchParams, int maxRows ) {
            return _database.Messages
                 .IncludeMessagesCommonTables()
                 .WhereMessageIsShowable()
                 .WhereMessageIsForMedicalTeam( medicalTeamId )
                 .FilterByDateTimeRange( rawUrlWithSearchParams )
                 .FilterByMessageContent( rawUrlWithSearchParams )
                 .OrderByDescendingLastReply()
                 .Take( maxRows )
                 .ToList();
        }

        public List<Message> GetMessagesAsPatient( Patient patient, int pagingCount ) {
            return _database.Messages
                .IncludeMessagesCommonTables()
                .WhereMessageIsOriginal()
                .WhereMessageIsForPatient( patient )
                .WhereMessageIsShowable()
                .OrderByDescendingLastReply()
                .Where( x => x.MessageType == MessageType.Patient )
                .Skip( SkipElements( pagingCount ) )
                .Take( _pagingSize )
                .ToList();
        }

        public List<Message> GetMessagesAsMedicUnreplied( Guid medicalTeamId, int pagingCount ) {
            return _database.Messages
                .IncludeMessagesCommonTables()
                .WhereMessageIsOriginal()
                .WhereMessageIsShowable()
                .WhereMessageIsForMedicalTeam( medicalTeamId )
                .WhereMessageIsUnrepliedFromMedic()
                .OrderByDescending( message => message.Created )
                .Skip( SkipElements( pagingCount ) )
                .Take( _pagingSize )
                .ToList();
        }

        public List<Message> GetMessagesAsMedic( MedicalTeam medicalTeam, int pagingCount ) {
            return _database.Messages
                .IncludeMessagesCommonTables()
                .WhereMessageIsOriginal()
                .WhereMessageIsForMedicalTeam( medicalTeam.Id )
                .WhereMessageIsShowable()
                .OrderByDescendingLastReply()
                .Skip( SkipElements( pagingCount ) )
                .Take( _pagingSize )
                .ToList();
        }

        public List<Message> GetAllMessagesForMedicalTeam( Guid medicalTeamId ) {
            return _database.Messages
                .IncludeMessagesCommonTables()
                .WhereMessageIsOriginal()
                .WhereMessageIsShowable()
                .OrderByDescendingLastReply()
                .Where( x => x.MedicalTeamId == medicalTeamId )
                .ToList();
        }

        public Message GetMessage( Guid messageId ) {
            return _database.Messages
                .IncludeMessagesCommonTables()
                .Where( message => message.Id == messageId )
                .FirstOrDefault();
        }

        public List<User> GetRecipients( Guid messageId ) {
            return GetMessage( messageId ).Recipients.Select( x => x.User ).ToList();
        }

        public List<User> GetRecipientsExceptUser( Guid userId, Guid messageId ) {
            return GetMessage( messageId ).Recipients
                .Where( x => x.UserId != userId ).Select( x => x.User ).ToList();
        }

        public void SetAsVisibile( Guid messageId ) {
            GetMessage( messageId ).Show = true;
        }

        public bool IsMessageAReplyToMyTopicMessage( Guid authorId, Message reply ) {
            var authorMessage = GetMessage( (Guid)reply.OriginalMessageId );

            if ( reply.IsStartingMessage ) {
                return false;
            }

            return reply.OriginalMessageId == authorMessage.Id;
        }

        public List<Message> GetAllMessagesWithAnalysisAsPatient( Guid userId ) {
            return _database.Messages
                .IncludeMessagesCommonTables()
                .Where( message => message.OriginalMessageId == Guid.Empty )
                .Where( message => message.AuthorId == userId )
                .ToList();
        }
    }
}
