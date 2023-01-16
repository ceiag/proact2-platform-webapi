using System;

namespace Proact.Services.Models.Messages {
    public class EditMessageRequest : MessageRequestData {
        public Guid MessageId { get; set; }
    }
}
