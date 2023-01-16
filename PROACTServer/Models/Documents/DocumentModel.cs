using Proact.Services.Entities;
using System;

namespace Proact.Services.Models {
    public class DocumentModel {
        public DocumentType Type { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public virtual Guid InstituteId { get; set; }
    }
}
