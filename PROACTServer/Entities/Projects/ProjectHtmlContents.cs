using System;

namespace Proact.Services.Entities {
    public class ProjectHtmlContent : TrackableEntity, IEntity {
        public string HtmlContent { get; set; }
        public ProjectHtmlType Type { get; set; }
        public virtual Guid ProjectId { get; set; }
    }
}
