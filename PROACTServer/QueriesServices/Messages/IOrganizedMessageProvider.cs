using Proact.Services.Entities;
using Proact.Services.Models;
using Proact.Services.QueriesServices;
using System.Collections.Generic;

namespace Proact.Services.Messages {
    public interface IOrganizedMessagesProvider : IDataEditorService {
        public List<BranchedMessagesModel> GetBranchedMessages( 
            List<Message> messages, int limitRepliesCount );
        public BranchedMessagesModel GetBranchedMessage( Message message, int limitRepliesCount );
        public List<MessageModel> GetAsMonodimensionalList( List<Message> messages );
        public MessageModel AdaptToBroadcastMessage( MessageModel message );
    }
}