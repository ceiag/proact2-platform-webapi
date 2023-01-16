using Proact.Services.Entities;
using Proact.Services.Models;
using System;
using System.Collections.Generic;

namespace Proact.Services.QueriesServices {
    public interface IInstitutesQueriesService : IQueriesService {
        public Institute Create( InstituteCreationRequest request );
        public Institute Update( Guid instituteId, InstituteUpdateRequest request );
        public Institute Get( Guid instituteId );
        public Institute GetByName( string name );
        public Institute GetWhereImAdmin( Guid userId );
        public List<InstituteAdmin> GetAdmins( Guid instituteId );
        public List<Institute> GetAll();
        public void AssignAdmin( User user, Guid instituteId );
        public void Open( Guid instituteId );
        public void Close( Guid instituteId );
    }
}
