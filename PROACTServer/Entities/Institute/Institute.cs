using System;
using System.Collections.Generic;

namespace Proact.Services.Entities {
    public class Institute : TrackableEntity, IEntity {
        public string Name { get; set; }
        public InstituteState State { get; set; }
        public virtual List<Project> Projects { get; set; } = new List<Project>();
        public virtual List<InstituteAdmin> Admins { get; set; } = new List<InstituteAdmin>();
        public virtual List<Document> Documents { get; set; } = new List<Document>();
    }
}
