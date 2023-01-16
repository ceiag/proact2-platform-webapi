using System.Collections.Generic;

namespace Proact.Services.AzureMediaServices {
    public class AssetFile {
        public string Duration { get; set; }
    }

    public class AzureAssetFileModel {
        public List<AssetFile> AssetFile { get; set; }
    }
}
