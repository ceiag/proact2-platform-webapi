using System;

namespace Proact.Services.QueriesServices {
    public class ChangesTrackingService : IChangesTrackingService {
        private ProactDatabaseContext _database;
        private IUserQueriesService _usersQueriesService;

        public ChangesTrackingService( 
            ProactDatabaseContext database, IUserQueriesService usersQueriesService ) {
            _database = database;
            _usersQueriesService = usersQueriesService;
        }

        public void SaveChanges( string accountId ) {
            _database.SaveChangesWithEntityTracking( 
                _usersQueriesService.GetByAccountId( accountId ).Id );
        }
    }
}
