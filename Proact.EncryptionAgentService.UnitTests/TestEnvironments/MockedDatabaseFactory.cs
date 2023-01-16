using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Proact.EncryptionAgentService.Configurations;

namespace Proact.EncryptionAgentService.UnitTests {
    public static class MockedDatabaseFactory {
        public static DbContextOptions<ProactAgentDatabaseContext> CreateNewContextOptions() {
            AutoMapperConfiguration.Configure();

            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            var builder = new DbContextOptionsBuilder<ProactAgentDatabaseContext>()
                .UseInMemoryDatabase( databaseName: "mockdb" )
                .UseInternalServiceProvider( serviceProvider );

            return builder.Options;
        }

        public static ProactAgentDatabaseContext GetMockedDatabase() {
            var database = new ProactAgentDatabaseContext( CreateNewContextOptions() );

            MessagesMockedDatabaseFactory.AddMessagesDataToDatabase( database );

            return database;
        }
    }
}
