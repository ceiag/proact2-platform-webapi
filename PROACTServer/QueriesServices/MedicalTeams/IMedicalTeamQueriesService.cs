using Proact.Services.Entities;
using Proact.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Proact.Services.QueriesServices {
    public interface IMedicalTeamQueriesService : IQueriesService {
        public IQueryable<MedicalTeam> GetMedicalTeamsWhereUserIsAdmin( Guid userId );
        public MedicalTeam Get( Guid medicalTeamId );
        public MedicalTeam GetByName( string name );
        public List<MedicalTeam> GetByUserId( Guid userId );
        public MedicAdmin GetAdmin( Guid medicalTeamId, Guid userId );
        public ICollection<MedicAdmin> GetAdmins( Guid medicalTeamId );
        public IQueryable<MedicalTeam> GetAssociatedToAProject( Guid projectId );
        public MedicAdmin AddAdmin( Guid medicalTeamId, Guid userId );
        public MedicalTeam Create( Guid projectId, MedicalTeamCreateRequest request );
        public MedicalTeam Update( Guid medicalTeamId, MedicalTeamUpdateRequest request );
        public void Delete( Guid medicalTeamId );
        public void Close( Guid medicalTeamId );
        public void Open( Guid medicalTeamId );
        public void RemoveAdmin( Guid medicalTeamId, Guid userId );
        public bool IsNameAvailable( string name );
        public bool IsMedicAdminOfMedicalTeam( Guid userId, Guid medicalTeamId );
        public bool UsersAreInTheSameMedicalTeam( Guid firstUserId, Guid secondUserId );
    }
}
