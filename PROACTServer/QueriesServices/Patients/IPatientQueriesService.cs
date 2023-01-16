using Proact.Services.Entities;
using Proact.Services.Models;
using System;
using System.Collections.Generic;

namespace Proact.Services.QueriesServices {
    public interface IPatientQueriesService : IQueriesService {
        public Patient Create( User user, PatientCreateRequest patientData );
        public Patient Get( Guid userId );
        public Patient GetByCode( string code );
        public List<Patient> Gets( List<Guid> userIds );
        public List<Patient> GetsAll( Guid instituteId );
        public List<Patient> GetFromMedicalTeam( Guid medicalTeamId );
        public List<Patient> GetFromProject( Guid projectId );
        public List<TreatmentHistory> GetTreatmentHistoriesByPatientId( Guid patientId );
        public void Delete( Patient patient );
        public void AddToMedicalTeam( Guid medicalTeamId, AssignPatientToMedicalTeamRequest request );
        public void RemoveFromMedicalTeam( Guid userId );
        public bool IsIntoMedicalTeam( Guid userId, MedicalTeam medicalTeam );
        public bool ArePatientsIntoProject( Guid projectId, List<Guid> userIds );
        public void SuspendAllPatientsWithTreatmentExpired();
    }
}
