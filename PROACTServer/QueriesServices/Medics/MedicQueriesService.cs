using Microsoft.EntityFrameworkCore;
using Proact.Services.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Proact.Services.QueriesServices {
    public class MedicQueriesService : IMedicQueriesService {
        private ProactDatabaseContext _database;

        public MedicQueriesService( ProactDatabaseContext proactDatabaseContext ) {
            _database = proactDatabaseContext;
        }

        public Medic Get( Guid userId ) {
            return _database.Medics.FirstOrDefault( x => x.UserId == userId );
        }

        public ICollection<Medic> GetsAll( Guid instituteId ) {
            return _database.Medics
                .Include( x => x.User )
                .ThenInclude( x => x.Institute )
                .Where( x => x.User.Institute.Id == instituteId )
                .ToList();
        }

        public MedicAdmin GetAdminById( Guid userId ) {
            return _database.MedicAdmins.FirstOrDefault( x => x.UserId == userId );
        }

        public bool IsWithoutMedicalTeam( Guid userId ) {
            return Get( userId ).MedicalTeams.Count == 0;
        }

        public Medic Create( Guid userId ) {
            return _database.Medics.Add( new Medic() { UserId = userId } ).Entity;
        }

        public Medic Delete( Guid userId ) {
            return _database.Medics.Remove( Get( userId ) ).Entity;
        }

        public bool IsMedicIntoProject( Guid userId, Guid projectId ) {
            return Get( userId ).MedicalTeams.Any( x => x.ProjectId == projectId );
        }

        private bool IsMedicAlreadyInThisMedicalTeam( Guid userId, Guid medicalTeamId ) {
            return GetMedicsMedicalTeamRelation( userId, medicalTeamId ) != null;
        }

        public void AddToMedicalTeam( Guid userId, Guid medicalTeamId ) {
            if ( !IsMedicAlreadyInThisMedicalTeam( userId, medicalTeamId ) ) {
                var medic = Get( userId );
                var medicMedicalTeamRelation = new MedicsMedicalTeamRelation() {
                    Id = Guid.NewGuid(),
                    MedicalTeamId = medicalTeamId,
                    MedicId = medic.Id
                };

                _database.MedicsMedicalTeamRelations.Add( medicMedicalTeamRelation );
            }
        }

        private MedicsMedicalTeamRelation GetMedicsMedicalTeamRelation( Guid userId, Guid medicalTeamId ) {
            return _database.MedicsMedicalTeamRelations.FirstOrDefault( 
                x => x.MedicalTeamId == medicalTeamId && x.Medic.UserId == userId );
        }

        public void RemoveFromMedicalTeam( Guid userId, MedicalTeam medicalTeam ) {
            var relationToRemove = GetMedicsMedicalTeamRelation( userId, medicalTeam.Id );
            _database.MedicsMedicalTeamRelations.Remove( relationToRemove );
        }

        public bool IsIntoMedicalTeam( Guid userId, Guid medicalTeamId ) {
            var user = Get( userId );
            return Get( userId ).MedicalTeams.Any( x => x.Id == medicalTeamId );
        }

        public bool IsMedicAdminOfMedicalTeam( Guid userId, Guid medicalTeamId ) {
            var medic = Get( userId );
            var medicalTeam = medic.MedicalTeams.FirstOrDefault( x => x.Id == medicalTeamId );

            if ( medicalTeam != null && medicalTeam.Admins != null ) {
                return medicalTeam.Admins.Any( x => x.UserId == userId );
            }

            return false;
        }
    }
}
