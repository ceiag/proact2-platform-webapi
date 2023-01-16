using Proact.Services.Models.Messages;
using System;
using System.IO;

namespace Proact.Services.AzureMediaServices {
    public static class MediaFileUploaderNamingResolver {
        public static string GetMediaContainerName( Guid userId ) {
            return $"{MediaFilesUploaderSettings.MediaFilesFolderPrefixName}{userId}";
        }

        public static string GetFileNameForAudio( Guid assetId, string extension ) {
            return $"{assetId}{extension}";
        }

        public static MediaFileStoringInfoModel CreateMediaFileNamingForVideo( Guid userId ) {
            Guid uniqueness = Guid.NewGuid();
            string tempFileName = $"{uniqueness}-temp.{MediaFilesUploaderSettings.MediaVideoExtensionFormat}";

            return new MediaFileStoringInfoModel() {
                AttachmentType = AttachmentType.VIDEO,
                ContainerName = GetMediaContainerName( userId ),
                FileName = $"{uniqueness}{MediaFilesUploaderSettings.MediaVideoExtensionFormat}",
                ContentType = MediaFilesUploaderSettings.MediaVideoContentType,
                Uniqueness = uniqueness,
                TempFileName = tempFileName,
                TempMediaFilePath = GetPathForTempMediaFiles( tempFileName ),
                ThumbnailFileName = $"{uniqueness}{MediaFilesUploaderSettings.ImageExtensionFormat}",
                TempFolderPath = GetTempPath()
            };
        }

        public static MediaFileStoringInfoModel CreateMediaFileNamingForAudio( Guid userId ) {
            Guid uniqueness = Guid.NewGuid();
            string tempFileName = $"{uniqueness}-temp{MediaFilesUploaderSettings.MediaAudioExtensionFormat}";

            return new MediaFileStoringInfoModel() {
                AttachmentType = AttachmentType.AUDIO,
                ContainerName = GetMediaContainerName( userId ),
                FileName = $"{uniqueness}{MediaFilesUploaderSettings.MediaAudioExtensionFormat}",
                ContentType = MediaFilesUploaderSettings.MediaAudioContentType,
                Uniqueness = uniqueness,
                TempFileName = tempFileName,
                TempMediaFilePath = GetPathForTempMediaFiles( tempFileName ),
                TempFolderPath = GetTempPath()
            };
        }

        public static MediaFileStoringInfoModel CreateMediaFileNamingForImage( Guid userId ) {
            Guid uniqueness = Guid.NewGuid();
            
            return new MediaFileStoringInfoModel() {
                AttachmentType = AttachmentType.IMAGE,
                ContainerName = $"{MediaFilesUploaderSettings.MediaFilesImagesPrefixName}{userId}",
                FileName = $"{uniqueness}{MediaFilesUploaderSettings.ImageExtensionFormat}",
                ContentType = MediaFilesUploaderSettings.ImageContentType,
                Uniqueness = uniqueness,
                TempFileName = string.Empty,
                TempMediaFilePath = string.Empty,
                ThumbnailFileName = string.Empty
            };
        }

        public static MediaFileStoringInfoModel CreateMediaFileNamingForThumbnail( Guid userId ) {
            Guid uniqueness = Guid.NewGuid();

            return new MediaFileStoringInfoModel() {
                AttachmentType = AttachmentType.IMAGE,
                ContainerName = $"{MediaFilesUploaderSettings.MediaFilesThumbsPrefixName}{userId}",
                FileName = $"{uniqueness}{MediaFilesUploaderSettings.ImageExtensionFormat}",
                ContentType = MediaFilesUploaderSettings.ImageContentType,
                Uniqueness = uniqueness,
                TempFileName = string.Empty,
                TempMediaFilePath = string.Empty,
                ThumbnailFileName = string.Empty
            };
        }

        public static MediaFileStoringInfoModel CreateMediaFileNamingForDocumentPdf( 
            string fileName, Guid instituteId ) {
            return new MediaFileStoringInfoModel() {
                AttachmentType = AttachmentType.DOCUMENT_PDF,
                ContainerName = $"{MediaFilesUploaderSettings.DocumentsPrefixName}{instituteId}",
                FileName = $"{fileName}{MediaFilesUploaderSettings.PdfExtensionFormat}",
                ContentType = MediaFilesUploaderSettings.PdfContentType,
                Uniqueness = Guid.Empty,
                TempFileName = string.Empty,
                TempMediaFilePath = string.Empty,
                ThumbnailFileName = string.Empty
            };
        }

        public static string GetTempPath() {
            return Path.GetTempPath();
        }
        public static string GetPathForTempMediaFiles( string tempFileName ) {
            return Path.Combine( GetTempPath(), tempFileName );
        }
    }
}
