using Proact.EncryptionAgentService.Entities;
using Proact.EncryptionAgentService.Models;

namespace Proact.EncryptionAgentService {
    public static class MessagesMapper {
        public static MessageDataModel Map( MessageData messageDataEntity ) {
            var messageDataModel = new MessageDataModel() {
                Body = messageDataEntity.Body,
                Title = messageDataEntity.Title,
                Token = "",
                MessageId = messageDataEntity.Id
            };

            return messageDataModel;
        }

        public static MessageDataModel[] Map( MessageData[] messageDataEntities ) {
            var messagesDataModel = new MessageDataModel[messageDataEntities.Length];

            int i = 0;
            foreach ( var messageData in messageDataEntities ) {
                messagesDataModel[i] = Map( messageData );
                ++i;
            }

            return messagesDataModel;
        }
    }
}
