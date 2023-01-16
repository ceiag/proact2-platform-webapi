using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proact.Services.AuthorizationPolicies;
using Proact.Services.Entities;
using Proact.Services.EntitiesMapper;
using Proact.Services.Models;
using Proact.Services.QueriesServices;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Net;

namespace Proact.Services.Controllers.MessageAnalysis {
    [ApiController]
    [Authorize( Policy = Policies.MessagesAnalysisReadWrite )]
    [Route( ProactRouteConfiguration.DefaultRoute )]
    public class MessageAnalysisController : ProactBaseController {
        private readonly IMessageAnalysisQueriesService _messageAnalysisQueriesService;

        public MessageAnalysisController(
            IMessageAnalysisQueriesService messageAnalysisQueriesService,
            IChangesTrackingService changesTrackingService, 
            ConsistencyRulesHelper consistencyRulesHelper ) 
            : base( changesTrackingService, consistencyRulesHelper ) {
            _messageAnalysisQueriesService = messageAnalysisQueriesService;
        }

        /// <summary>
        /// Add an analysis to a message
        /// </summary>
        /// <param name="projectId">Project identifier</param>
        /// <param name="medicalTeamId">Medical team identifier</param>
        /// <param name="request">Data to add analysis to a message</param>
        /// <returns>Message information</returns>
        [HttpPost]
        [Obsolete]
        [Route( "{projectId:guid}/{medicalTeamId:guid}/create" )]
        [Authorize( Policy = Policies.MessagesAnalysisReadWrite )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( AnalysisModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.BadRequest, Type = typeof( ErrorModel ) )]
        public IActionResult AddAnalysisToMessage(
            Guid projectId, Guid medicalTeamId, AnalysisCreationRequest request ) {
            MedicalTeam medicalTeam = null;
            Project project = null;
            Message message = null;
            ProjectProperties projectProps = null;

            var currentUser = GetCurrentUser();

            return RulesHelper
                .IfProjectIsValid( projectId, out project )
                .IfProjectHasProjectProperties( projectId, out projectProps )
                .IfProjectHasAnalystConsoleActive( projectId )
                .IfMedicalTeamIsValid( medicalTeamId, out medicalTeam )
                .IfProfessionistIsIntoTheMedicalTeam( currentUser.Id, medicalTeamId, GetCurrentUserRoles() )
                .IfMessageIsValid( request.MessageId, out message )
                .IfMessageCanBeAnalyzedAfterMiniumTimePassed( request.MessageId )
                .IfMessageIsInMyInstitute( GetInstituteId(), message )
                .Then( () => {
                    var createdAnalysis = _messageAnalysisQueriesService
                        .Create( currentUser.Id, request );

                    SaveChanges();

                    var retrivedAnalysis = _messageAnalysisQueriesService.Get( createdAnalysis.Id );

                    return Ok( AnalysisEntityMapper.Map( retrivedAnalysis, GetCurrentUser().Id ) );
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Add an analysis to a message
        /// </summary>
        /// <param name="messageId">Message identifier</param>
        /// <param name="request">Data to add analysis to a message</param>
        /// <returns>Message information</returns>
        [HttpPost]
        [Route( "messages/{messageId:guid}" )]
        [Authorize( Policy = Policies.MessagesAnalysisReadWrite )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( AnalysisModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.BadRequest, Type = typeof( ErrorModel ) )]
        public IActionResult AddAnalysisToMessage( 
            Guid messageId, AnalysisCreationRequest request ) {
            Message message = null;
            
            var currentUser = GetCurrentUser();

            return RulesHelper
                .IfMessageIsValid( messageId, out message )
                .IfMessageCanBeAnalyzedAfterMiniumTimePassed( messageId )
                .IfUserIsInMedicalTeam( GetCurrentUser().Id, message.MedicalTeam.Id )
                .Then( () => {
                    var createdAnalysis = _messageAnalysisQueriesService
                        .Create( currentUser.Id, request );

                    SaveChanges();

                    var retrivedAnalysis = _messageAnalysisQueriesService.Get( createdAnalysis.Id );

                    return Ok( AnalysisEntityMapper.Map( retrivedAnalysis, GetCurrentUser().Id ) );
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Delete an analysis from a message
        /// </summary>
        /// <param name="projectId">Project identifier</param>
        /// <param name="medicalTeamId">Medical team identifier</param>
        /// <param name="analysisId">Analysis identifier</param>
        [HttpDelete]
        [Route( "{projectId:guid}/{medicalTeamId:guid}/{analysisId:guid}" )]
        [Authorize( Policy = Policies.MessagesAnalysisReadWrite )]
        [SwaggerResponse( (int)HttpStatusCode.OK )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.BadRequest, Type = typeof( ErrorModel ) )]
        public IActionResult DeleteAnalysisFromMessage(
            Guid projectId, Guid medicalTeamId, Guid analysisId ) {
            MedicalTeam medicalTeam = null;
            Project project = null;

            var currentUser = GetCurrentUser();

            return RulesHelper
                .IfProjectIsValid( projectId, out project )
                .IfProjectHasAnalystConsoleActive( projectId )
                .IfMedicalTeamIsValid( medicalTeamId, out medicalTeam )
                .IfProfessionistIsIntoTheMedicalTeam( currentUser.Id, medicalTeamId, GetCurrentUserRoles() )
                .IfUserCanModifyAnalysis( currentUser.Id, analysisId )
                .IfAnalysisIsInMyInstitute( GetInstituteId(), analysisId )
                .Then( () => {
                    _messageAnalysisQueriesService.Delete( analysisId );

                    SaveChanges();

                    return Ok();
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Update an analysis from a message
        /// </summary>
        /// <param name="projectId">Project identifier</param>
        /// <param name="medicalTeamId">Medical team identifier</param>
        /// <param name="analysisId">Analysis identifier</param>
        /// <param name="request">Data to add analysis to a message</param>
        [HttpPut]
        [Route( "{projectId:guid}/{medicalTeamId:guid}/{analysisId:guid}" )]
        [Authorize( Policy = Policies.MessagesAnalysisReadWrite )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( AnalysisResultModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.BadRequest, Type = typeof( ErrorModel ) )]
        public IActionResult UpdateAnalysisFromMessage(
            Guid projectId, Guid medicalTeamId, Guid analysisId, AnalysisCreationRequest request ) {
            MedicalTeam medicalTeam = null;
            Project project = null;

            var currentUser = GetCurrentUser();

            return RulesHelper
                .IfProjectIsValid( projectId, out project )
                .IfProjectHasAnalystConsoleActive( projectId )
                .IfMedicalTeamIsValid( medicalTeamId, out medicalTeam )
                .IfProfessionistIsIntoTheMedicalTeam( currentUser.Id, medicalTeamId, GetCurrentUserRoles() )
                .IfUserCanModifyAnalysis( currentUser.Id, analysisId )
                .IfAnalysisIsInMyInstitute( GetInstituteId(), analysisId )
                .Then( () => {
                    var updatedAnalysis = _messageAnalysisQueriesService
                        .Update( analysisId, currentUser.Id, request );

                    SaveChanges();

                    var retrivedAnalysis = _messageAnalysisQueriesService.Get( updatedAnalysis.Id );

                    return Ok( AnalysisEntityMapper.Map( retrivedAnalysis, GetCurrentUser().Id ) );
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Get an analysis resume for a message
        /// </summary>
        /// <param name="projectId">Project identifier</param>
        /// <param name="medicalTeamId">Medical team identifier</param>
        /// <param name="messageId">Message identifier</param>
        /// <returns>Message information</returns>
        [HttpGet]
        [Route( "{projectId:guid}/{medicalTeamId:guid}/{messageId:guid}" )]
        [Authorize( Policy = Policies.MessagesAnalysisReadWrite )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( AnalysisResumeModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.BadRequest, Type = typeof( ErrorModel ) )]
        public IActionResult GetMessageAnalysisResume( 
            Guid projectId, Guid medicalTeamId, Guid messageId ) {
            MedicalTeam medicalTeam = null;
            Project project = null;
            ProjectProperties projectProperties = null;
            Message message = null;

            var currentUser = GetCurrentUser();

            return RulesHelper
                .IfProjectIsValid( projectId, out project )
                .IfProjectHasProjectProperties( projectId, out projectProperties )
                .IfProjectHasAnalystConsoleActive( projectId )
                .IfMedicalTeamIsValid( medicalTeamId, out medicalTeam )
                .IfProfessionistIsIntoTheMedicalTeam( currentUser.Id, medicalTeamId, GetCurrentUserRoles() )
                .IfMessageIsValid( messageId, out message )
                .Then( () => {
                    if ( projectProperties.MedicsCanSeeOtherAnalisys ) {
                        var analysisResult = AnalysisEntityMapper
                            .ToAnalysisResume( message.Analysis, GetCurrentUser().Id );
                        return Ok( analysisResult );
                    }
                    else {
                        var analysisResult = AnalysisEntityMapper
                            .ToAnalysisResumeOnlyMine( message.Analysis, GetCurrentUser().Id );
                        return Ok( analysisResult );
                    }
                } )
                .ReturnResult();
        }
    }
}
