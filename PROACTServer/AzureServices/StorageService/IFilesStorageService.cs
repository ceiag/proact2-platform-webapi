using Microsoft.AspNetCore.Http;
using Proact.Services.AzureMediaServices;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Proact.Services {
    public interface IFilesStorageService {
        public Task<MediaUploadedResultModel> UploadMediaFile(
            Stream fileStream, AccessFolderType accessType, MediaFileStoringInfoModel mediaFileModel );
        public Task<MediaUploadedResultModel> UploadFileDirectlyIntoContainer(
            Stream fileStream, AccessFolderType accessType,
            string contentType, string container, string filename );
        public Task<Stream> GetMediaFileStream( string storageName, string fileName );
        public Uri GetMediaFileSASUri( string storageName, string fileName, int durationInMinutes );
        public Task DeleteMediaFileIfExist( MediaFileStoringInfoModel mediaFileModel );
        public Task DeleteStorageIfExist( string storageName );
        public Task<bool> FileExist( string container, string filename );
    }
}
