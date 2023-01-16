using Microsoft.EntityFrameworkCore;
using Proact.Services.Entities;
using Proact.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Proact.Services.QueriesServices {
    public class InstitutesQueriesService : IInstitutesQueriesService {
        private readonly ProactDatabaseContext _database;

        public InstitutesQueriesService( ProactDatabaseContext database ) {
            _database = database;
        }

        public Institute Create( InstituteCreationRequest request ) {
            var institute = new Institute() {
                Name = request.Name,
                State = InstituteState.Open
            };

            return _database.Institutes.Add( institute ).Entity;
        }

        public Institute Update( Guid instituteId, InstituteUpdateRequest request ) {
            var institute = Get( instituteId );
            institute.Name = request.Name;

            return institute;
        }

        public void Close( Guid instituteId ) {
            Get( instituteId ).State = InstituteState.Closed;
        }

        public void Open( Guid instituteId ) {
            Get( instituteId ).State = InstituteState.Open;
        }

        public Institute Get( Guid instituteId ) {
            return _database.Institutes
                .Include( x => x.Admins )
                .ThenInclude( x => x.User )
                .FirstOrDefault( x => x.Id == instituteId );
        }

        public Institute GetWhereImAdmin( Guid userId ) {
            return _database.InstituteAdmins
                .Include( x => x.Institute )
                .Include( x => x.User )
                .FirstOrDefault( x => x.UserId == userId )
                .Institute;
        }

        public Institute GetByName( string name ) {
            return _database.Institutes
                .Include( x => x.Admins )
                .ThenInclude( x => x.User )
                .FirstOrDefault( x => x.Name == name );
        }

        public void AssignAdmin( User user, Guid instituteId ) {
            var instituteAdmin = new InstituteAdmin() {
                UserId = user.Id,
                InstituteId = instituteId
            };

            _database.InstituteAdmins.Add( instituteAdmin );
        }

        public List<InstituteAdmin> GetAdmins( Guid instituteId ) {
            return _database.InstituteAdmins.Where( x => x.InstituteId == instituteId ).ToList();
        }

        public List<Institute> GetAll() {
            return _database.Institutes
                .Include( x => x.Admins )
                .ThenInclude( x => x.User )
                .ToList();
        }
    }
}
