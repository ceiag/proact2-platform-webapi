using Proact.Services.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Proact.Services.Models {
    public class ProjectModel {
        [Required]
        public Guid ProjectId { get; set; }
        public Guid InstituteId { get; set; }
        [Required]
        public ProjectState Status { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string SponsorName { get; set; }
        [Required]
        public string Description { get; set; }
        public ProjectPropertiesModel Properties { get; set; }
    }
}
