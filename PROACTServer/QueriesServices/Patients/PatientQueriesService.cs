using Proact.Services.Entities;
using Proact.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Proact.Services.QueriesServices {
    public class PatientQueriesService : IPatientQueriesService {

        private ProactDatabaseContext _database;

        public PatientQueriesService( ProactDatabaseContext proactDatabaseContext ) {
            _database = proactDatabaseContext;
        }

        public Patient Create( User user, PatientCreateRequest patientData ) {
            var patient = new Patient {
                User = user,
                BirthYear = patientData.BirthYear,
                Gender = patientData.Gender,
                UserId = user.Id,
            };

            return _database.Patients.Add( patient ).Entity;
        }

        public Patient Get( Guid userId ) {
            return _database.Patients.FirstOrDefault( x => x.UserId == userId );
        }

        public Patient GetByCode( string code ) {
            return _database.Patients.FirstOrDefault( x => x.Code == code );
        }

        public List<Patient> Gets( List<Guid> userIds ) {
            return _database.Patients.Where( x => userIds.Contains( x.UserId ) ).ToList();
        }

        public List<Patient> GetsAll( Guid instituteId ) {
            return _database.Patients.Where( x => x.User.InstituteId == instituteId ).ToList();
        }

        public List<Patient> GetFromMedicalTeam( Guid medicalTeamId ) {
            return _database.Patients.Where( x => x.MedicalTeamId == medicalTeamId ).ToList();
        }

        public List<Patient> GetFromProject( Guid projectId ) {
            return _database.Patients.Where( x => x.MedicalTeam.ProjectId == projectId ).ToList();
        }

        public List<TreatmentHistory> GetTreatmentHistoriesByPatientId( Guid patientId ) {
            return _database.TreatmentsHistory.Where( x => x.PatientId == patientId ).ToList();
        }

        public void Delete( Patient patient ) {
            _database.Patients.Remove( patient );
        }

        public void AddToMedicalTeam( Guid medicalTeamId, AssignPatientToMedicalTeamRequest request ) {
            var patient = Get( request.UserId );

            AddTreatmentHistory( patient, request );

            patient.MedicalTeamId = medicalTeamId;
            patient.TreatmentStartDate = request.TreatmentStartDate;
            patient.TreatmentEndDate = request.TreatmentEndDate;
            patient.Code = request.Code;

            _database.Patients.Update( patient );
            _database.SaveChanges();
        }

        private void AddTreatmentHistory( Patient patient, AssignPatientToMedicalTeamRequest request ) {
            _database.TreatmentsHistory.Add( new TreatmentHistory() {
                PatientId = patient.Id,
                MedicalTeamId = patient.MedicalTeamId,
                Code = patient.Code,
                StartAt = (DateTime)request.TreatmentStartDate,
                ExpireAt = (DateTime)request.TreatmentEndDate
            } );

            _database.SaveChanges();
        }

        public void RemoveFromMedicalTeam( Guid userId ) {
            Get( userId ).MedicalTeamId = null;
        }

        public bool IsIntoMedicalTeam( Guid userId, MedicalTeam medicalTeam ) {
            return Get( userId ).MedicalTeamId == medicalTeam.Id;
        }

        public bool ArePatientsIntoProject( Guid projectId, List<Guid> userIds ) {
            return Gets( userIds )
                .Where( x => x.MedicalTeam.ProjectId == projectId )
                .Count() == userIds.Count;
        }

        public void SuspendAllPatientsWithTreatmentExpired() {
            var patientsWithExpiredTreatment = _database.Patients
                .Where( x => x.TreatmentEndDate < DateTime.UtcNow )
                .Where( x => x.User.State == UserSubscriptionState.Active )
                .ToList();

            foreach ( var patient in patientsWithExpiredTreatment ) {
                patient.User.State = UserSubscriptionState.Suspended;
            }

            _database.SaveChanges();
        }
    }
}
