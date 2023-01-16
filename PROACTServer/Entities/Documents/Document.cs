using System;

namespace Proact.Services.Entities {
    public class Document : TrackableEntity, IEntity {
        public DocumentType Type { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public string FileName { get; set; }
        public virtual Guid InstituteId { get; set; }
        public virtual Institute Institute { get; set; }
    }
}
