using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using Microsoft.AspNetCore.Http;
using Proact.Services.AzureMediaServices;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Proact.Services {
    public class AzureMediaStorageService : IFilesStorageService {
        public async Task<MediaUploadedResultModel> UploadMediaFile(
            Stream fileStream, AccessFolderType accessType, MediaFileStoringInfoModel mediaFileModel ) {

            await DeleteMediaFileIfExist( mediaFileModel );
            fileStream.Position = 0;

            var blobContainer 
                = await CreateBlobContainerIfNotExist( mediaFileModel.ContainerName, accessType );
            var uploadResult = await blobContainer.UploadBlobAsync( mediaFileModel.FileName, fileStream );

            if ( uploadResult.GetRawResponse().Status != 201 ) {
                throw new Exception( "CreateBlobContainerAsync return Status Code: "
                        + uploadResult.GetRawResponse().Status );
            }

            SetFileContentType( blobContainer, mediaFileModel.ContentType, mediaFileModel.FileName );
            return GetUploadedMediaUrl( blobContainer, mediaFileModel.FileName );
        }

        public async Task<MediaUploadedResultModel> UploadFileDirectlyIntoContainer(
            Stream fileStream, AccessFolderType accessType, 
            string contentType, string container, string filename ) {
            fileStream.Position = 0;

            var blobContainer
                = await CreateBlobContainerIfNotExist( container, accessType );
            var uploadResult = await blobContainer.UploadBlobAsync( filename, fileStream );

            if ( uploadResult.GetRawResponse().Status != 201 ) {
                throw new Exception( "CreateBlobContainerAsync return Status Code: "
                        + uploadResult.GetRawResponse().Status );
            }

            SetFileContentType( blobContainer, contentType, filename );
            return GetUploadedMediaUrl( blobContainer, filename );
        }

        private void SetFileContentType( 
            BlobContainerClient blobContainer, string contentType, string fileName ) {
            var blobClient = blobContainer.GetBlobClient( fileName );
           
            BlobHttpHeaders blobHttpHeaders = new BlobHttpHeaders();
            blobHttpHeaders.ContentType = contentType;

            blobClient.SetHttpHeaders( blobHttpHeaders );
        }

        public async Task<Stream> GetMediaFileStream( string storageName, string fileName ) {
            BlobServiceClient blobServiceClient = new BlobServiceClient(
                     AzureMediaServicesConfiguration.ConnectionString );

            var blobContainerClient = blobServiceClient.GetBlobContainerClient( storageName );
            var blobClient = blobContainerClient.GetBlobClient( fileName );
            var result = await blobClient.DownloadStreamingAsync();

            return ConvertBlobStreamToStream( result.Value.Content );
        }

        private MemoryStream ConvertBlobStreamToStream( Stream blobStream ) {
            var memoryStream = new MemoryStream();
            blobStream.CopyTo( memoryStream );
            memoryStream.Position = 0;

            return memoryStream;
        }

        public async Task DeleteStorageIfExist( string storageName ) {
            BlobServiceClient blobServiceClient = new BlobServiceClient(
                AzureMediaServicesConfiguration.ConnectionString );

            var blobContainerClient = blobServiceClient.GetBlobContainerClient( storageName );

            if ( await blobContainerClient.ExistsAsync() ) {
                await blobContainerClient.DeleteAsync();
            }
        }

        public async Task DeleteMediaFileIfExist( MediaFileStoringInfoModel mediaFileModel ) {
            BlobServiceClient blobServiceClient = new BlobServiceClient(
                AzureMediaServicesConfiguration.ConnectionString );

            var blobContainerClient = blobServiceClient.GetBlobContainerClient( mediaFileModel.ContainerName );
            var existingBlobClient = blobContainerClient.GetBlobClient( mediaFileModel.FileName );

            if ( await existingBlobClient.ExistsAsync() ) {
                await existingBlobClient.DeleteAsync();
            }
        }

        public async Task<bool> FileExist( string container, string filename ) {
            BlobServiceClient blobServiceClient = new BlobServiceClient(
                AzureMediaServicesConfiguration.ConnectionString );

            var blobContainerClient = blobServiceClient.GetBlobContainerClient( container );
            var existingBlobClient = blobContainerClient.GetBlobClient( filename );

            return await existingBlobClient.ExistsAsync();
        }

        private MediaUploadedResultModel GetUploadedMediaUrl(
            BlobContainerClient blobContainerClient, string fileName ) {
            var imgUrl = AzureMediaServicesConfiguration.MediaStorageUrl
                + blobContainerClient.Name + "/" + fileName;

            var mediaUploadedResult = new MediaUploadedResultModel() {
                ContentUrl = imgUrl,
                ThumbnailUrl = imgUrl,
                UploadOk = true,
                ContainerName = blobContainerClient.Name,
                FileName = fileName
            };

            return mediaUploadedResult;
        }

        public Uri GetMediaFileSASUri( string storageName, string fileName, int durationInMinutes ) {
            BlobServiceClient blobServiceClient = new BlobServiceClient(
                AzureMediaServicesConfiguration.ConnectionString );

            var blobContainerClient = blobServiceClient.GetBlobContainerClient( storageName );
            var blobClient = blobContainerClient.GetBlobClient( fileName );
            
            return blobClient.GenerateSasUri( 
                BlobSasPermissions.Read, DateTimeOffset.UtcNow.AddMinutes( durationInMinutes ) );
        }

        private async Task<BlobContainerClient> CreateBlobContainerIfNotExist(
            string storageName, AccessFolderType accessType ) {
            BlobServiceClient blobServiceClient = new BlobServiceClient(
                AzureMediaServicesConfiguration.ConnectionString );

            var blobContainer = blobServiceClient.GetBlobContainerClient( storageName );

            if ( !blobContainer.Exists() ) {
                PublicAccessType blobAccessType = PublicAccessType.None;

                if ( accessType == AccessFolderType.PUBLIC ) {
                    blobAccessType = PublicAccessType.Blob;
                }

                var blobContainerCreationResult = await blobServiceClient
                        .CreateBlobContainerAsync( storageName, blobAccessType );

                if ( blobContainerCreationResult.GetRawResponse().Status != 201 ) {
                    throw new Exception( "CreateBlobContainerAsync return Status Code: "
                        + blobContainerCreationResult.GetRawResponse().Status );
                }
            }

            return blobContainer;
        }
    }
}
