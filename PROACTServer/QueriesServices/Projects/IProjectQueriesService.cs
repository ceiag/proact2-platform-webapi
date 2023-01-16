using Proact.Services.Entities;
using Proact.Services.Models;
using System;
using System.Collections.Generic;

namespace Proact.Services.QueriesServices {
    public interface IProjectQueriesService : IQueriesService {
        public Project Create( Guid instituteId, ProjectCreateRequest projectCreateRequest );
        public Project Update( Guid projectId, ProjectUpdateRequest projectUpdateRequest );
        public void AssignAdmin( Guid projectId, Medic medic );
        public void Remove( Guid projectId );
        public bool IsProjectNameAvailable( string name );
        public Project Get( Guid projectId );
        public List<Project> GetsAll( Guid InstituteId );
        public bool IsOpened( Guid projectId );
        public List<Project> GetProjectsWhereUserIsAssociated( Guid userId );
    }
}
