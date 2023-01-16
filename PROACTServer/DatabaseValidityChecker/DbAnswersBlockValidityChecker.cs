using Microsoft.AspNetCore.Mvc;
using Proact.Services.Entities;
using System;

namespace Proact.Services.QueriesServices {
    public static class DbAnswersBlockValidityChecker {
        public static ConsistencyRulesHelper IfAnswersBlockIsValid(
            this ConsistencyRulesHelper rulesHelper, Guid answersBlockId, out SurveyAnswersBlock answersBlock ) {
            SurveyAnswersBlock answersBlockResult = null;

            var validityChecker = rulesHelper.CheckIf(
                () => {
                    answersBlockResult = rulesHelper
                        .GetQueriesService<ISurveyAnswersBlockQueriesService>().Get( answersBlockId );

                    return answersBlockResult != null;
                },
                () => {
                    return new OkObjectResult( answersBlockResult );
                },
                () => {
                    return new NotFoundObjectResult( $"answers block with id {answersBlockId} not found." );
                } );

            answersBlock = answersBlockResult;
            return validityChecker;
        }
    }
}
