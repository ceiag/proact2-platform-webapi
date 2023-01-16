using Proact.Services.Entities;
using Proact.Services.Models;

namespace Proact.Services.EntitiesMapper {
    public static class ProtocolEntityMapper {
        public static ProtocolModel Map( Protocol protocol ) {
            if ( protocol == null ) { return null; }

            return new ProtocolModel() {
                Id = protocol.Id,
                Name = protocol.Name,
                InternalCode = protocol.InternalCode,
                InternationalCode = protocol.InternationalCode,
                Url = string.Empty,
            };
        }
    }
}
