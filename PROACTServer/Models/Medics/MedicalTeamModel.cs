using Proact.Services.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Proact.Services.Models {
    public class MedicalTeamModel : GeneralityModel {
        [Required]
        public Guid MedicalTeamId { get; set; }

        [Required]
        public string Name { get; set; }
        public MedicalTeamState State { get; set; }
        public ProjectModel Project { get; set; }
    }
}
