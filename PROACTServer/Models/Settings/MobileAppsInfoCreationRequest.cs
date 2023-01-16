using System.ComponentModel.DataAnnotations;

namespace Proact.Services.Models {
    public class MobileAppsInfoCreationRequest {
        [Required]
        public int AndroidLastBuildRequired { get; set; }
        [Required]
        public int iOSLastBuildRequired { get; set; }
        public string AndroidStoreUrl { get; set; }
        public string iOSStoreUrl { get; set; }
    }
}
