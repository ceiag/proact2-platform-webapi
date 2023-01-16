using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proact.Services.AuthorizationPolicies;
using Proact.Services.Entities;
using Proact.Services.Models;
using Proact.Services.QueriesServices;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Linq;
using System.Net;

namespace Proact.Services.Controllers {

    [ApiController]
    [Route( ProactRouteConfiguration.DefaultRoute )]
    public class ProjectsController : ProactBaseController {
        private readonly IProjectQueriesService _projectQueriesService;
        private readonly IProjectStateEditorService _projectEditorService;
        private readonly IProjectPropertiesQueriesService _projectPropertiesQueriesService;
        private readonly IInstitutesQueriesService _institutesQueriesService;

        public ProjectsController(
            IChangesTrackingService changesTrackingService,
            ConsistencyRulesHelper consistencyRulesHelper, 
            IProjectQueriesService projectQueriesService,
            IProjectPropertiesQueriesService projectPropertiesQueriesService,
            IInstitutesQueriesService institutesQueriesService,
            IProjectStateEditorService projectEditorService )
            : base( changesTrackingService, consistencyRulesHelper ) {
            _projectQueriesService = projectQueriesService;
            _projectPropertiesQueriesService = projectPropertiesQueriesService;
            _projectEditorService = projectEditorService;
            _institutesQueriesService = institutesQueriesService;
        }

        /// <summary>
        /// Creates a new project in Open state"
        /// </summary>
        /// <param name="projectCreateRequest">Project information</param>
        /// <returns>Project information</returns>
        /// <response code="201">Returns the newly created item</response>
        [HttpPost]
        [Authorize( Policy = Policies.ProjectsReadWrite )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( ProjectModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.BadRequest, Type = typeof( ErrorModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.Conflict, Type = typeof( ErrorModel ) )]
        public IActionResult CreateProject( ProjectCreateRequest projectCreateRequest ) {
            return RulesHelper
                .Then( () => {
                    var instituteWhereImAdmin = _institutesQueriesService
                        .GetWhereImAdmin( GetCurrentUser().Id );

                    var project = _projectQueriesService
                        .Create( instituteWhereImAdmin.Id, projectCreateRequest );
                    SaveChanges();

                    var projectProperties = _projectPropertiesQueriesService
                        .Create( project.Id, projectCreateRequest.Properties );
                    SaveChanges();

                    return Ok( ProjectEntityMapper.Map( project ) );
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Update project information
        /// </summary>
        /// <param name="projectId">Project identifier</param>
        /// <param name="projectUpdateRequest">New project information</param>
        /// <returns>Project information</returns>
        [HttpPut]
        [Authorize( Policy = Policies.ProjectsReadWrite )]
        [Route( "{projectId:guid}" )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( ProjectModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.BadRequest, Type = typeof( ErrorModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        public IActionResult UpdateProject( Guid projectId, ProjectUpdateRequest projectUpdateRequest ) {
            Project project = null;

            return RulesHelper
                .IfProjectIsValid( projectId, out project )
                .IfProjectIsInMyInstitute( GetCurrentInstitute().Id, project )
                .Then( () => {
                    var updatedProject = _projectQueriesService.Update( projectId, projectUpdateRequest );

                    if ( projectUpdateRequest.Status == ProjectState.Closed ) {
                        _projectEditorService.CloseProject( projectId );
                    }
                    else {
                        _projectEditorService.OpenProject( projectId );
                    }

                    SaveChanges();

                    return Ok( ProjectEntityMapper.Map( updatedProject ) );
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Close Project
        /// </summary>
        /// <param name="projectId">Project identifier</param>
        [HttpPut]
        [Authorize( Policy = Policies.ProjectsReadWrite )]
        [Route( "{projectId:guid}/close" )]
        [SwaggerResponse( (int)HttpStatusCode.OK )]
        [SwaggerResponse( (int)HttpStatusCode.BadRequest, Type = typeof( ErrorModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        public IActionResult CloseProject( Guid projectId ) {
            Project project = null;

            return RulesHelper
                .IfProjectIsValid( projectId, out project )
                .IfProjectIsInMyInstitute( GetCurrentInstitute().Id, project )
                .IfProjectIsOpen( projectId )
                .Then( () => {
                    _projectEditorService.CloseProject( projectId );

                    SaveChanges();

                    return Ok();
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Open Project
        /// </summary>
        /// <param name="projectId">Project identifier</param>
        [HttpPut]
        [Authorize( Policy = Policies.ProjectsReadWrite )]
        [Route( "{projectId:guid}/open" )]
        [SwaggerResponse( (int)HttpStatusCode.OK )]
        [SwaggerResponse( (int)HttpStatusCode.BadRequest, Type = typeof( ErrorModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        public IActionResult ReOpenProject( Guid projectId ) {
            Project project = null;

            return RulesHelper
                .IfProjectIsValid( projectId, out project )
                .IfProjectIsInMyInstitute( GetCurrentInstitute().Id, project )
                .Then( () => {
                    _projectEditorService.OpenProject( projectId );

                    SaveChanges();

                    return Ok();
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Gets the project info by its identifier
        /// </summary>
        /// <param name="projectId">The identifier of the project</param>
        /// <returns>Project information</returns>
        [HttpGet]
        [Authorize( Policy = Policies.ProjectsRead )]
        [Route( "{projectId:guid}" )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( ProjectModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.BadRequest, Type = typeof( ErrorModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        public IActionResult GetProject( Guid projectId ) {
            Project project = null;

            return RulesHelper
                .IfProjectIsValid( projectId, out project )
                .IfProjectIsInMyInstitute( GetCurrentInstitute().Id, project )
                .Then( () => {
                    return Ok( ProjectEntityMapper.Map( project ) );
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Provides a list of projects current user participates in
        /// </summary>
        /// <returns>List of projects</returns>
        [HttpGet]
        [Authorize( Policy = Policies.ProjectsRead )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( ProjectModel[] ) )]
        [SwaggerResponse( (int)HttpStatusCode.Forbidden, Type = typeof( ErrorModel ) )]
        public IActionResult GetProjectsWhereImAssociated() {
            var userId = GetCurrentUser().Id;

            if ( HasRoleOf( Roles.InstituteAdmin ) ) {
                return Ok( ProjectEntityMapper.Map( 
                    _projectQueriesService.GetsAll( GetCurrentInstitute().Id ) ) );
            }
            else {
                return Ok( ProjectEntityMapper.Map(
                    _projectQueriesService.GetProjectsWhereUserIsAssociated( userId ) ) );
            }
        }

        /// <summary>
        /// Provides a list of projects current user participates in
        /// </summary>
        /// <returns>List of projects</returns>
        [HttpGet]
        [Route( "all" )]
        [Authorize( Policy = Policies.ProjectsRead )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( ProjectModel[] ) )]
        [SwaggerResponse( (int)HttpStatusCode.Forbidden, Type = typeof( ErrorModel ) )]
        public IActionResult GetProjectsAll() {
            return Ok( ProjectEntityMapper.Map( 
                _projectQueriesService.GetsAll( GetCurrentInstitute().Id ) ) );
        }

        /// <summary>
        /// Get info of user administrating specified project
        /// </summary>
        /// <param name="projectId">Project identifier</param>
        /// <returns>Administrator information</returns>
        [HttpGet]
        [Authorize( Policy = Policies.ProjectsReadWrite )]
        [Route( "{projectId:guid}/admin" )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( UserModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        public IActionResult GetProjectAdmin( Guid projectId ) {
            Project project = null;

            return RulesHelper
                .IfProjectIsValid( projectId, out project )
                .IfProjectIsOpen( projectId )
                .Then( () => {
                    return Ok( UserEntityMapper.Map(
                        _projectQueriesService.Get( projectId ).Admin ) );
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Assign a user as project administrator
        /// </summary>
        /// <param name="projectId">Project identifier</param>
        /// <param name="userId">UserId</param>
        /// <returns>Administrator information</returns>
        [HttpPut]
        [Authorize( Policy = Policies.ProjectsReadWrite )]
        [Route( "{projectId:guid}/admin" )]
        [SwaggerResponse( (int)HttpStatusCode.OK )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.BadRequest, Type = typeof( ErrorModel ) )]
        public IActionResult AssignAdminToProject( Guid projectId, Guid userId ) {
            Project project = null;
            User user = null;
            Medic medic = null;

            return RulesHelper
                .IfProjectIsValid( projectId, out project )
                .IfProjectIsInMyInstitute( GetCurrentInstitute().Id, project )
                .IfProjectIsOpen( projectId )
                .IfUserIsValid( userId, out user )
                .IfUserIsInMyInstitute( GetCurrentInstitute().Id, user )
                .IfMedicIsValid( userId, out medic )
                .Then( () => {
                    _projectQueriesService.AssignAdmin( projectId, medic );

                    SaveChanges();

                    return Ok();
                } )
                .ReturnResult();
        }
    }
}
