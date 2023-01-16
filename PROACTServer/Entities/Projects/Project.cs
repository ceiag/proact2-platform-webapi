using System;
using System.Collections.Generic;

namespace Proact.Services.Entities {
    public class Project : TrackableEntity, IEntity {
        public ProjectState State { get; set; }
        public string Name { get; set; }
        public string SponsorName { get; set; }
        public string Description { get; set; }
        public virtual User Admin { get; set; }
        public virtual Guid AdminUserId { get; set; }
        public virtual Guid? ProjectPropertiesId { get; set; }
        public virtual ProjectProperties ProjectProperties { get; set; }
        public virtual List<MedicalTeam> MedicalTeams { get; set; }
        public virtual Institute Institute { get; set; }
        public virtual Guid? InstituteId { get; set; }
    }
}
