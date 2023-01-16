using System;
using System.IO;
using System.Threading.Tasks;

namespace Proact.Services.AzureMediaServices {
    public interface IImageUploaderService {
        public Task<MediaUploadedResultModel> UploadImageToServer( 
            Guid userId, Stream fileStream );
        public Task<MediaUploadedResultModel> UploadImageToServer( 
            Guid userId, Stream fileStream, string fileName );
    }
}
