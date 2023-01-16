using Microsoft.AspNetCore.Mvc;
using Proact.Services.AuthorizationPolicies;
using Proact.Services.Entities;
using Proact.Services.Utils;
using System;
using System.Linq;

namespace Proact.Services.QueriesServices {
    public static class DbMessageValidityChecker {
        public static ConsistencyRulesHelper IfMessageIsValid(
            this ConsistencyRulesHelper rulesHelper, Guid messageId, out Message message ) {
            Message messageResult = null;

            var validityChecker = rulesHelper.CheckIf(
                () => {
                    messageResult = rulesHelper
                        .GetQueriesService<IMessagesQueriesService>().GetMessage( messageId );

                    return messageResult != null;
                },
                () => {
                    return new OkObjectResult( messageResult );
                },
                () => {
                    return new NotFoundObjectResult( $"message with id {messageId} not found!" );
                } );

            message = messageResult;
            return validityChecker;
        }

        private static bool IsAuthorOfTheMessage( Message message, Guid userId ) {
            return message.AuthorId == userId;
        }

        private static bool IsMedicalProfessionalIntoMedicalTeam( 
            ConsistencyRulesHelper rulesHelper, UserRoles userRoles, Guid medicalTeamId, Guid userId ) {
            if ( userRoles.HasRoleOf( Roles.MedicalProfessional )
                || userRoles.HasRoleOf( Roles.MedicalTeamAdmin ) ) {
                return rulesHelper
                    .GetQueriesService<IMedicQueriesService>().IsIntoMedicalTeam( userId, medicalTeamId );
            }
            else if ( userRoles.HasRoleOf( Roles.Nurse ) ) {
                return rulesHelper
                    .GetQueriesService<INurseQueriesService>().IsIntoMedicalTeam( userId, medicalTeamId );
            }
            else if ( userRoles.HasRoleOf( Roles.Researcher ) ) {
                return rulesHelper
                    .GetQueriesService<IResearcherQueriesService>().IsIntoMedicalTeam( userId, medicalTeamId );
            }

            return false;
        }

        public static ConsistencyRulesHelper IfUserCanDecryptMedia(
            this ConsistencyRulesHelper rulesHelper, 
            Message message, Guid medicalTeamId, UserRoles userRoles, Guid userId ) {

            var validityChecker = rulesHelper.CheckIf(
                () => {
                    if ( userRoles.HasRoleOf( Roles.Patient ) ) {
                        return IsAuthorOfTheMessage( message, userId ) 
                            || rulesHelper.GetQueriesService<IMessagesQueriesService>()
                                .IsMessageAReplyToMyTopicMessage( userId, message );
                    }
                    else {
                        return IsMedicalProfessionalIntoMedicalTeam( 
                            rulesHelper, userRoles, medicalTeamId, userId );
                    }
                },
                () => {
                    return new OkObjectResult( userId );
                },
                () => {
                    return new UnauthorizedObjectResult( "You can not decrypt this media" );
                } );

            return validityChecker;
        }

        public static ConsistencyRulesHelper IfUserCanDecryptMedia(
            this ConsistencyRulesHelper rulesHelper,
            Message message, UserRoles requesterRoles, Guid requesterUserId ) {

            var validityChecker = rulesHelper.CheckIf(
                () => {
                    if ( requesterRoles.HasRoleOf( Roles.Patient ) ) {
                        return IsAuthorOfTheMessage( message, requesterUserId )
                            || rulesHelper.GetQueriesService<IMessagesQueriesService>()
                                .IsMessageAReplyToMyTopicMessage( requesterUserId, message );
                    }
                    else {
                        return rulesHelper
                            .GetQueriesService<IMedicalTeamQueriesService>()
                            .UsersAreInTheSameMedicalTeam( requesterUserId, (Guid)message.AuthorId );
                    }
                },
                () => {
                    return new OkObjectResult( requesterUserId );
                },
                () => {
                    return new UnauthorizedObjectResult( "You can not decrypt this media" );
                } );

            return validityChecker;
        }

        public static ConsistencyRulesHelper IfUserIsAuthorOfMessage(
            this ConsistencyRulesHelper rulesHelper, Message message, Guid userId ) {

            var validityChecker = rulesHelper.CheckIf(
                () => {
                    return IsAuthorOfTheMessage( message, userId );
                },
                () => {
                    return new OkObjectResult( userId );
                },
                () => {
                    return new UnauthorizedObjectResult( "" );
                } );

            return validityChecker;
        }

        public static ConsistencyRulesHelper IfMessageCanBeRepliedByMedic(
            this ConsistencyRulesHelper rulesHelper, Guid messageId, UserRoles userRoles ) {

            var message = rulesHelper
                        .GetQueriesService<IMessagesQueriesService>()
                        .GetMessage( messageId );

            var projectProps = rulesHelper
                .GetQueriesService<IProjectPropertiesQueriesService>()
                .GetByProjectId( message.MedicalTeam.ProjectId );

            var minutesPassed = TimeCalculatorUtils.GetMinutesPassedSinceInUtc( message.Created );

            var validityChecker = rulesHelper.CheckIf(
                () => {
                    if ( userRoles.HasRoleOf( Roles.Patient ) ) {
                        return true;
                    }
                    else {
                        return minutesPassed >= projectProps.MessageCanBeRepliedAfterMinutes;
                    }
                },
                () => {
                    return new OkObjectResult( "" );
                },
                () => {
                    return new BadRequestObjectResult(
                        string.Format( rulesHelper.StringLocalizer["message_can_not_be_replied"].Value,
                            projectProps.MessageCanBeRepliedAfterMinutes ) );
                } );

            return validityChecker;
        }

        public static ConsistencyRulesHelper IfMessageCanBeDeleted(
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
                    return minutesPassed < projectProps.MessageCanNotBeDeletedAfterMinutes;
                },
                () => {
                    return new OkObjectResult( "" );
                },
                () => {
                    return new BadRequestObjectResult(
                        rulesHelper.StringLocalizer["message_can_not_be_deleted"].Value );
                } );

            return validityChecker;
        }

        public static ConsistencyRulesHelper IfMessageIsInMyInstitute(
            this ConsistencyRulesHelper rulesHelper, Guid myInstituteId, Message message ) {

            var validityChecker = rulesHelper.CheckIf(
                () => {
                    return message.Author.InstituteId == myInstituteId;
                },
                () => {
                    return new OkObjectResult( "" );
                },
                () => {
                    return new BadRequestObjectResult(
                        $"message {message.Id} is not into institute {myInstituteId}" );
                } );

            return validityChecker;
        }
    }
}
