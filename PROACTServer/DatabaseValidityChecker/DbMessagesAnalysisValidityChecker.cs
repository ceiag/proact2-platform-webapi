using Microsoft.AspNetCore.Mvc;
using Proact.Services.QueriesServices;
using Proact.Services.Utils;
using System;

namespace Proact.Services {
    public static class DbMessagesAnalysisValidityChecker {
        public static ConsistencyRulesHelper IfUserCanModifyAnalysis(
           this ConsistencyRulesHelper rulesHelper, Guid userId, Guid analysisId ) {
            var validityChecker = rulesHelper.CheckIf(
                () => {
                    return rulesHelper.GetQueriesService<IMessageAnalysisQueriesService>()
                        .Get( analysisId ).UserId == userId;
                },
                () => {
                    return new OkObjectResult( "" );
                },
                () => {
                    return new BadRequestObjectResult( $"You can not modify this Analysis" );
                } );

            return validityChecker;
        }

        public static ConsistencyRulesHelper IfMessageCanBeAnalyzedAfterMiniumTimePassed(
           this ConsistencyRulesHelper rulesHelper, Guid messageId ) {
            var message = rulesHelper
                        .GetQueriesService<IMessagesQueriesService>()
                        .GetMessage( messageId );

            var projectProps = rulesHelper
                .GetQueriesService<IProjectPropertiesQueriesService>()
                .GetByProjectId( message.MedicalTeam.ProjectId );

            var minutesPassed = TimeCalculatorUtils.GetMinutesPassedSinceInUtc( message.Created );

            var validityChecker = rulesHelper.CheckIf(
                () => {
                    return minutesPassed >= projectProps.MessageCanBeAnalizedAfterMinutes;
                },
                () => {
                    return new OkObjectResult( "" );
                },
                () => {
                    return new BadRequestObjectResult(
                        string.Format( rulesHelper.StringLocalizer["message_can_not_be_analyzed_until"].Value,
                            projectProps.MessageCanBeAnalizedAfterMinutes ) );
                } );

            return validityChecker;
        }

        public static ConsistencyRulesHelper IfAnalysisIsInMyInstitute(
            this ConsistencyRulesHelper rulesHelper, Guid myInstituteId, Guid analysisId ) {

            var validityChecker = rulesHelper.CheckIf(
                () => {
                    return rulesHelper
                        .GetQueriesService<IMessageAnalysisQueriesService>()
                        .Get( analysisId ).Message.Author.InstituteId == myInstituteId;
                },
                () => {
                    return new OkObjectResult( "" );
                },
                () => {
                    return new BadRequestObjectResult(
                        $"analysis {analysisId} is not into institute {myInstituteId}" );
                } );

            return validityChecker;
        }
    }
}
