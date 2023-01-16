using System.ComponentModel.DataAnnotations;

namespace Proact.Services.Models {
    public class MedicalTeamUpdateRequest : GeneralityModel {
        [Required]
        public string Name { get; set; }
        [Required]
        public bool Enabled { get; set; }
    }
}
