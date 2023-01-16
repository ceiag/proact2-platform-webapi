using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proact.Services.AuthorizationPolicies;
using Proact.Services.Entities;
using Proact.Services.Entities.MessageAnalysis;
using Proact.Services.EntitiesMapper;
using Proact.Services.Models;
using Proact.Services.QueriesServices;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Net;

namespace Proact.Services.Controllers {
    [ApiController]
    [Route( ProactRouteConfiguration.DefaultRoute )]
    public class LexiconsController : ProactBaseController {
        private readonly ILexiconQueriesService _lexiconQueriesService;

        public LexiconsController( 
            ILexiconQueriesService lexiconQueriesService,
            IChangesTrackingService changesTrackingService, ConsistencyRulesHelper consistencyRulesHelper ) 
            : base( changesTrackingService, consistencyRulesHelper ) {
            _lexiconQueriesService = lexiconQueriesService;
        }

        /// <summary>
        /// Create a Lexicon of terms
        /// </summary>
        /// <param name="request">Data to create Lexicon</param>
        /// <returns>Message information</returns>
        [HttpPost]
        [Route( "create" )]
        [Authorize( Policy = Policies.LexiconReadWrite )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( LexiconModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.BadRequest, Type = typeof( ErrorModel ) )]
        public IActionResult CreateLexicon( LexiconCreationRequest request ) {
            return RulesHelper
                .Then( () => {
                    var lexiconCreated = LexiconEntityMapper
                        .Map( _lexiconQueriesService.Create( GetInstituteId(), request ) );

                    SaveChanges();

                    return Ok( lexiconCreated );
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Get Lexicon of terms
        /// </summary>
        /// <param name="lexiconId">Lexicon identifier</param>
        [HttpGet]
        [Route("{lexiconId:guid}")]
        [Authorize( Policy = Policies.LexiconRead )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( LexiconModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.BadRequest, Type = typeof( ErrorModel ) )]
        public IActionResult GetLexicon( Guid lexiconId ) {
            Lexicon lexicon = null;

            return RulesHelper
                .IfLexiconIsValid( lexiconId, out lexicon )
                .IfLexiconIsInMyInstitute( GetInstituteId(), lexiconId )
                .Then( () => {
                    return Ok( LexiconEntityMapper.Map( lexicon ) );
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Get Lexicon of terms associated to a Project
        /// </summary>
        /// <param name="projectId">Project identifier</param>
        [HttpGet]
        [Route( "projects/{projectId:guid}" )]
        [Authorize( Policy = Policies.LexiconRead )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( LexiconModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.BadRequest, Type = typeof( ErrorModel ) )]
        public IActionResult GetLexiconFromProjectId( Guid projectId ) {
            Project project = null;
            ProjectProperties projectProps = null;

            return RulesHelper
                .IfProjectIsValid( projectId, out project )
                .IfProjectHasProjectProperties( projectId, out projectProps )
                .IfUserIsInProject( GetCurrentUser().Id, projectId )
                .Then( () => {
                    return Ok( LexiconEntityMapper.Map( project.ProjectProperties.Lexicon ) );
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Set Lexicon as published
        /// </summary>
        /// <param name="request">Body of the request</param>
        [HttpPut]
        [Route( "{lexiconId:guid}/publish" )]
        [Authorize( Policy = Policies.LexiconReadWrite )]
        [SwaggerResponse( (int)HttpStatusCode.OK )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.BadRequest, Type = typeof( ErrorModel ) )]
        public IActionResult SetLexiconAsPublished( LexiconPublishRequest request ) {
            Lexicon lexicon = null;

            return RulesHelper
                .IfLexiconIsValid( request.LexiconId, out lexicon )
                .IfLexiconIsNotPublished( request.LexiconId )
                .IfLexiconIsInMyInstitute( GetInstituteId(), lexicon.Id )
                .Then( () => {
                    _lexiconQueriesService.PublishLexicon( request.LexiconId );

                    SaveChanges();

                    return Ok();
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Get Lexicons of terms prenset into the system
        /// </summary>
        [HttpGet]
        [Authorize( Policy = Policies.LexiconRead )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( List<LexiconModel> ) )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.BadRequest, Type = typeof( ErrorModel ) )]
        public IActionResult GetLexicons() {
            return RulesHelper
                .Then( () => {
                    return Ok( LexiconEntityMapper.MapHideCategories( 
                        _lexiconQueriesService.GetAll( GetInstituteId() ) ) );
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Delete a Lexicon if is in a draw state
        /// </summary>
        /// <param name="lexiconId">Lexicon identifier</param>
        [HttpDelete]
        [Route( "{lexiconId:guid}" )]
        [Authorize( Policy = Policies.LexiconReadWrite )]
        [SwaggerResponse( (int)HttpStatusCode.OK )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.BadRequest, Type = typeof( ErrorModel ) )]
        public IActionResult DeleteLexicon( Guid lexiconId ) {
            Lexicon lexicon = null;

            return RulesHelper
                .IfLexiconIsValid( lexiconId, out lexicon )
                .IfLexiconIsNotPublished( lexiconId )
                .IfLexiconIsInMyInstitute( GetInstituteId(), lexiconId )
                .Then( () => {
                    _lexiconQueriesService.Delete( lexiconId );

                    SaveChanges();

                    return Ok();
                } )
                .ReturnResult();
        }
    }
}
