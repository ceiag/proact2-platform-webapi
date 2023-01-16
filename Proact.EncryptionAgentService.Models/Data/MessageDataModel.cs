using System;

namespace Proact.EncryptionAgentService.Models {
    public class MessageDataModel {
        public Guid MessageId { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }

        public string Token { get; set; }
    }
}
