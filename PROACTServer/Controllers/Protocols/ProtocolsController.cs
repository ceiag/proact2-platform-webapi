using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proact.Services.AuthorizationPolicies;
using Proact.Services.Entities;
using Proact.Services.Models;
using Proact.Services.QueriesServices;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Net;

namespace Proact.Services.Controllers.Institutes {
    [ApiController]
    [Route( ProactRouteConfiguration.DefaultRoute )]
    public class ProtocolsController : ProactBaseController {
        private readonly IProtocolStorageService _protocolStorageService;

        public ProtocolsController(
            IProtocolStorageService projectProtocolStorageService,
            IChangesTrackingService changesTrackingService,
            ConsistencyRulesHelper consistencyRulesHelper )
            : base( changesTrackingService, consistencyRulesHelper ) {
            _protocolStorageService = projectProtocolStorageService;
        }

        /// <summary>
        /// Upload protocol pdf file to the project
        /// </summary>
        /// <param name="projectId">Project indentifier</param>
        /// <param name="request">Body of the request</param>
        /// <returns>Protocol information</returns>
        [HttpPost]
        [Route( "project/{projectId:guid}" )]
        [Authorize( Policy = Policies.MedicalTeamReadWrite )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( ProtocolModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.BadRequest, Type = typeof( ErrorModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.Conflict, Type = typeof( ErrorModel ) )]
        public IActionResult AddProtocolToProject(
            Guid projectId, [FromForm] ProtocolCreationRequest request ) {
            Institute institute = null;
            Project project = null;

            return RulesHelper
                .IfInstituteIsValid( GetInstituteId(), out institute )
                .IfUserIsInMyInstitute( institute.Id, GetCurrentUser() )
                .IfProjectIsValid( projectId, out project )
                .IfProjectIsOpen( projectId )
                .Then( async () => {
                    try {
                        var documentCreated = await _protocolStorageService
                            .AddProtocolToProjectOverrideIfExist( projectId, request.pdfFile, request );
                        SaveChanges();

                        return Ok( documentCreated );
                    }
                    catch ( Exception e ) {
                        return BadRequest( e.Message );
                    }
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Upload protocol pdf file to the patient
        /// </summary>
        /// <param name="userId">User indentifier</param>
        /// <param name="request">Body of the request</param>
        /// <returns>Protocol information</returns>
        [HttpPost]
        [Route( "patient/{userId:guid}" )]
        [Authorize( Policy = Policies.MedicalTeamReadWrite )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( ProtocolModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.BadRequest, Type = typeof( ErrorModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.Conflict, Type = typeof( ErrorModel ) )]
        public IActionResult AddProtocolToPatient(
            Guid userId, [FromForm] ProtocolCreationRequest request ) {
            Institute institute = null;
            User user = null;

            return RulesHelper
                .IfUserIsValid( userId, out user )
                .IfInstituteIsValid( GetInstituteId(), out institute )
                .IfUserIsInMyInstitute( institute.Id, GetCurrentUser() )
                .Then( async () => {
                    try {
                        var documentCreated = await _protocolStorageService
                            .AddProtocolToPatientOverrideIfExist( userId, request.pdfFile, request );
                        SaveChanges();

                        return Ok( documentCreated );
                    }
                    catch ( Exception e ) {
                        return BadRequest( e.Message );
                    }
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Get protocol pdf file url from a patient
        /// </summary>
        /// <param name="userId">User indentifier</param>
        /// <returns>Protocol information</returns>
        [HttpGet]
        [Route( "patient/{userId:guid}" )]
        [Authorize( Policy = Policies.MedicalTeamReadWrite )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( ProtocolModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.BadRequest, Type = typeof( ErrorModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.Conflict, Type = typeof( ErrorModel ) )]
        public IActionResult GetProtocolFromPatient( Guid userId ) {
            Institute institute = null;
            User user = null;

            return RulesHelper
                .IfUserIsValid( userId, out user )
                .IfInstituteIsValid( GetInstituteId(), out institute )
                .IfUserIsInMyInstitute( institute.Id, GetCurrentUser() )
                .Then( () => {
                    try {
                        return Ok( _protocolStorageService.GetByUserId( userId ) );
                    }
                    catch {
                        return new NotFoundObjectResult( $"No protocol associated to user {userId}" );
                    }
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Get protocol pdf file url from me as patient
        /// </summary>
        /// <returns>Protocol information</returns>
        [HttpGet]
        [Route( "me" )]
        [Authorize( Policy = Policies.UsersMeRead )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( ProtocolModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.BadRequest, Type = typeof( ErrorModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.Conflict, Type = typeof( ErrorModel ) )]
        public IActionResult GetUserProtocols() {
            return RulesHelper
                .Then( () => {
                    try {
                        return Ok( _protocolStorageService.GetUserProtocols( GetCurrentUser().Id ) );
                    }
                    catch {
                        return new NotFoundObjectResult( $"No protocol associated to user {GetCurrentUser().Id}" );
                    }
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Get protocol pdf file url from a project
        /// </summary>
        /// <param name="projectId">Project indentifier</param>
        /// <returns>Protocol information</returns>
        [HttpGet]
        [Route( "project/{projectId:guid}" )]
        [Authorize( Policy = Policies.MedicalTeamReadWrite )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( ProtocolModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.BadRequest, Type = typeof( ErrorModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.Conflict, Type = typeof( ErrorModel ) )]
        public IActionResult GetProtocolFromProject( Guid projectId ) {
            Institute institute = null;
            Project project = null;

            return RulesHelper
                .IfProjectIsValid( projectId, out project )
                .IfInstituteIsValid( GetInstituteId(), out institute )
                .IfUserIsInMyInstitute( institute.Id, GetCurrentUser() )
                .Then( () => {
                    try {
                        return Ok( _protocolStorageService.GetByProjectId( projectId ) );
                    }
                    catch {
                        return new NotFoundObjectResult( $"Protocol for project {projectId} not found" );
                    }
                } )
                .ReturnResult();
        }
    }
}
