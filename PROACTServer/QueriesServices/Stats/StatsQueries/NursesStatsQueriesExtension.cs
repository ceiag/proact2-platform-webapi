using Microsoft.EntityFrameworkCore;
using Proact.Services.Entities;
using System;
using System.Linq;

namespace Proact.Services.QueriesServices.Stats.StatsQueries {
    public static class NursesStatsQueriesExtension {
        public static IQueryable<Nurse> InsideInstitute(
            this IQueryable<Nurse> query, Guid instituteId ) {
            return query
                .Include( x => x.User )
                .Where( x => x.User.InstituteId == instituteId );
        }

        public static IQueryable<Nurse> InsideProject(
            this IQueryable<Nurse> query, Guid projectId ) {
            return query
                .Include( x => x.User )
                .Where( x => x.MedicalTeamRelations
                    .Any( x => x.MedicalTeam.ProjectId == projectId ) );
        }
    }
}
