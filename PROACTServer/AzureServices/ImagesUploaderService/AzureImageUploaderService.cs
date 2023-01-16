using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Proact.Services.AzureMediaServices {
    [Obsolete]
    public class AzureImageUploaderService : IImageUploaderService {

        private IFilesStorageService _mediaStorageService;

        public AzureImageUploaderService( IFilesStorageService mediaStorageService ) {
            _mediaStorageService = mediaStorageService;
        }

        private async Task SetContentTypeAsJpeg( string storageName, string fileName, Stream fileStream ) {
            BlobServiceClient blobServiceClient = new BlobServiceClient(
                AzureMediaServicesConfiguration.ConnectionString );

            var blobContainer = blobServiceClient.GetBlobContainerClient( storageName );
            var blobClient = blobContainer.GetBlobClient( fileName );

            BlobHttpHeaders blobHttpHeaders = new BlobHttpHeaders();
            blobHttpHeaders.ContentType = "image/jpg";

            blobClient.SetHttpHeaders( blobHttpHeaders );

            fileStream.Position = 0;
            await blobClient.UploadAsync( fileStream, blobHttpHeaders );
        }

        public async Task<MediaUploadedResultModel> UploadImageToServer( 
            Guid userId, Stream fileStream, string fileName ) {

            string storageName = "imgs-" + userId.ToString();

            //var uploadResult = await _mediaStorageService
            //    .UploadMediaFile( storageName, fileName, fileStream, AccessFolderType.PUBLIC, "image/jpg" );

            await SetContentTypeAsJpeg( storageName, fileName, fileStream );

            //return uploadResult;

            return null;
        }

        public async Task<MediaUploadedResultModel> UploadImageToServer( Guid userId, Stream fileStream ) {
            string imgName = GetNewImageName();

            return await UploadImageToServer( userId, fileStream, imgName );
        }

        private string GetNewImageName() {
            string imgName = "img-" + Guid.NewGuid().ToString() + ".jpg";
            return imgName;
        }
    }
}
