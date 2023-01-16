using Microsoft.AspNetCore.Mvc;
using Proact.Services.Entities;
using Proact.Services.QueriesServices;
using System;

namespace Proact.Services {
    public static class DbResearchersValidityChecker {
        public static ConsistencyRulesHelper IfResearcherIsValid(
            this ConsistencyRulesHelper rulesHelper, Guid userId, out Researcher researcher ) {
            Researcher researcherResult = null;

            var validityChecker = rulesHelper.CheckIf(
                () => {
                    researcherResult = rulesHelper
                        .GetQueriesService<IResearcherQueriesService>().Get( userId );

                    return researcherResult != null;
                },
                () => {
                    return new OkObjectResult( researcherResult );
                },
                () => {
                    return new NotFoundObjectResult( $"Reseacher with userid {userId} not found!" );
                } );

            researcher = researcherResult;
            return validityChecker;
        }

        public static ConsistencyRulesHelper IfResearcherIsNotAlreadyIntoTheMedicalTeam(
            this ConsistencyRulesHelper rulesHelper, Guid userId, Guid medicalTeamId ) {

            var validityChecker = rulesHelper.CheckIf(
                () => {
                    return !rulesHelper
                        .GetQueriesService<IResearcherQueriesService>()
                        .IsIntoMedicalTeam( userId, medicalTeamId );
                },
                () => {
                    return new OkObjectResult( userId );
                },
                () => {
                    return new ConflictObjectResult( "Researcher already present" );
                } );

            return validityChecker;
        }
    }
}
