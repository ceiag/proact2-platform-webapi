using Proact.Services.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Proact.Services.QueriesServices {
    public class NurseQueriesService : INurseQueriesService {
        private ProactDatabaseContext _database;

        public NurseQueriesService( ProactDatabaseContext proactDatabaseContext ) {
            _database = proactDatabaseContext;
        }

        public Nurse Get( Guid userId ) {
            return _database.Nurses.FirstOrDefault( x => x.UserId == userId );
        }

        public Nurse Create( Guid userId ) {
            return _database.Nurses.Add( new Nurse() { UserId = userId } ).Entity;
        }

        public void RemoveNurseFromMedicalTeam( MedicalTeam medicalTeam, Nurse medic ) {
            medicalTeam.Nurses.Remove( medic );
        }

        public Nurse Delete( Guid userId ) {
            return _database.Nurses.Remove( Get( userId ) ).Entity;
        }

        public List<Nurse> GetAll( Guid instituteId ) {
            return _database.Nurses.Where( x => x.User.InstituteId == instituteId ).ToList();
        }

        public void AddToMedicalTeam( Guid userId, Guid medicalTeamId ) {
            var nurse = Get( userId );
            var nurseMedicalTeamRelation = new NursesMedicalTeamRelation() {
                Id = Guid.NewGuid(),
                MedicalTeamId = medicalTeamId,
                NurseId = nurse.Id
            };

            _database.NursesMedicalTeamRelations.Add( nurseMedicalTeamRelation );
        }

        private NursesMedicalTeamRelation GetNurseMedicalTeamRelation( Guid userId, Guid medicalTeamId ) {
            return _database.NursesMedicalTeamRelations.FirstOrDefault(
                x => x.MedicalTeamId == medicalTeamId && x.Nurse.UserId == userId );
        }

        public void RemoveFromMedicalTeam( Guid userId, MedicalTeam medicalTeam ) {
            var relationToRemove = GetNurseMedicalTeamRelation( userId, medicalTeam.Id );

            _database.NursesMedicalTeamRelations.Remove( relationToRemove );
        }

        public bool IsIntoMedicalTeam( Guid userId, Guid medicalTeamId ) {
            return Get( userId ).MedicalTeams.Any( x => x.Id == medicalTeamId );
        }
    }
}
