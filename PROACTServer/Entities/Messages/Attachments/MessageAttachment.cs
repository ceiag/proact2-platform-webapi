using Proact.Services.Models.Messages;
using System;

namespace Proact.Services.Entities {
    public class MessageAttachment : TrackableEntity, IEntity {
        public DateTime? UploadedTime { get; set; }
        public double DurationInMilliseconds { get; set; }
        public string ContentUrl { get; set; }
        public string ThumbnailUrl { get; set; }
        public string AssetId { get; set; } = Guid.Empty.ToString();
        public string FileName { get; set; }
        public string ContainerName { get; set; }
        public MessageContentStatusEnum? ContentStatus { get; set; }
        public AttachmentType AttachmentType { get; set; }
        public virtual Guid MessageId { get; set; }
        public virtual Message Message { get; set; }
    }
}
