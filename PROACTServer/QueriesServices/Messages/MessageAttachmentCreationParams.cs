using Proact.Services.Entities;
using Proact.Services.Models.Messages;

namespace Proact.Services.Messages {
    public class MessageAttachmentCreationParams {
        public Message Message { get; set; }
        public MediaUploadedResultModel MediaUploadResult { get; set; }
        public AttachmentType AttachmentType { get; set; }
        public MessageContentStatusEnum AttachmentStatus { get; set; }
    }
}
