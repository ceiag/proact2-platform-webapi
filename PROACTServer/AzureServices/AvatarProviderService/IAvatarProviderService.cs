using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Proact.Services {
    public interface IAvatarProviderService {
        public Task<MediaUploadedResultModel> UploadAvatar( Guid userId, IFormFile imageStream );
    }
}
