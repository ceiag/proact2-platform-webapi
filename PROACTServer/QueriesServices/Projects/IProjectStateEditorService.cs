using System;

namespace Proact.Services.QueriesServices {
    public interface IProjectStateEditorService : IDataEditorService {
        public void CloseProject( Guid projectId );
        public void OpenProject( Guid projectId );
    }
}
