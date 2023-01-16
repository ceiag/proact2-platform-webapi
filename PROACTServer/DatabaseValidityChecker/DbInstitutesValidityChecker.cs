using Microsoft.AspNetCore.Mvc;
using Proact.Services.Entities;
using Proact.Services.QueriesServices;
using System;

namespace Proact.Services {
    public static class DbInstitutesValidityChecker {
        public static ConsistencyRulesHelper IfInstituteIsValid(
           this ConsistencyRulesHelper rulesHelper, Guid instituteId, out Institute institute ) {
            Institute instituteResult = null;

            var validityChecker = rulesHelper.CheckIf(
                () => {
                    instituteResult = rulesHelper
                        .GetQueriesService<IInstitutesQueriesService>().Get( instituteId );

                    return instituteResult != null;
                },
                () => {
                    return new OkObjectResult( "" );
                },
                () => {
                    return new NotFoundObjectResult( $"Institute with id: {instituteId} not found!" );
                } );

            institute = instituteResult;
            return validityChecker;
        }

        public static ConsistencyRulesHelper IfInstituteNameIsAvailable(
           this ConsistencyRulesHelper rulesHelper, string name ) {
            var validityChecker = rulesHelper.CheckIf(
                () => {
                    return rulesHelper.GetQueriesService<IInstitutesQueriesService>()
                        .GetByName( name ) == null;
                },
                () => {
                    return new OkObjectResult( "" );
                },
                () => {
                    return new ConflictObjectResult( $"{name} is already taken!" );
                } );

            return validityChecker;
        }
    }
}
