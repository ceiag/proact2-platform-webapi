using Microsoft.EntityFrameworkCore;
using Proact.Services.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Proact.Services.QueriesServices {
    public class ResearcherQueriesService : IResearcherQueriesService {
        private ProactDatabaseContext _database;

        public ResearcherQueriesService( ProactDatabaseContext database ) { 
            _database = database;
        }

        public void AddToMedicalTeam( Guid userId, Guid medicalTeamId ) {
            if ( !IsAlreadyInThisMedicalTeam( userId, medicalTeamId ) ) {
                var researcher = Get( userId );
                var researcheMedicalTeamRelation = new ResearchersMedicalTeamRelation() {
                    Id = Guid.NewGuid(),
                    MedicalTeamId = medicalTeamId,
                    ResearcherId = researcher.Id
                };

                _database.ResearchersMedicalTeamRelation.Add( researcheMedicalTeamRelation );
            }
        }

        private bool IsAlreadyInThisMedicalTeam( Guid userId, Guid medicalTeamId ) {
            return GetResearcherMedicalTeamRelation( userId, medicalTeamId ) != null;
        }

        public Researcher Create( Guid userId ) {
            return _database.Researchers.Add( new Researcher() { UserId = userId } ).Entity;
        }

        public Researcher Delete( Guid userId ) {
            return _database.Researchers.Remove( Get( userId ) ).Entity;
        }

        public Researcher Get( Guid userId ) {
            return _database.Researchers.FirstOrDefault( x => x.UserId == userId );
        }

        public List<Researcher> GetsAll( Guid instituteId ) {
            return _database.Researchers
                .Include( x => x.User )
                .ThenInclude( x => x.Institute )
                .Where( x => x.User.Institute.Id == instituteId )
                .ToList();
        }

        public bool IsIntoMedicalTeam( Guid userId, Guid medicalTeamId ) {
            return Get( userId ).MedicalTeams.Any( x => x.Id == medicalTeamId );
        }

        public bool IsIntoProject( Guid userId, Guid projectId ) {
            return Get( userId ).MedicalTeams.Any( x => x.ProjectId == projectId );
        }

        public bool IsWithoutMedicalTeam( Guid userId ) {
            return Get( userId ).MedicalTeams.Count == 0;
        }

        public void RemoveFromMedicalTeam( Guid userId, MedicalTeam medicalTeam ) {
            var relationToRemove = GetResearcherMedicalTeamRelation( userId, medicalTeam.Id );
            _database.ResearchersMedicalTeamRelation.Remove( relationToRemove );
        }

        private ResearchersMedicalTeamRelation GetResearcherMedicalTeamRelation( 
            Guid userId, Guid medicalTeamId ) {
            return _database.ResearchersMedicalTeamRelation.FirstOrDefault(
                x => x.MedicalTeamId == medicalTeamId && x.Researcher.UserId == userId );
        }
    }
}
