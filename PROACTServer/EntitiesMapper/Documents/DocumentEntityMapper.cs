using Proact.Services.Entities;
using Proact.Services.Models;

namespace Proact.Services.EntitiesMapper {
    public static class DocumentEntityMapper {
        public static DocumentModel Map( Document document ) {
            if ( document == null ) return null;

            return new DocumentModel() {
                InstituteId = document.InstituteId,
                Title = document.Title,
                Description = document.Description,
                Type = document.Type,
                Url = document.Url,
            };
        }
    }
}
