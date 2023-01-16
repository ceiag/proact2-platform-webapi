using Proact.Services.Entities;
using System;
using System.Collections.Generic;

namespace Proact.Services.QueriesServices {
    public interface IResearcherQueriesService : IQueriesService {
        public Researcher Get( Guid userId );
        public List<Researcher> GetsAll( Guid instituteId );
        public Researcher Create( Guid userId );
        public Researcher Delete( Guid userId );
        public bool IsWithoutMedicalTeam( Guid userId );
        public bool IsIntoProject( Guid userId, Guid projectId );
        public void AddToMedicalTeam( Guid userId, Guid medicalTeamId );
        public void RemoveFromMedicalTeam( Guid userId, MedicalTeam medicalTeam );
        public bool IsIntoMedicalTeam( Guid userId, Guid medicalTeamId );
    }
}
