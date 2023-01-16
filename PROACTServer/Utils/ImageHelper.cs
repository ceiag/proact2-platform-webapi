using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using System;
using System.IO;

namespace Proact.Services.Utils {
    public static class ImageHelper {
        private static Size ComputeSize( int pixelSizeToReach, int originalWidth, int originalHeight ) {
            var maxSide = Math.Max( originalWidth, originalHeight );
            var width = pixelSizeToReach * originalWidth / maxSide;
            var height = pixelSizeToReach * originalHeight / maxSide;

            return new Size() {
                Height = height,
                Width = width
            };
        }

        public static Stream ResizeImage(
            Stream originalStream, int pixelSize, int quality ) {
            Stream croppedStream = new MemoryStream();

            var imageToCrop = Image.Load( originalStream );

            if ( imageToCrop.Width < pixelSize || imageToCrop.Height < pixelSize ) {
                originalStream.Position = 0;
                return originalStream;
            }

            var size = ComputeSize( pixelSize, imageToCrop.Width, imageToCrop.Height );

            imageToCrop.Mutate( p => p.Resize( size ) );

            JpegEncoder jpegEncoder = new JpegEncoder() {
                Quality = quality
            };
            
            imageToCrop.SaveAsJpeg( croppedStream, jpegEncoder );

            croppedStream.Position = 0;

            return croppedStream;
        }
    }
}
