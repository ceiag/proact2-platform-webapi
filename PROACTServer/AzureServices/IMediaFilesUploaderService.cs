using Proact.Services.AzureMediaServices;
using Proact.Services.Entities;
using Proact.Services.Models;
using Proact.Services.Models.Messages;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Proact.Services {
    public interface IMediaFilesUploaderService {
        public Task<MediaUploadedResultModel> UploadMediaFileOnStorage(
            Guid id, Stream fileStream, AttachmentType attachmentType );
        public Task<MediaFileSecurityInfosModel> GetMediaFileSecurityInfos( 
            MessageAttachment messageAttachment );
        public Task DeleteMediaFile( MediaFileStoringInfoModel fileInfo );
        public Task<MediaUploadedResultModel> UploadDocumentPdfIntoContainer(
            Stream imageStream, string fileName, Guid instituteId );
    }
}
