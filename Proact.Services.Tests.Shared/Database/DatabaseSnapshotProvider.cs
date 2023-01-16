using System;

namespace Proact.Services.Tests.Shared {
    public class DatabaseSnapshotProvider {
        private readonly ProactServicesProvider _serviceProvider;

        public ProactServicesProvider ServiceProvider {
            get { return _serviceProvider; }
        }

        public DatabaseSnapshotProvider( ProactServicesProvider serviceProvider ) {
            _serviceProvider = serviceProvider;
        }
    }
}
