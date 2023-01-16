using Proact.Services.Models.Stats;
using System;

namespace Proact.Services.QueriesServices.Stats {
    public interface IMessagesStatsProviderService : IDataEditorService {
        public MessagesStatsModel GetMessagesStats();
        public MessagesStatsModel GetMessagesStatsForInstitute( Guid instituteId );
        public MessagesStatsModel GetMessagesStatsForProject( Guid projectId );
    }
}
