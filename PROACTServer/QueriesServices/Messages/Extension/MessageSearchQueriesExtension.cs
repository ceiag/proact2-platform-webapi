using Proact.Services.Entities;
using System;
using System.Linq;
using System.Web;

namespace Proact.Services {
    public static class MessageSearchQueriesExtension {
        private static readonly string _messageSearchKey = "message";
        private static readonly string _fromDateSearchKey = "fromdate";
        private static readonly string _toDateSearchKey = "todate";

        private static string GetParamContent( string content, string key ) {
            return HttpUtility.ParseQueryString( content ).Get( key );
        }

        public static IQueryable<Message> FilterByMessageContent(
            this IQueryable<Message> rulesHelper, string queryParams ) {
            var messageContent = GetParamContent( queryParams, _messageSearchKey );

            if ( !string.IsNullOrEmpty( messageContent ) ) {
                return rulesHelper.Where( x => x.MessageData.Body.Contains( messageContent ) );
            }
            else {
                return rulesHelper;
            }
        }

        public static IQueryable<Message> FilterByDateTimeRange(
            this IQueryable<Message> rulesHelper, string queryParams ) {
            var fromDateContent = GetParamContent( queryParams, _fromDateSearchKey );
            var toDateContent = GetParamContent( queryParams, _toDateSearchKey );

            var fromDate = DateTime.MinValue;
            var toDate = DateTime.MaxValue;

            if ( !string.IsNullOrEmpty( fromDateContent ) ) {
                fromDate = DateTime.Parse( fromDateContent );
            }

            if ( !string.IsNullOrEmpty( toDateContent ) ) {
                toDate = DateTime.Parse( toDateContent );
            }

            return rulesHelper.Where( x => x.Created >= fromDate && x.Created <= toDate );
        }
    }
}
