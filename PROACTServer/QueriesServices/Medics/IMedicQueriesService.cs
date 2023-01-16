using Proact.Services.Entities;
using System;
using System.Collections.Generic;

namespace Proact.Services.QueriesServices {
    public interface IMedicQueriesService : IQueriesService {
        public Medic Get( Guid userId );
        public ICollection<Medic> GetsAll( Guid instituteId );
        public Medic Create( Guid userId );
        public Medic Delete( Guid userId );
        public MedicAdmin GetAdminById( Guid userId );
        public bool IsWithoutMedicalTeam( Guid userId );
        public bool IsMedicIntoProject( Guid userId, Guid projectId );
        public void AddToMedicalTeam( Guid userId, Guid medicalTeamId );
        public void RemoveFromMedicalTeam( Guid userId, MedicalTeam medicalTeam );
        public bool IsIntoMedicalTeam( Guid userId, Guid medicalTeamId );
        public bool IsMedicAdminOfMedicalTeam( Guid userId, Guid medicalTeamId );
    }
}
