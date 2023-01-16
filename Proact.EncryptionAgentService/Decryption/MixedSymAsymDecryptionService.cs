using Proact.EncryptionAgentService.Entities;
using Proact.EncryptionAgentService.InputEncryption;
using Proact.EncryptionAgentService.Models;

namespace Proact.EncryptionAgentService.Decryption {
    public class MixedSymAsymDecryptionService : IDecryptionService {
        public MessageData DecryptMessageData( 
            CreateMessageDataRequest encryptedMsgDataModel, MessageData messageData ) {
            var encryptor = new MixedSymAsymEncryptor(
                    encryptedMsgDataModel.EncryptedKey, encryptedMsgDataModel.EncryptedIV );

            messageData.Title = encryptor.DecryptAsString( encryptedMsgDataModel.EncryptedTitle );
            messageData.Body = encryptor.DecryptAsString( encryptedMsgDataModel.EncryptedBody );

            return messageData;
        }
    }
}
