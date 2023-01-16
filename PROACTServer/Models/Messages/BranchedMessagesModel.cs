using System.Collections.Generic;

namespace Proact.Services.Models {
    public class BranchedMessagesModel {
        public MessageModel OriginalMessage { get; set; }
        public List<MessageModel> ReplyMessages { get; set; }
        public int ReplyMessagesCount { get; set; }
    }
}
