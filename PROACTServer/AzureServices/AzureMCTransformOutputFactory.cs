using Microsoft.Azure.Management.Media.Models;
using System;

namespace Proact.Services {
    public static class AzureMCTransformOutputFactory {

        private static AacAudio GetAudioCodec() {
            var audioCodec = new AacAudio(
                channels: 2,
                samplingRate: 48000,
                bitrate: 128000,
                profile: AacAudioProfile.AacLc );

            return audioCodec;
        }

        private static H264Video GetVideoCodec() {
            var videoCodec = new H264Video( 
                keyFrameInterval: TimeSpan.FromSeconds( 2 ),
                layers: new H264Layer[] { 
                    new H264Layer (
                        bitrate: 1600000, 
                        width: "960",
                        height: "540",
                        label: "SD-1600kbps" ) 
                } );

            return videoCodec;
        }

        private static JpgImage GetThumbnailsCodec() {
            return new JpgImage(
                start: "50%", step: "50%", range: "50%",
                layers: new JpgLayer[] {
                    new JpgLayer( width: "100%", height: "100%", null, 80 )
                } );
        }

        public static TransformOutput GetTransformOutputWithThumbnails() {
            var transformOutput = new TransformOutput(
                 new StandardEncoderPreset(
                     codecs: new Codec[] {
                        GetThumbnailsCodec(),
                        GetAudioCodec(),
                        GetVideoCodec()
                     },
                     formats: new Format[] {
                        new JpgFormat(
                            filenamePattern: "Thumbnail-{Basename}-{Index}{Extension}"
                        ),
                         new Mp4Format(
                            filenamePattern: "Video-{Basename}-{Label}-{Bitrate}{Extension}"
                        )
                     }
                 ),
                 onError: OnErrorType.StopProcessingJob,
                 relativePriority: Priority.Normal
             );

            return transformOutput;
        }

        public static TransformOutput GetTransformOutputStandardEncoder() {
            var transformOutput = new TransformOutput {
                Preset = new BuiltInStandardEncoderPreset() {
                    PresetName = EncoderNamedPreset.AdaptiveStreaming
                }
            };

            return transformOutput;
        }
    }
}
