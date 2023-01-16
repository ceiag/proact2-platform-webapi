using Microsoft.EntityFrameworkCore;
using Proact.EncryptionAgentService.Entities;

namespace Proact.EncryptionAgentService {
    public class ProactAgentDatabaseContext : DbContext {
        public DbSet<MessageData> Messages { get; set; }

        public ProactAgentDatabaseContext(
            DbContextOptions<ProactAgentDatabaseContext> options ) : base( options ) {
        }
    }
}
