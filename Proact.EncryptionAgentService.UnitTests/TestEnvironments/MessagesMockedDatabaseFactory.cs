using Proact.EncryptionAgentService.Entities;
using System;

namespace Proact.EncryptionAgentService.UnitTests {
    public static class MessagesMockedDatabaseFactory {

        public static Guid[] MessagesIds = { 
            Guid.NewGuid(),
            Guid.NewGuid()
        };

        public static void AddMessagesDataToDatabase( ProactAgentDatabaseContext database ) {
            database.Messages.Add( new MessageData {
                Id = MessagesIds[0],
                Body = "message_0",
                Title = "title_0"
            } );

            database.Messages.Add( new MessageData {
                Id = MessagesIds[1],
                Body = "message_1",
                Title = "title_1"
            } );

            database.SaveChanges();
        }
    }
}
