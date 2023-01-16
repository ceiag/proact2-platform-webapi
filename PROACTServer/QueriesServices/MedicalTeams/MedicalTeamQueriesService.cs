using Microsoft.EntityFrameworkCore;
using Proact.Services.Entities;
using Proact.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Proact.Services.QueriesServices {
    public class MedicalTeamQueriesService : IMedicalTeamQueriesService {
        private ProactDatabaseContext _database;

        public MedicalTeamQueriesService( ProactDatabaseContext database ) {
            _database = database;
        }

        public IQueryable<MedicalTeam> GetMedicalTeamsWhereUserIsAdmin( Guid userId ) {
            return _database.MedicalTeams
                .Include( x => x.Admins )
                .Include( x => x.Patients )
                .Include( x => x.MedicsRelation )
                .Where( x => x.Admins.Any( x => x.UserId == userId ) );
        }

        public MedicalTeam Get( Guid medicalTeamId ) {
            return _database.MedicalTeams
                .Include( x => x.Admins )
                .Include( x => x.Patients )
                .Include( x => x.MedicsRelation )
                .FirstOrDefault( x => x.Id == medicalTeamId );
        }

        public List<MedicalTeam> GetByUserId( Guid userId ) {
            var medicalTeamForMedics = _database.MedicsMedicalTeamRelations
                .Where( x => x.Medic.UserId == userId )
                .Select( x => x.MedicalTeam )
                .ToList();

            var medicalTeamForNurses = _database.NursesMedicalTeamRelations
                .Where( x => x.Nurse.UserId == userId )
                .Select( x => x.MedicalTeam )
                .ToList();

            var medicalTeamForResearchers = _database.ResearchersMedicalTeamRelation
                .Where( x => x.Researcher.UserId == userId )
                .Select( x => x.MedicalTeam )
                .ToList();

            var patient = _database.Patients
                .FirstOrDefault( x => x.UserId == userId );

            var medicalTeams = new List<MedicalTeam>();
            medicalTeams.AddRange( medicalTeamForMedics );
            medicalTeams.AddRange( medicalTeamForNurses );
            medicalTeams.AddRange( medicalTeamForResearchers );

            if ( patient != null ) {
                medicalTeams.Add( patient.MedicalTeam );
            }
            
            return medicalTeams;
        }

        public bool UsersAreInTheSameMedicalTeam( Guid firstUserId, Guid secondUserId ) {
            var firstUserMedicalTeams = GetByUserId( firstUserId );
            var secondUserMedicaleams = GetByUserId( secondUserId );

            return firstUserMedicalTeams.Intersect( secondUserMedicaleams ).Any();
        }

        public IQueryable<MedicalTeam> GetAssociatedToAProject( Guid projectId ) {
            return _database.MedicalTeams.Where( x => x.ProjectId == projectId );
        }

        public ICollection<MedicAdmin> GetAdmins( Guid medicalTeamId ) {
            return Get( medicalTeamId ).Admins;
        }

        public MedicAdmin AddAdmin( Guid medicalTeamId, Guid userId ) {
            var medicAdmin = new MedicAdmin() {
                UserId = userId,
                MedicalTeamId = medicalTeamId
            };

            return _database.MedicAdmins.Add( medicAdmin ).Entity;
        }

        public MedicalTeam Create( Guid projectId, MedicalTeamCreateRequest request ) {
            return _database.MedicalTeams.Add( new MedicalTeam() {
                ProjectId = projectId,
                Enabled = true,
                Name = request.Name,
                Phone = request.Phone,
                AddressLine1 = request.AddressLine1,
                AddressLine2 = request.AddressLine2,
                City = request.City,
                Country = request.Country,
                PostalCode = request.PostalCode,
                TimeZone = request.TimeZone,
                RegionCode = request.RegionCode,
                StateOrProvince = request.StateOrProvince
            } ).Entity;
        }

        public MedicalTeam Update( Guid medicalTeamId, MedicalTeamUpdateRequest request ) {
            var medicalTeam = Get( medicalTeamId );

            medicalTeam.Name = request.Name;
            medicalTeam.Phone = request.Phone;
            medicalTeam.AddressLine1 = request.AddressLine1;
            medicalTeam.AddressLine2 = request.AddressLine2;
            medicalTeam.City = request.City;
            medicalTeam.Country = request.Country;
            medicalTeam.PostalCode = request.PostalCode;
            medicalTeam.TimeZone = request.TimeZone;
            medicalTeam.RegionCode = request.RegionCode;
            medicalTeam.StateOrProvince = request.StateOrProvince;
            medicalTeam.Enabled = request.Enabled;

            return medicalTeam;
        }

        public void Delete( Guid medicalTeamId ) {
            _database.MedicalTeams.Remove( Get( medicalTeamId ) );
        }

        public void RemoveAdmin( Guid medicalTeamId, Guid userId ) {
            Get( medicalTeamId ).Admins.Remove( GetAdmin( medicalTeamId, userId ) );
        }

        public MedicalTeam GetByName( string name ) {
            return _database.MedicalTeams.FirstOrDefault( x => x.Name == name );
        }

        public MedicAdmin GetAdmin( Guid medicalTeamId, Guid userId ) {
            return Get( medicalTeamId ).Admins.First( x => x.UserId == userId );
        }

        public bool IsMedicAdminOfMedicalTeam( Guid userId, Guid medicalTeamId ) {
            return _database.MedicAdmins.FirstOrDefault( 
                x => x.UserId == userId && x.MedicalTeamId == medicalTeamId ) != null;
        }

        public bool IsNameAvailable( string name ) {
            return GetByName( name ) == null;
        }

        public void Close( Guid medicalTeamId ) {
            Get( medicalTeamId ).State = MedicalTeamState.ClosedByMedicalTeam;
        }

        public void Open( Guid medicalTeamId ) {
            var medicalTeam = Get( medicalTeamId );

            if ( medicalTeam.State == MedicalTeamState.ClosedByMedicalTeam ) {
                medicalTeam.State = MedicalTeamState.Open;
            }
        }
    }
}
