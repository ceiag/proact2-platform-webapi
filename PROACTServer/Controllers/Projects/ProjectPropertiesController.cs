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
    public class ProjectPropertiesController : ProactBaseController {
        private readonly IProjectPropertiesQueriesService _projectPropertiesQueriesService;

        public ProjectPropertiesController(
            IProjectPropertiesQueriesService projectPropertiesQueriesService,
            IChangesTrackingService changesTrackingService, ConsistencyRulesHelper consistencyRulesHelper ) 
            : base( changesTrackingService, consistencyRulesHelper ) {
            _projectPropertiesQueriesService = projectPropertiesQueriesService;
        }

        /// <summary>
        /// Creates properties for an existing project
        /// </summary>
        /// <param name="projectId">Project identifier</param>
        /// <param name="request">Body of request</param>
        /// <returns>Project Properties informations</returns>
        [HttpPost]
        [Route( "{projectId:guid}" )]
        [Authorize( Policy = Policies.ProjectsReadWrite )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( ProjectPropertiesModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.BadRequest, Type = typeof( ErrorModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.Conflict, Type = typeof( ErrorModel ) )]
        public IActionResult Create( Guid projectId, ProjectPropertiesCreateRequest request ) {
            Project project = null;

            return RulesHelper
                .IfProjectIsValid( projectId, out project )
                .IfProjectIsOpen( projectId )
                .IfProjectHasNotProjectProperties( projectId )
                .IfProjectIsInMyInstitute( GetCurrentInstitute().Id, project )
                .Then( () => {
                    var projectProps = _projectPropertiesQueriesService.Create( projectId, request );

                    SaveChanges();

                    return Ok( ProjectPropertiesEntityMapper.Map( projectProps ) );
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Update properties for an existing project
        /// </summary>
        /// <param name="projectId">Project identifier</param>
        /// <param name="request">Body of request</param>
        /// <returns>Project Properties informations</returns>
        [HttpPut]
        [Route( "{projectId:guid}" )]
        [Authorize( Policy = Policies.ProjectsReadWrite )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( ProjectPropertiesModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.BadRequest, Type = typeof( ErrorModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.Conflict, Type = typeof( ErrorModel ) )]
        public IActionResult Update( Guid projectId, ProjectPropertiesUpdateRequest request ) {
            Project project = null;
            ProjectProperties projectProperties = null;

            return RulesHelper
                .IfProjectIsValid( projectId, out project )
                .IfProjectIsOpen( projectId )
                .IfProjectHasProjectProperties( projectId, out projectProperties )
                .IfProjectIsInMyInstitute( GetCurrentInstitute().Id, project )
                .Then( () => {
                    var projectProps = _projectPropertiesQueriesService.Update( projectId, request );

                    SaveChanges();

                    return Ok( ProjectPropertiesEntityMapper.Map( projectProps ) );
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Get properties for an existing project
        /// </summary>
        /// <param name="projectId">Project identifier</param>
        /// <returns>Project Properties informations</returns>
        [HttpGet]
        [Route( "{projectId:guid}" )]
        [Authorize( Policy = Policies.ProjectsReadWrite )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( ProjectPropertiesModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.BadRequest, Type = typeof( ErrorModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.Conflict, Type = typeof( ErrorModel ) )]
        public IActionResult Get( Guid projectId ) {
            Project project = null;
            ProjectProperties projectProperties = null;

            return RulesHelper
                .IfProjectIsValid( projectId, out project )
                .IfProjectHasProjectProperties( projectId, out projectProperties )
                .IfProjectIsInMyInstitute( GetCurrentInstitute().Id, project )
                .Then( () => {
                    var projectProps = _projectPropertiesQueriesService.GetByProjectId( projectId );

                    SaveChanges();

                    return Ok( ProjectPropertiesEntityMapper.Map( projectProps ) );
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Add a Lexicon of terms to a Project
        /// </summary>
        /// <param name="projectId">project identifier</param>
        /// <param name="request">Request body for lexicon assignation</param>
        [HttpPut]
        [Route( "{projectId:guid}/AssociateToProject" )]
        [Authorize( Policy = Policies.LexiconReadWrite )]
        [SwaggerResponse( (int)HttpStatusCode.OK )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.BadRequest, Type = typeof( ErrorModel ) )]
        public IActionResult AddLexiconToProject( Guid projectId, LexiconAssignationRequest request  ) {
            Project project = null;
            ProjectProperties properties = null;

            return RulesHelper
                .IfProjectIsValid( projectId, out project )
                .IfProjectHasProjectProperties( projectId, out properties )
                .IfProjectIsOpen( projectId )
                .IfProjectHasNotLexiconAssignedYet( projectId )
                .IfProjectIsInMyInstitute( GetCurrentInstitute().Id, project )
                .Then( () => {
                    _projectPropertiesQueriesService.AddLexicon( projectId, request.LexiconId );

                    SaveChanges();

                    return Ok();
                } )
                .ReturnResult();
        }
    }
}
