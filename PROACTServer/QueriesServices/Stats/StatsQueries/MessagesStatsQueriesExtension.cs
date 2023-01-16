using Microsoft.EntityFrameworkCore;
using Proact.Services.Entities;
using Proact.Services.Models;
using Proact.Services.Models.Messages;
using System;
using System.Linq;

namespace Proact.Services.QueriesServices.Stats.StatsQueries {
    public static class MessagesStatsQueriesExtension {
        public static IQueryable<Message> Topics( this IQueryable<Message> query ) {
            return query
                .Include( x => x.MedicalTeam )
                .Include( x => x.MessageData )
                .Include( x => x.Author )
                .Include( x => x.MessageAttachment )
                .Where( x => x.OriginalMessageId == Guid.Empty )
                .Where( x => x.Show );
        }

        public static IQueryable<Message> Replies( this IQueryable<Message> query ) {
            return query
                .Include( x => x.MedicalTeam )
                .Include( x => x.MessageData )
                .Include( x => x.Author )
                .Include( x => x.MessageAttachment )
                .Where( x => x.OriginalMessageId != Guid.Empty )
                .Where( x => x.Show );
        }

        public static IQueryable<Message> Unreplied( this IQueryable<Message> query ) {
            return query.Where( x => x.Replies.Count() == 0 );
        }

        public static IQueryable<Message> WithMessageScope( 
            this IQueryable<Message> query, MessageScope scope ) {
            return query.Where( x => x.MessageScope == scope );
        }

        public static IQueryable<Message> WithPatientMood(
            this IQueryable<Message> query, PatientMood mood ) {
            return query.Where( x => x.Emotion == mood );
        }

        public static IQueryable<Message> FromPatients( this IQueryable<Message> query ) {
            return query.Where( x => x.MessageType == MessageType.Patient );
        }

        public static IQueryable<Message> FromMedics( this IQueryable<Message> query ) {
            return query.Where( x => x.MessageType == MessageType.Medic );
        }

        public static IQueryable<Message> FromNurses( this IQueryable<Message> query ) {
            return query.Where( x => x.MessageType == MessageType.Nurse );
        }

        public static IQueryable<Message> FromInstitute( 
            this IQueryable<Message> query,Guid instituteId ) {
            return query.Where( x => x.Author.InstituteId == instituteId );
        }

        public static IQueryable<Message> FromProject(
            this IQueryable<Message> query, Guid projectId ) {
            return query.Where( x => x.MedicalTeam.ProjectId == projectId );
        }

        public static IQueryable<Message> TextOnly( this IQueryable<Message> query ) {
            return query.Where( x => x.MessageAttachment == null );
        }

        public static IQueryable<Message> WithVideo( this IQueryable<Message> query ) {
            return query
                .Where( x => x.MessageAttachment != null )
                .Where( x => x.MessageAttachment.AttachmentType == AttachmentType.VIDEO );
        }

        public static IQueryable<Message> WithAudio( this IQueryable<Message> query ) {
            return query
                .Where( x => x.MessageAttachment != null )
                .Where( x => x.MessageAttachment.AttachmentType == AttachmentType.AUDIO );
        }

        public static IQueryable<Message> WithImage( this IQueryable<Message> query ) {
            return query
                .Where( x => x.MessageAttachment != null )
                .Where( x => x.MessageAttachment.AttachmentType == AttachmentType.IMAGE );
        }

        public static double AvgDuration( this IQueryable<Message> query ) {
            if ( query.Count() == 0 ) {
                return 0.0f;
            }

            return query.Average( x => x.MessageAttachment.DurationInMilliseconds );
        }

        public static double AvgTextLength( this IQueryable<Message> query ) {
            if ( query.Count() == 0 ) {
                return 0.0f;
            }

            return query.Average( x => x.MessageData.Body.Length );
        }
    }
}
