using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using Microsoft.Azure.Management.Media;
using Microsoft.Azure.Management.Media.Models;
using Newtonsoft.Json;
using Proact.Services.Entities;
using Proact.Services.Models;
using Proact.Services.Models.Messages;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace Proact.Services.AzureMediaServices {
    public class AzureMediaServiceUploaderService : IMediaFilesUploaderService {
        private const string _h264transform = "Custom_H264_3Layer";
        private IAzureMediaServicesClient _azureMediaServiceClient;
        private readonly IAzureMediaEncryptionService _azureMediaEncryptionService;
        private readonly IFilesStorageService _fileStorageService;

        public AzureMediaServiceUploaderService(
            IAzureMediaEncryptionService azureMediaEncryptionService,
            IFilesStorageService filesStorageService ) {
            _azureMediaEncryptionService = azureMediaEncryptionService;
            _fileStorageService = filesStorageService;
        }

        public async Task<MediaUploadedResultModel> UploadMediaFileOnStorage(
            Guid userId, Stream fileStream, AttachmentType attachmentType ) {
            return await PerformUploadMediaToServer( userId, fileStream, attachmentType );
        }

        private async Task<MediaUploadedResultModel> PerformUploadMediaToServer(
            Guid userId, Stream fileStream, AttachmentType attachmentType ) {
            return await UploadVideoAudioToServer( userId, fileStream, attachmentType );
        }

        private async Task<MediaUploadedResultModel> UploadVideoAudioToServer(
            Guid userId, Stream fileStream, AttachmentType attachmentType ) {
            string uniqueness = Guid.NewGuid().ToString( "N" );
            string inputAssetName = $"input-{uniqueness}";
            string blobClientName = $"blobclient-{uniqueness}";
            string jobName = $"job-{uniqueness}";
            string locatorName = $"locator-{uniqueness}";

            await InitAzureMediaService();

            var transform = await GetOrCreateTransformAsync(
                attachmentType,
                AzureMediaServicesConfiguration.ResourceGroup,
                AzureMediaServicesConfiguration.AccountName,
                _h264transform );

            var inputAsset = await CreateInputAssetAsync(
                AzureMediaServicesConfiguration.ResourceGroup,
                AzureMediaServicesConfiguration.AccountName,
                inputAssetName, blobClientName, fileStream );

            var outputAsset = await CreateOutputAssetAsync(
                AzureMediaServicesConfiguration.ResourceGroup,
                AzureMediaServicesConfiguration.AccountName,
                inputAssetName );

            var submitJobResult = await SubmitJobAsync(
                AzureMediaServicesConfiguration.ResourceGroup,
                AzureMediaServicesConfiguration.AccountName,
                _h264transform, jobName, inputAsset.Name, outputAsset.Name );

            var waitJobToFinishResult = await WaitForJobToFinishAsync(
                AzureMediaServicesConfiguration.ResourceGroup,
                AzureMediaServicesConfiguration.AccountName,
                _h264transform, submitJobResult.Name );

            var streamingLocator = await _azureMediaEncryptionService
                .CreateStreamingLocatorAsync( _azureMediaServiceClient, outputAsset.Name, locatorName );

            var mediaUploadedResultModel = new MediaUploadedResultModel() {
                AssetId = uniqueness,
                UploadOk = true,
            };

            await FillMediaUploadedResult( userId,
                AzureMediaServicesConfiguration.ResourceGroup,
                AzureMediaServicesConfiguration.AccountName,
                outputAsset.Name, mediaUploadedResultModel );

            mediaUploadedResultModel.UploadOk = true;

            await _azureMediaServiceClient.Assets.DeleteAsync(
                AzureMediaServicesConfiguration.ResourceGroup,
                AzureMediaServicesConfiguration.AccountName,
                inputAsset.Name );

            return mediaUploadedResultModel;
        }

        public async Task<MediaFileSecurityInfosModel> GetMediaFileSecurityInfos(
            MessageAttachment messageAttachment ) {
            await InitAzureMediaService();

            BlobServiceClient blobServiceClient = new BlobServiceClient(
                AzureMediaServicesConfiguration.ConnectionString );

            var blobContainerClient = blobServiceClient
                .GetBlobContainerClient( messageAttachment.ContainerName );
            var blobClient = blobContainerClient.GetBlobClient( messageAttachment.FileName );

            var sasUri = blobClient.GenerateSasUri(
                BlobSasPermissions.Read, DateTimeOffset.UtcNow.AddMinutes( 60 ) );

            return new MediaFileSecurityInfosModel() {
                ContentUrl = sasUri.ToString()
            };
        }

        private async Task InitAzureMediaService() {
            _azureMediaServiceClient = await AzureMediaServiceAuthHelper.CreateMediaServicesClientAsync();
        }

        public async Task<Asset> CreateInputAssetAsync(
            string resourceGroupName, string accountName,
            string assetName, string blobClientName, Stream fileStream ) {

            Asset asset = await _azureMediaServiceClient.Assets
                .CreateOrUpdateAsync( resourceGroupName, accountName, assetName, new Asset() );

            var assetContainerSas = await _azureMediaServiceClient.Assets
                .ListContainerSasAsync(
                    resourceGroupName, accountName, assetName,
                    permissions: AssetContainerPermission.ReadWrite,
                    expiryTime: DateTime.UtcNow.AddHours( 4 ).ToUniversalTime() );

            var sasUri = new Uri( assetContainerSas.AssetContainerSasUrls.First() );
            await CreateBlobContainer( blobClientName, sasUri, fileStream );

            return asset;
        }

        private async Task CreateBlobContainer(
            string blobClientName, Uri sasUri, Stream fileStream ) {
            var blobContainerClient = new BlobContainerClient( sasUri );
            BlobClient blobClient = blobContainerClient
                .GetBlobClient( blobClientName );

            await blobClient.UploadAsync( fileStream );
        }

        public async Task<Asset> CreateOutputAssetAsync(
            string resouceGroupName, string accountName, string assetName ) {

            Asset outputAsset = await _azureMediaServiceClient.Assets
                .GetAsync( resouceGroupName, accountName, assetName );

            if ( outputAsset != null ) {
                string uniqueness = $"-{Guid.NewGuid():N}";
                assetName += uniqueness;
            }

            Asset assetParameters = new Asset();

            var createdAsset = await _azureMediaServiceClient.Assets
                .CreateOrUpdateAsync( resouceGroupName, accountName, assetName, assetParameters );

            return createdAsset;
        }

        public async Task<Transform> GetOrCreateTransformAsync(
            AttachmentType attachmentType, string resourceGroupName,
            string accountName, string transformName ) {
            Transform transform = null;

            try {
                 transform = await _azureMediaServiceClient
                    .Transforms
                    .GetAsync(
                        AzureMediaServicesConfiguration.ResourceGroup,
                        AzureMediaServicesConfiguration.AccountName,
                        transformName );
            }
            catch ( Exception e ) {}

            if ( transform is null ) {
                var transformOutput = AzureMCTransformOutputFactory.GetTransformOutputStandardEncoder();

                if ( attachmentType == AttachmentType.VIDEO ) {
                    transformOutput = AzureMCTransformOutputFactory.GetTransformOutputWithThumbnails();
                }

                TransformOutput[] transformOutputs = new TransformOutput[] {
                    transformOutput
                };

                transform = await _azureMediaServiceClient
                        .Transforms
                        .CreateOrUpdateAsync(
                            resourceGroupName,
                            accountName,
                            transformName,
                            transformOutputs );
            }

            return transform;
        }

        public async Task<Job> SubmitJobAsync(
            string resourceGroupName, string accountName, string transformName,
            string jobName, string inputAssetName, string outputAssetName ) {

            try {
                JobInput jobInput = new JobInputAsset( assetName: inputAssetName );

                JobOutput[] jobOutputs = {
                    new JobOutputAsset( outputAssetName )
                };

                Job job = await _azureMediaServiceClient.Jobs.CreateAsync(
                    resourceGroupName, accountName, transformName, jobName,
                    new Job() {
                        Input = jobInput,
                        Outputs = jobOutputs
                    } );

                return job;
            }
            catch ( Exception e ) {
                int a = 0;
            }

            return null;
        }

        public async Task<Job> WaitForJobToFinishAsync(
            string resourceGroupName, string accountName, string transformName, string jobName ) {

            const int sleepIntervalsMs = 20 * 100;

            Job job;

            do {
                job = await _azureMediaServiceClient.Jobs
                    .GetAsync( resourceGroupName, accountName, transformName, jobName );

                if ( job.State != JobState.Finished
                        && job.State != JobState.Error
                            && job.State != JobState.Canceled ) {
                    await Task.Delay( sleepIntervalsMs );
                }
            }
            while ( job.State != JobState.Finished
                        && job.State != JobState.Error
                            && job.State != JobState.Canceled );

            return job;
        }

        private async Task<BlobContainerClient> CreateBlobThumbnailsContainerIfNotExist( Guid userId ) {
            string containerName = "thumbnails-" + userId.ToString();

            BlobServiceClient blobServiceClient = new BlobServiceClient(
                AzureMediaServicesConfiguration.ConnectionString );

            var thumbnailsBlobContainer
                = blobServiceClient.GetBlobContainerClient( containerName );

            if ( !thumbnailsBlobContainer.Exists() ) {
                var blobContainerCreationResult = await blobServiceClient
                    .CreateBlobContainerAsync( containerName, PublicAccessType.Blob );

                if ( blobContainerCreationResult.GetRawResponse().Status != 201 ) {
                    throw new Exception( "CreateBlobContainerAsync return Status Code: "
                        + blobContainerCreationResult.GetRawResponse().Status );
                }
            }

            return thumbnailsBlobContainer;
        }

        private async Task<string> MoveAndGetThumbnailToContainer(
            Guid userId, Uri sasBlobContainerUri, BlobItem blobSourceItem ) {

            var blobContainerClient = new BlobContainerClient( sasBlobContainerUri );
            var thumbnailsBlobContainer = await CreateBlobThumbnailsContainerIfNotExist( userId );

            BlobClient sourceBlob = blobContainerClient.GetBlobClient( blobSourceItem.Name );
            BlobClient destBlob = thumbnailsBlobContainer.GetBlobClient( blobSourceItem.Name );

            await destBlob.StartCopyFromUriAsync( sourceBlob.Uri );
            await sourceBlob.DeleteAsync();

            return destBlob.Uri.ToString();
        }

        private async Task<double> GetVideoDurationInMilliseconds(
            BlobContainerClient blobContainerClient, BlobItem blobItem ) {
            BlobClient blobClient = blobContainerClient.GetBlobClient( blobItem.Name );

            var content = await blobClient.DownloadContentAsync();

            var azureAssetFileModel = JsonConvert
                .DeserializeObject<AzureAssetFileModel>( content.Value.Content.ToString() );

            string duration = azureAssetFileModel.AssetFile[0].Duration;

            return XmlConvert.ToTimeSpan( duration ).TotalMilliseconds;
        }

        //Todo: refator this
        private async Task FillMediaUploadedResult(
            Guid userId, string resourceGroupName, string accountName, string assetName,
            MediaUploadedResultModel mediaUploadedResultModel ) {

            var response = await _azureMediaServiceClient.Assets
                    .ListContainerSasAsync(
                        resourceGroupName, accountName, assetName,
                        permissions: AssetContainerPermission.ReadWriteDelete,
                        expiryTime: DateTime.UtcNow.AddHours( 4 ).ToUniversalTime() );
            var sasUri = new Uri( response.AssetContainerSasUrls.First() );

            BlobContainerClient blobContainerClient = new BlobContainerClient( sasUri );
            var blobs = blobContainerClient.GetBlobsAsync().AsPages( default, 100 );

            await foreach ( var blobPage in blobs ) {
                foreach ( BlobItem blobItem in blobPage.Values ) {

                    if ( blobItem.Name.Contains( "manifest" ) ) {
                        mediaUploadedResultModel.DurationInMilliseconds
                            = await GetVideoDurationInMilliseconds( blobContainerClient, blobItem );
                    }

                    if ( blobItem.Properties.ContentType == "image/jpeg" ) {
                        string url = await MoveAndGetThumbnailToContainer( userId, sasUri, blobItem );
                        mediaUploadedResultModel.ThumbnailUrl = url;
                    }

                    if ( blobItem.Properties.ContentType == "video/mp4" ) {
                        mediaUploadedResultModel.FileName = blobItem.Name;
                        mediaUploadedResultModel.ContainerName = blobContainerClient.Name;
                    }
                }
            }
        }

        public async Task DeleteMediaFile( MediaFileStoringInfoModel fileInfo ) {
            await _fileStorageService.DeleteMediaFileIfExist( fileInfo );
        }

        public async Task<MediaUploadedResultModel> UploadDocumentPdfIntoContainer(
            Stream imageStream, string fileName, Guid instituteId ) {
            var mediaFileInfos = MediaFileUploaderNamingResolver
                .CreateMediaFileNamingForDocumentPdf( fileName, instituteId );

            return await _fileStorageService.UploadMediaFile(
                imageStream, AccessFolderType.PUBLIC, mediaFileInfos );
        }
    }
}
