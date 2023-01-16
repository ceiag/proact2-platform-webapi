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

namespace Proact.Services.Controllers {
    [ApiController]
    [Route( ProactRouteConfiguration.DefaultRoute )]
    [Authorize( Policy = Policies.MedicalTeamReadWrite )]
    public class ProjectEmailsBodiesController : ProactBaseController {
        private readonly IProjectHtmlContentsQueriesService _projContactsQueriesService;

        public ProjectEmailsBodiesController(
            IProjectHtmlContentsQueriesService projContactsQueriesService,
            IChangesTrackingService changesTrackingService,
            ConsistencyRulesHelper consistencyRulesHelper )
            : base( changesTrackingService, consistencyRulesHelper ) {
            _projContactsQueriesService = projContactsQueriesService;
        }

        /// <summary>
        /// Upload user welcome email html content
        /// </summary>
        /// <param name="projectId">Project Identifier</param>
        /// <param name="request">Body of the request</param>
        /// <returns>html body</returns>
        [HttpPost]
        [Route( "{projectId:guid}/UserWelcome" )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( ProjectHtmlContentsModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.BadRequest, Type = typeof( ErrorModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        public IActionResult SetEmailBodyForUserWelcome( 
            Guid projectId, ProjectHtmlContentCreationRequest request ) {
            Institute institute = null;
            Project project = null;

            return RulesHelper
                .IfInstituteIsValid( GetInstituteId(), out institute )
                .IfProjectIsValid( projectId, out project )
                .IfProjectIsInMyInstitute( institute.Id, project )
                .Then( () => {
                    _projContactsQueriesService.DeleteByProjectId( 
                        projectId, ProjectHtmlType.UserWelcomeEmail );

                    var contacts = _projContactsQueriesService.Create(
                        projectId, ProjectHtmlType.UserWelcomeEmail, request );
                    SaveChanges();

                    return Ok( ProjectHtmlContentsEntityMapper.Map( contacts ) );
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Get project html body for user welcome email
        /// </summary>
        /// <param name="projectId">Project Identifier</param>
        /// <returns>html body</returns>
        [HttpGet]
        [Route( "{projectId:guid}/UserWelcome" )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( ProjectHtmlContentsModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.BadRequest, Type = typeof( ErrorModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        public IActionResult GetEmailBodyForUserWelcome( Guid projectId ) {
            Institute institute = null;
            Project project = null;
            ProjectHtmlContent projectContacts = null;

            return RulesHelper
                .IfInstituteIsValid( GetInstituteId(), out institute )
                .IfProjectIsValid( projectId, out project )
                .IfProjectIsInMyInstitute( institute.Id, project )
                .IfProjectHtmlContentIsValid( projectId, ProjectHtmlType.UserWelcomeEmail, out projectContacts )
                .Then( () => {
                    return Ok( ProjectHtmlContentsEntityMapper.Map( projectContacts ) );
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Upload user suspended email html content
        /// </summary>
        /// <param name="projectId">Project Identifier</param>
        /// <param name="request">Body of the request</param>
        /// <returns>html body</returns>
        [HttpPost]
        [Route( "{projectId:guid}/UserSuspended" )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( ProjectHtmlContentsModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.BadRequest, Type = typeof( ErrorModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        public IActionResult SetEmailBodyForUserSuspended(
            Guid projectId, ProjectHtmlContentCreationRequest request ) {
            Institute institute = null;
            Project project = null;

            return RulesHelper
                .IfInstituteIsValid( GetInstituteId(), out institute )
                .IfProjectIsValid( projectId, out project )
                .IfProjectIsInMyInstitute( institute.Id, project )
                .Then( () => {
                    _projContactsQueriesService.DeleteByProjectId(
                        projectId, ProjectHtmlType.UserSuspendedEmail );

                    var contacts = _projContactsQueriesService.Create(
                        projectId, ProjectHtmlType.UserSuspendedEmail, request );
                    SaveChanges();

                    return Ok( ProjectHtmlContentsEntityMapper.Map( contacts ) );
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Get project html body for user suspended email
        /// </summary>
        /// <param name="projectId">Project Identifier</param>
        /// <returns>html body</returns>
        [HttpGet]
        [Route( "{projectId:guid}/UserSuspended" )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( ProjectHtmlContentsModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.BadRequest, Type = typeof( ErrorModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        public IActionResult GetEmailBodyForUserSuspended( Guid projectId ) {
            Institute institute = null;
            Project project = null;
            ProjectHtmlContent projectContacts = null;

            return RulesHelper
                .IfInstituteIsValid( GetInstituteId(), out institute )
                .IfProjectIsValid( projectId, out project )
                .IfProjectIsInMyInstitute( institute.Id, project )
                .IfProjectHtmlContentIsValid( projectId, ProjectHtmlType.UserSuspendedEmail, out projectContacts )
                .Then( () => {
                    return Ok( ProjectHtmlContentsEntityMapper.Map( projectContacts ) );
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Upload user deactivated email html content
        /// </summary>
        /// <param name="projectId">Project Identifier</param>
        /// <param name="request">Body of the request</param>
        /// <returns>html body</returns>
        [HttpPost]
        [Route( "{projectId:guid}/UserDeactivated" )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( ProjectHtmlContentsModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.BadRequest, Type = typeof( ErrorModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        public IActionResult SetEmailBodyForUserDeactivated(
            Guid projectId, ProjectHtmlContentCreationRequest request ) {
            Institute institute = null;
            Project project = null;

            return RulesHelper
                .IfInstituteIsValid( GetInstituteId(), out institute )
                .IfProjectIsValid( projectId, out project )
                .IfProjectIsInMyInstitute( institute.Id, project )
                .Then( () => {
                    _projContactsQueriesService.DeleteByProjectId(
                        projectId, ProjectHtmlType.UserDeactivatedEmail );

                    var contacts = _projContactsQueriesService.Create(
                        projectId, ProjectHtmlType.UserDeactivatedEmail, request );
                    SaveChanges();

                    return Ok( ProjectHtmlContentsEntityMapper.Map( contacts ) );
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Get project html body for user deactivated email
        /// </summary>
        /// <param name="projectId">Project Identifier</param>
        /// <returns>html body</returns>
        [HttpGet]
        [Route( "{projectId:guid}/UserDeactivated" )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( ProjectHtmlContentsModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.BadRequest, Type = typeof( ErrorModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        public IActionResult GetEmailBodyForUserDeactivated( Guid projectId ) {
            Institute institute = null;
            Project project = null;
            ProjectHtmlContent projectContacts = null;

            return RulesHelper
                .IfInstituteIsValid( GetInstituteId(), out institute )
                .IfProjectIsValid( projectId, out project )
                .IfProjectIsInMyInstitute( institute.Id, project )
                .IfProjectHtmlContentIsValid( projectId, ProjectHtmlType.UserDeactivatedEmail, out projectContacts )
                .Then( () => {
                    return Ok( ProjectHtmlContentsEntityMapper.Map( projectContacts ) );
                } )
                .ReturnResult();
        }
    }
}
