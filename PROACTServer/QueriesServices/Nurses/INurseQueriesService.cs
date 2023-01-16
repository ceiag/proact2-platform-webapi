using Proact.Services.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Proact.Services.QueriesServices {
    public interface INurseQueriesService : IQueriesService {
        public Nurse Get( Guid userId );
        public List<Nurse> GetAll( Guid instituteId );
        public Nurse Create( Guid userId );
        public Nurse Delete( Guid userId );
        public void AddToMedicalTeam( Guid userId, Guid medicalTeamId );
        public void RemoveFromMedicalTeam( Guid userId, MedicalTeam medicalTeam );
        public bool IsIntoMedicalTeam( Guid userId, Guid medicalTeamId );
    }
}
