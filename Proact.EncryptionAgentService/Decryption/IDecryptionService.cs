using Proact.EncryptionAgentService.Entities;
using Proact.EncryptionAgentService.Models;

namespace Proact.EncryptionAgentService.Decryption {
    public interface IDecryptionService {
        MessageData DecryptMessageData( 
            CreateMessageDataRequest encryptedMsgDataModel, MessageData messageData );
    }
}
