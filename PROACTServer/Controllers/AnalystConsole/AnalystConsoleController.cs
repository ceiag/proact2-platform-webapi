using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proact.Services.AuthorizationPolicies;
using Proact.Services.Entities;
using Proact.Services.EntitiesMapper;
using Proact.Services.Models;
using Proact.Services.QueriesServices;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Net;

namespace Proact.Services.Controllers.AnalystConsole {
    [ApiController]
    [Authorize( Policy = Policies.MessagesAnalysisFromBrowserReadWrite )]
    [Route( ProactRouteConfiguration.DefaultRoute )]
    public class AnalystConsoleController : ProactBaseController {
        private readonly IMessageFormatterService _messageFormatterService;

        public AnalystConsoleController(
            IMessageFormatterService messageFormatterService, 
            IChangesTrackingService changesTrackingService,
            ConsistencyRulesHelper consistencyRulesHelper )
            : base( changesTrackingService, consistencyRulesHelper ) {
            _messageFormatterService = messageFormatterService;
        }

        /// <summary>
        /// Get patient messages anonimized
        /// </summary>
        /// <param name="userId">Patient user id</param>
        /// <returns>Message information</returns>
        [HttpGet]
        [Route( "userId/{userId:guid}/messages" )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( List<BranchedMessagesModel> ) )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.BadRequest, Type = typeof( ErrorModel ) )]
        public IActionResult GetMessagesFromPatient( Guid userId ) {
            User user = null;
            Patient patient = null;
            var currentUser = GetCurrentUser();

            return RulesHelper
                .IfUserIsValid( userId, out user )
                .IfPatientIsValid( userId, out patient )
                .IfUserIsInMyInstitute( GetInstituteId(), user )
                .IfPatientIsInAnyOfMyMedicalTeams( userId, currentUser.Id )
                .Then( () => {
                    return Ok( _messageFormatterService
                        .GetPatientMessagesForAnalystConsole( patient, GetCurrentUserRoles(), 0, int.MaxValue ) );
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Get patient message details
        /// </summary>
        /// <param name="userId">Patient user id</param>
        /// <param name="messageId">Message id</param>
        /// <returns>Message information</returns>
        [HttpGet]
        [Route( "userId/{userId:guid}/messages/{messageId:guid}" )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( BranchedMessagesModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.BadRequest, Type = typeof( ErrorModel ) )]
        public IActionResult GetMessageDetailsFromPatient( Guid userId, Guid messageId ) {
            User user = null;
            Patient patient = null;
            Message message = null;
            var currentUser = GetCurrentUser();

            return RulesHelper
                .IfUserIsValid( userId, out user )
                .IfPatientIsValid( userId, out patient )
                .IfUserIsInMyInstitute( GetInstituteId(), user )
                .IfPatientIsInAnyOfMyMedicalTeams( userId, currentUser.Id )
                .IfMessageIsValid( messageId, out message )
                .IfMessageIsInMyInstitute( GetInstituteId(), message )
                .Then( () => {
                    var branchedMessages = _messageFormatterService
                        .GetPatientMessageForAnalystConsole( messageId, GetCurrentUserRoles(), int.MaxValue );

                    return Ok( branchedMessages );
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Get an analysis resume for a message
        /// </summary>
        /// <param name="messageId">Message identifier</param>
        /// <returns>Message Analysis informations</returns>
        [HttpGet]
        [Route( "messageId/{messageId}/analysis" )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( AnalysisResumeModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.BadRequest, Type = typeof( ErrorModel ) )]
        public IActionResult GetMessageAnalysisResume( Guid messageId ) {
            Project project = null;
            ProjectProperties projectProperties = null;
            Message message = null;

            var currentUser = GetCurrentUser();

            return RulesHelper
                .IfMessageIsValid( messageId, out message )
                .IfProjectIsValid( message.MedicalTeam.ProjectId, out project )
                .IfProjectHasProjectProperties( project.Id, out projectProperties )
                .IfProjectHasAnalystConsoleActive( project.Id )
                .IfProfessionistIsIntoTheMedicalTeam( 
                    currentUser.Id, message.MedicalTeam.Id, GetCurrentUserRoles() )
                .Then( () => {
                    if ( projectProperties.MedicsCanSeeOtherAnalisys ) {
                        return Ok( AnalysisEntityMapper.ToAnalysisResume( 
                            message.Analysis, GetCurrentUser().Id ) );
                    }
                    else {
                        return Ok( AnalysisEntityMapper.ToAnalysisResumeOnlyMine( 
                            message.Analysis, GetCurrentUser().Id ) );
                    }
                } )
                .ReturnResult();
        }
    }
}
