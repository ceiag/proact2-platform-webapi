using Proact.Services.Models.Messages;

namespace Proact.Services.Models {
    public class CreateAttachmentRequest {
        public int Milliseconds { get; set; }
        public AttachmentType AttachmentType { get; set; }
    }
}
