using Microsoft.AspNetCore.Http;
using Proact.Services.AzureMediaServices;
using Proact.Services.Utils;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Proact.Services {
    public class AvatarProviderService : IAvatarProviderService {
        private IFilesStorageService _fileStorageService;
        private const int _avatarPixelSize = 512;
        private const int _avatarQuality = 80;

        public AvatarProviderService( IFilesStorageService filesStorageService ) {
            _fileStorageService = filesStorageService;
        }

        private Stream GetResizedImageAvatar( IFormFile imageStream ) {
            return ImageHelper.ResizeImage(
                    imageStream.OpenReadStream(), _avatarPixelSize, _avatarQuality );
        }

        public async Task<MediaUploadedResultModel> UploadAvatar( Guid userId, IFormFile imageStream ) {
            var avatarFileInfos = MediaFileUploaderNamingResolver.CreateMediaFileNamingForImage( userId );

            return await _fileStorageService.UploadMediaFile( 
                GetResizedImageAvatar( imageStream ), AccessFolderType.PUBLIC, avatarFileInfos );
        }
    }
}
