using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Proact.Services.Tests.Shared {
    public class DatabaseProvider {
        private ProactDatabaseContext _database;

        public ProactDatabaseContext CreateDatabase() {
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            var builder = new DbContextOptionsBuilder<ProactDatabaseContext>()
                .UseInMemoryDatabase( databaseName: "mockdb" )
                .UseInternalServiceProvider( serviceProvider );

            _database = new ProactDatabaseContext( builder.Options );
            _database.SaveChanges();

            return _database;
        }
    }
}
