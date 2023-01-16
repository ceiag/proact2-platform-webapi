using System;

namespace Proact.EncryptionAgentService.Models {
    public class MediaAsset {
        public TimeSpan MediaDuration { get; set; }

        public string AssetId { get; set; }

        public string ManifestUrl { get; set; }
    }
}
