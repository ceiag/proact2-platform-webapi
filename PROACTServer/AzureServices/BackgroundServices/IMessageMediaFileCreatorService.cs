using Microsoft.AspNetCore.Http;
using Proact.Services.Entities;
using Proact.Services.Models;
using Proact.Services.Models.Messages;
using System;
using System.Threading.Tasks;

namespace Proact.Services {
    public interface IMessageMediaFileCreatorService {
        public void SetMediaUploaderMethod( IMediaFilesUploaderService mediaFilesUploaderService );
        public Task CreateMediaFile( Guid userId, Guid messageId, AttachmentType attachmentType );
        public Task CreateMediaFile(
            Guid userId, Guid messageId, IFormFile fileStream, AttachmentType attachmentType );
        public Task<MediaUploadedResultModel> UploadMediaFileForAsyncEncoding( Guid messageId, IFormFile mediaFile );
        public Task<MediaFileSecurityInfosModel> GetMediaFileSecurityInfos( MessageAttachment messageAttachment );
    }
}
