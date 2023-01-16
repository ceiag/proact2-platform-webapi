using Microsoft.EntityFrameworkCore;
using Proact.Services.Entities;
using System;
using System.Linq;

namespace Proact.Services.QueriesServices.Stats.StatsQueries {
    public static class MedicsStatsQueriesExtension {
        public static IQueryable<Medic> InsideInstitute(
            this IQueryable<Medic> query, Guid instituteId ) {
            return query
                .Include( x => x.User )
                .Where( x => x.User.InstituteId == instituteId );
        }

        public static IQueryable<Medic> InsideProject(
            this IQueryable<Medic> query, Guid projectId ) {
            return query
                .Include( x => x.User )
                .Where( x => x.MedicalTeamRelations
                    .Any( p => p.MedicalTeam.ProjectId == projectId ) );
        }
    }
}
