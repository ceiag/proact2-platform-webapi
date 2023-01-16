using Moq;
using Proact.EncryptionAgentService.Controllers;
using Proact.EncryptionAgentService.Decryption;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proact.EncryptionAgentService.UnitTests.Messages {
    public abstract class MessagesDataController_UnitTest {
        protected MessageDataController GetMockedMessageDataController( 
            ProactAgentDatabaseContext database ) {
            var decryptionService = new MixedSymAsymDecryptionService();
            var messageDataController
                = new MessageDataController( database, decryptionService );

            return messageDataController;
        }
    }
}
