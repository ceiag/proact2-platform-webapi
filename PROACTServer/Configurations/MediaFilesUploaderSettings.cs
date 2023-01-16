using Microsoft.Extensions.Configuration;

namespace Proact.Services {
    public static class MediaFilesUploaderSettings {
        private static IConfiguration _config;

        public static void Init( IConfiguration config ) {
            _config = config;
        }

        public static string FFMpegDebugBinaryFolder {
            get { return _config["FFmpeg:FFMpegDebugBinaryFolder"]; }
        }

        public static string FFMpegDebugTemporaryFilesFolder {
            get { return _config["FFmpeg:FFMpegDebugTemporaryFilesFolder"]; }
        }

        public static string FFMpegReleaseBinaryFolder {
            get { return _config["FFmpeg:FFMpegReleaseBinaryFolder"]; }
        }

        public static string FFMpegReleaseTemporaryFilesFolder {
            get { return _config["FFmpeg:FFMpegReleaseTemporaryFilesFolder"]; }
        }

        public static string VideoTempFolderName {
            get { return _config["FFmpeg:VideoTempFolderName"]; }
        }

        public static int SASUriDurationInMinutes {
            get { return int.Parse( _config["FFmpeg:SASUriDurationInMinutes"] ); }
        }

        public static string MediaFilesFolderPrefixName {
            get { return _config["FFmpeg:MediaFilesFolderPrefixName"]; }
        }

        public static string MediaFilesThumbsPrefixName {
            get { return _config["FFmpeg:MediaFilesThumbsPrefixName"]; }
        }

        public static string MediaFilesImagesPrefixName {
            get { return _config["FFmpeg:MediaFilesImagesPrefixName"]; }
        }

        public static string MediaVideoExtensionFormat {
            get { return _config["FFmpeg:MediaVideoExtensionFormat"]; }
        }

        public static string MediaAudioExtensionFormat {
            get { return _config["FFmpeg:MediaAudioExtensionFormat"]; }
        }

        public static string ImageExtensionFormat {
            get { return _config["FFmpeg:ImageExtensionFormat"]; }
        }

        public static string MediaVideoContentType {
            get { return _config["FFmpeg:MediaVideoContentType"]; }
        }

        public static string MediaAudioContentType {
            get { return _config["FFmpeg:MediaAudioContentType"]; }
        }

        public static string ImageContentType {
            get { return _config["FFmpeg:ImageContentType"]; }
        }

        public static string PdfContentType {
            get { return _config["Documents:PdfContentType"]; }
        }

        public static string PdfExtensionFormat {
            get { return _config["Documents:PdfExtensionFormat"]; }
        }

        public static string DocumentsPrefixName {
            get { return _config["Documents:DocumentsPrefixName"]; }
        }

        public static string ProtocolPrefixName {
            get { return _config["Documents:ProtocolPrefixName"]; }
        }
    }
}
