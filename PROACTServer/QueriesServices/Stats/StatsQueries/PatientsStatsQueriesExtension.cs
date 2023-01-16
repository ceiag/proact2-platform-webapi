using Microsoft.EntityFrameworkCore;
using Proact.Services.Entities;
using System;
using System.Linq;

namespace Proact.Services.QueriesServices.Stats.StatsQueries {
    public static class PatientsStatsQueriesExtension {
        public static IQueryable<Patient> InsideInstitute( 
            this IQueryable<Patient> query, Guid instituteId ) {
            return query
                .Include( x => x.User )
                .Where( x => x.User.InstituteId == instituteId );
        }

        public static IQueryable<Patient> InsideProject(
            this IQueryable<Patient> query, Guid projectId ) {
            return query
                .Include( x => x.User )
                .Include( x => x.MedicalTeam )
                .Where( x => x.MedicalTeam.ProjectId == projectId );
        }
    }
}
