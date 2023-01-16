using Proact.Services.Models.Messages;
using System;
using System.IO;

namespace Proact.Services.AzureMediaServices {
    public class MediaFileStoringInfoModel {
        public Guid Uniqueness { get; set; }
        public string FileName { get; set; }
        public string ContainerName { get; set; }
        public string TempFileName { get; set; }
        public string TempMediaFilePath { get; set; }
        public string ThumbnailFileName { get; set; }
        public string TempFolderPath { get; set; }
        public string ContentType { get; set; }
        public AttachmentType AttachmentType { get; set; }
    }
}
