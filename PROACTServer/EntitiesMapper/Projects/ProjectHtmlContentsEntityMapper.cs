using Proact.Services.Entities;
using Proact.Services.Models;

namespace Proact.Services.EntitiesMapper {
    public static class ProjectHtmlContentsEntityMapper {
        public static ProjectHtmlContentsModel Map( ProjectHtmlContent projectHtmlContent ) {
            return new ProjectHtmlContentsModel() {
                HtmlContent = projectHtmlContent.HtmlContent,
                Id = projectHtmlContent.Id,
                ProjectId = projectHtmlContent.ProjectId,
            };
        }
    }
}
