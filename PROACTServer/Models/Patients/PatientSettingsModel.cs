using System.ComponentModel.DataAnnotations;

namespace Proact.Services.Models {
    public class PatientSettingsModel {
        [Required]
        public bool ShowBroadcastMessages { get; set; }
    }
}
