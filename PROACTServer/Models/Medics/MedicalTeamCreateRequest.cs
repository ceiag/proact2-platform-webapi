using System.ComponentModel.DataAnnotations;

namespace Proact.Services.Models {
    public class MedicalTeamCreateRequest : GeneralityModel {
        [Required]
        public string Name { get; set; }
    }
}
