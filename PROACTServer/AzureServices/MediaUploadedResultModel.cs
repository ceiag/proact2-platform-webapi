using System;

namespace Proact.Services {
    public class MediaUploadedResultModel {
        public string ContentUrl { get; set; }
        public string ThumbnailUrl { get; set; }
        public string AssetId { get; set; }
        public string FileName { get; set; }
        public string ContainerName { get; set; }
        public double DurationInMilliseconds { get; set; } = 0.0f;
        public bool UploadOk { get; set; }
        public string ErrorMessage { get; set; }
        public string UploadTime { get; set; }
        public double ProcessingTime { get; set; }
    }
}
