using Microsoft.EntityFrameworkCore;
using Proact.Services.Entities;
using Proact.Services.Entities.MessageAnalysis;
using System;
using System.Linq;

namespace Proact.Services {
    public static class MessageQueriesExtension {
        public static IQueryable<Message> IncludeMessagesCommonTables( this IQueryable<Message> rulesHelper ) {
            return rulesHelper
                .Include( x => x.Recipients )
                .Include( x => x.MessageAttachment )
                .Include( x => x.Author )
                .Include( x => x.MessageData )
                .Include( x => x.Analysis.Where( x => x.State == AnalysisState.Current ) )
                .Include( x => x.Replies
                    .Where( x => x.ReplyMessage.Show )
                    .OrderByDescending( x => x.Created ) )
                .Include( "Replies.ReplyMessage" )
                .Include( "Replies.ReplyMessage.MessageData" )
                .Include( "Replies.ReplyMessage.MessageAttachment" );
        }

        public static IQueryable<Message> WhereMessageIsOriginal( this IQueryable<Message> rulesHelper ) {
            return rulesHelper.Where( message => message.OriginalMessageId == Guid.Empty );
        }

        public static IQueryable<Message> WhereMessageIsForPatient( 
            this IQueryable<Message> rulesHelper, Patient patient ) {

            return rulesHelper.Where( x => x.Recipients.Any( p => p.UserId == patient.UserId ) );
        }

        public static IQueryable<Message> WhereMessageIsForMedicalTeam(
            this IQueryable<Message> rulesHelper, Guid medicalTeamId ) {
            return rulesHelper.Where( message => message.MedicalTeamId == medicalTeamId );
        }

        public static IQueryable<Message> WhereMessageIsShowable( this IQueryable<Message> rulesHelper ) {
            return rulesHelper.Where( message => message.Show );
        }

        public static IQueryable<Message> WhereMessageIsUnrepliedFromMedic( 
            this IQueryable<Message> rulesHelper ) {
            return rulesHelper
                .Where( message => message.Replies.Count == 0
                    || !message.Replies.Any( x => x.ReplyMessage.MessageType != MessageType.Patient ) )
                .Where( message => message.MessageType != MessageType.Broadcast );
        }

        public static IQueryable<Message> OrderByDescendingLastReply( this IQueryable<Message> rulesHelper ) {
            return rulesHelper.OrderByDescending( message => message.Replies.Count > 0 ?
                message.Replies.Max( x => x.Created ) : message.Created );
        }
    }
}
