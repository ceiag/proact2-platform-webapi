using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.IO;

namespace Proact.Services.Utils {
    public static class FormFileToSreamSerializer {
        private static void CopyStream( Stream input, Stream output ) {
            byte[] buffer = new byte[16 * 1024];
            int read;

            while ( ( read = input.Read( buffer, 0, buffer.Length ) ) > 0 ) {
                output.Write( buffer, 0, read );
            }
        }

        public static string GetStreamSerializedMediaFile( IFormFile formFile ) {
            var fileAsMemoryStream = new MemoryStream();
            CopyStream( formFile.OpenReadStream(), fileAsMemoryStream );

            var mediaFileSerialized = JsonConvert.SerializeObject(
                fileAsMemoryStream, Formatting.Indented, new MemoryStreamJsonConverter() );

            return mediaFileSerialized;
        }
    }
}
