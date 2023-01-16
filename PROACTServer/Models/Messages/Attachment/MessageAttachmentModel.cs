using Proact.Services.Entities;
using Proact.Services.Models.Messages;
using System;

namespace Proact.Services.Models {
    public class MessageAttachmentModel {
        public DateTime? UploadedTime { get; set; }
        public int DurationInMilliseconds { get; set; }
        public string Url { get; set; }
        public string ThumbnailUrl { get; set; }
        public string AssetId { get; set; }
        public MessageContentStatusEnum? ContentStatus { get; set; } = MessageContentStatusEnum.Uploading;
        public AttachmentType AttachmentType { get; set; }
    }
}
