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
    public class ProjectContactsController : ProactBaseController {
        private readonly IProjectHtmlContentsQueriesService _projContactsQueriesService;

        public ProjectContactsController(
            IProjectHtmlContentsQueriesService projContactsQueriesService,
            IChangesTrackingService changesTrackingService,
            ConsistencyRulesHelper consistencyRulesHelper )
            : base( changesTrackingService, consistencyRulesHelper ) {
            _projContactsQueriesService = projContactsQueriesService;
        }

        /// <summary>
        /// Upload terms and conditions file for an Institute
        /// </summary>
        /// <param name="projectId">Project Identifier</param>
        /// <param name="request">Body of the request</param>
        /// <returns>contacts information</returns>
        [HttpPost]
        [Route( "{projectId:guid}" )]
        [Authorize( Policy = Policies.MedicalTeamReadWrite )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( ProjectHtmlContentsModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.BadRequest, Type = typeof( ErrorModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        public IActionResult SetContactsForProject( Guid projectId, ProjectHtmlContentCreationRequest request ) {
            Institute institute = null;
            Project project = null;

            return RulesHelper
                .IfInstituteIsValid( GetInstituteId(), out institute )
                .IfProjectIsValid( projectId, out project )
                .IfProjectIsInMyInstitute( institute.Id, project )
                .Then( () => {
                    _projContactsQueriesService.DeleteByProjectId( projectId, ProjectHtmlType.Contacts );

                    var contacts = _projContactsQueriesService.Create( 
                        projectId, ProjectHtmlType.Contacts, request );
                    SaveChanges();

                    return Ok( ProjectHtmlContentsEntityMapper.Map( contacts ) );
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Get project contacts html informations
        /// </summary>
        /// <param name="projectId">Project Identifier</param>
        /// <returns>contacts information</returns>
        [HttpGet]
        [Route( "{projectId:guid}" )]
        [Authorize( Policy = Policies.UsersMeRead )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( ProjectHtmlContentsModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.BadRequest, Type = typeof( ErrorModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        public IActionResult GetContactsForProject( Guid projectId ) {
            Institute institute = null;
            Project project = null;
            ProjectHtmlContent projectContacts = null;

            return RulesHelper
                .IfInstituteIsValid( GetInstituteId(), out institute )
                .IfProjectIsValid( projectId, out project )
                .IfProjectIsInMyInstitute( institute.Id, project )
                .IfProjectHtmlContentIsValid( projectId, ProjectHtmlType.Contacts, out projectContacts )
                .Then( () => {
                    return Ok( ProjectHtmlContentsEntityMapper.Map( projectContacts ) );
                } )
                .ReturnResult();
        }
    }
}
