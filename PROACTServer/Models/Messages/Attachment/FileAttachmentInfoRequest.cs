using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;

namespace Proact.Services.Models.Messages {
    public class CreateAttachMediaFileRequest {
        public string FileName { get; private set; }
        public string ContentType { get; private set; }
        public string Extension { get; private set; }
        public AttachmentType AttachmentType { get; private set; }
        private readonly List<string> _supportedTypes 
            = new List<string>() { 
                ".png", 
                ".jpg", 
                ".jpeg", 
                ".mp4", 
                ".mov", 
                ".mp3", 
                ".wav", 
                ".webm", 
                ".ogg" };

        public CreateAttachMediaFileRequest( IFormFile file, AttachmentType attachmentType ) {
            Extension = Path.GetExtension( file.FileName );
            FileName = file.FileName;
            ContentType = file.ContentType;
            AttachmentType = attachmentType;
            AssertAllowedExtensions();
        }

        private void AssertAllowedExtensions() {
            if ( !_supportedTypes.Contains( Extension ) ) {
                throw new Exception( $"file {FileName} with extension {Extension} not supported!" );
            }
        }
    }
}
