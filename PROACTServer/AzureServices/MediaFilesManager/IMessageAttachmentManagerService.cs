using Microsoft.AspNetCore.Http;
using Proact.Services.AuthorizationPolicies;
using Proact.Services.Entities;
using Proact.Services.Models;
using Proact.Services.Models.Messages;
using System;
using System.Threading.Tasks;

namespace Proact.Services {
    public interface IMessageAttachmentManagerService {
        public Task<MediaUploadedResultModel> AttachAudioFileFromTempFolderToMessage( 
            Message message, CreateAttachMediaFileRequest request );
        public Task AttachVideoFileFromTempFolderToMessage( Guid messageId );
        public Task<MediaUploadedResultModel> AttachImageFileToMessage(
            Message message, IFormFile mediaFile );
        public Task UploadMediaFileOnTempFolder( IFormFile file, Message message, AttachmentType type );
        public Task<MediaFileSecurityInfosModel> GetAttachmentUrlWithSASToken( MessageAttachment attachment );
    }
}
