using Microsoft.AspNetCore.Mvc;
using Proact.Services.Entities;
using System;
using System.Linq;

namespace Proact.Services.QueriesServices {
    public static class DbUserValidityChecker {
        public static ConsistencyRulesHelper IfUserIsValid(
            this ConsistencyRulesHelper rulesHelper, Guid userId, out User user ) {
            User userResult = null;

            var validityChecker = rulesHelper.CheckIf(
                () => {
                    userResult = rulesHelper.GetQueriesService<IUserQueriesService>().Get( userId );
                    return userResult != null;
                },
                () => {
                    return new OkObjectResult( userResult );
                },
                () => {
                    return new NotFoundObjectResult( $"user {userId} can not be found" );
                } );

            user = userResult;
            return validityChecker;
        }

        public static ConsistencyRulesHelper IfUserAccountIsValid(
            this ConsistencyRulesHelper rulesHelper, string accountId, out User user ) {
            User userResult = null;

            var validityChecker = rulesHelper.CheckIf(
                () => {
                    userResult = rulesHelper
                        .GetQueriesService<IUserQueriesService>().GetByAccountId( accountId );

                    return userResult != null;
                },
                () => {
                    return new OkObjectResult( userResult );
                },
                () => {
                    return new NotFoundObjectResult( "accountId not found: " + accountId );
                } );

            user = userResult;
            return validityChecker;
        }

        public static ConsistencyRulesHelper IfUserIsInMyInstitute(
            this ConsistencyRulesHelper rulesHelper, Guid myInstituteId, User user ) {

            var validityChecker = rulesHelper.CheckIf(
                () => {
                    return user.InstituteId == myInstituteId;
                },
                () => {
                    return new OkObjectResult( "" );
                },
                () => {
                    return new BadRequestObjectResult(
                        $"user {user.Id} is not into institute {myInstituteId}" );
                } );

            return validityChecker;
        }

        public static ConsistencyRulesHelper IfUserIsActive(
            this ConsistencyRulesHelper rulesHelper, User user ) {

            var validityChecker = rulesHelper.CheckIf(
                () => {
                    return user.State == UserSubscriptionState.Active;
                },
                () => {
                    return new OkObjectResult( "" );
                },
                () => {
                    return new BadRequestObjectResult( rulesHelper.StringLocalizer["user_not_activated"].Value );
                } );

            return validityChecker;
        }
    }
}
