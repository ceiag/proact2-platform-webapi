using Proact.Services.Entities;
using Proact.Services.EntitiesMapper;
using Proact.Services.Models;
using System.Linq;

namespace Proact.Services.QueriesServices {
    public class MobileAppsInfoQueriesService : IMobileAppsInfoQueriesService {
        private readonly ProactDatabaseContext _database;

        public MobileAppsInfoQueriesService( ProactDatabaseContext database ) {
            _database = database;
        }

        public MobileAppsInfoModel Get() {
            return MobileAppsInfoEntityMapper.Map( _database.MobileAppsInfo.ToList()[0] );
        }

        private void DeleteLastEntry() {
            if ( _database.MobileAppsInfo.ToList().Count > 0 ) {
                var lastEntry = _database.MobileAppsInfo.ToList()[0];
                _database.MobileAppsInfo.Remove( lastEntry );
                _database.SaveChanges();
            }
        }

        public void Set( MobileAppsInfoCreationRequest request ) {
            DeleteLastEntry();

            _database.MobileAppsInfo.Add( new MobileAppsInfo() {
                AndroidLastBuildRequired = request.AndroidLastBuildRequired,
                AndroidStoreUrl = request.AndroidStoreUrl,
                iOSLastBuildRequired = request.iOSLastBuildRequired,
                iOSStoreUrl = request.iOSStoreUrl
            } );

            _database.SaveChanges();
        }
    }
}
