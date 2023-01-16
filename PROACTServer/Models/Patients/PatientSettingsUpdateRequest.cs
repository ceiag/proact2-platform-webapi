using System.ComponentModel.DataAnnotations;

namespace Proact.Services.Models {
    public class PatientSettingsUpdateRequest {
        [Required]
        public bool ShowBroadcastMessages { get; set; }
    }
}
