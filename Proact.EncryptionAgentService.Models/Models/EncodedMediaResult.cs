using System;

namespace Proact.EncryptionAgentService.Models {
    public class EncodedMediaResult {
        public Guid UserId { get; set; }

        public MediaType MediaType { get; set; }

        public MediaAsset DrmProtectedMediaAsset { get; set; }

        public MediaAsset AesProtedMediaAsset { get; set; }
    }
}
