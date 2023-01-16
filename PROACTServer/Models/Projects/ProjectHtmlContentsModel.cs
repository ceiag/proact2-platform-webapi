using System;

namespace Proact.Services.Models {
    public class ProjectHtmlContentsModel {
        public Guid Id { get; set; }
        public string HtmlContent { get; set; }
        public Guid ProjectId { get; set; }
    }
}
