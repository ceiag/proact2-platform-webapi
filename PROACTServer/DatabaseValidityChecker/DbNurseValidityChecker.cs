using Microsoft.AspNetCore.Mvc;
using Proact.Services.Entities;
using System;

namespace Proact.Services.QueriesServices {
    public static class DbNurseValidityChecker {
        public static ConsistencyRulesHelper IfNurseIsValid(
            this ConsistencyRulesHelper rulesHelper, Guid userId, out Nurse nurse ) {
            Nurse nurseResult = null;

            var validityChecker = rulesHelper.CheckIf(
                () => {
                    nurseResult = rulesHelper.GetQueriesService<INurseQueriesService>().Get( userId );
                    
                    return nurseResult != null;
                },
                () => {
                    return new OkObjectResult( userId );
                },
                () => {
                    return new NotFoundObjectResult( $"Nurse with userId: {userId} not found!" );
                } );

            nurse = nurseResult;
            return validityChecker;
        }

        public static ConsistencyRulesHelper IfNurseNotExist(
            this ConsistencyRulesHelper rulesHelper, Guid userId ) {

            var validityChecker = rulesHelper.CheckIf(
                () => {
                    return rulesHelper.GetQueriesService<INurseQueriesService>().Get( userId ) == null;
                },
                () => {
                    return new OkObjectResult( userId );
                },
                () => {
                    return new ConflictObjectResult( $"Nurse with userId: {userId} already exist!" );
                } );

            return validityChecker;
        }
    }
}
