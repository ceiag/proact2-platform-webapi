using Microsoft.EntityFrameworkCore;
using Proact.Services.Entities;
using System.Linq;

namespace Proact.Services {
    public static class SurveyAssegnationQueriesExtension {
        public static IQueryable<SurveysAssignationRelation> IncludeSurveyAssegnationCommonTables(
            this IQueryable<SurveysAssignationRelation> rulesHelper ) {
            return rulesHelper
                .Include( x => x.Survey )
                .Include( x => x.Survey.Questions )
                .Include( x => x.User )
                .Include( x => x.UserAnswers );
        }
    }
}
