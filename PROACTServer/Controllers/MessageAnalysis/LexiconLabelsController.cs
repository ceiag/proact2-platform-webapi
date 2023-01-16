using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proact.Services.AuthorizationPolicies;
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
    public class LexiconLabelController : ProactBaseController {
        private readonly ILexiconLabelQueriesService _lexiconLabelsQueriesService;

        public LexiconLabelController(
            ILexiconLabelQueriesService lexiconLabelsQueriesService,
            IChangesTrackingService changesTrackingService, ConsistencyRulesHelper consistencyRulesHelper )
            : base( changesTrackingService, consistencyRulesHelper ) {
            _lexiconLabelsQueriesService = lexiconLabelsQueriesService;
        }

        /// <summary>
        /// Add a Label to Category inside a existing Lexicon
        /// </summary>
        /// <param name="lexiconId">Lexicon identifier</param>
        /// <param name="categoryId">Category identifier</param>
        /// <param name="request">Body of the request</param>
        [HttpPost]
        [Route( "{lexiconId:guid}/{categoryId:guid}" )]
        [Authorize( Policy = Policies.LexiconReadWrite )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( LexiconLabelModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.BadRequest, Type = typeof( ErrorModel ) )]
        public IActionResult AddLexiconLabel( 
            Guid lexiconId, Guid categoryId, LexiconLabelCreationRequest request ) {
            Lexicon lexicon = null;
            LexiconCategory category = null;

            return RulesHelper
                .IfLexiconIsValid( lexiconId, out lexicon )
                .IfLexiconCategoryIsValid( categoryId, out category )
                .IfLexiconLabelNameIsAvailable( categoryId, request.Label )
                .Then( () => {
                    var labelCreated = LexiconEntityMapper.Map( 
                        _lexiconLabelsQueriesService.Create( category, request ) );

                    SaveChanges();

                    return Ok( labelCreated );
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Update a Label to Category inside a existing Lexicon
        /// </summary>
        /// <param name="lexiconId">Lexicon identifier</param>
        /// <param name="categoryId">Category identifier</param>
        /// <param name="labelId">Label identifier</param>
        /// <param name="request">Body of the request</param>
        [HttpPut]
        [Route( "{lexiconId:guid}/{categoryId:guid}/{labelId:guid}" )]
        [Authorize( Policy = Policies.LexiconReadWrite )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( LexiconLabelModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.BadRequest, Type = typeof( ErrorModel ) )]
        public IActionResult UpdateLexiconLabel(
            Guid lexiconId, Guid categoryId, Guid labelId, LexiconLabelUpdateRequest request ) {
            Lexicon lexicon = null;
            LexiconCategory category = null;

            return RulesHelper
                .IfLexiconIsValid( lexiconId, out lexicon )
                .IfLexiconCategoryIsValid( categoryId, out category )
                .IfLexiconLabelNameIsAvailableForUpdate( categoryId, labelId, request.Label )
                .IfLexiconIsNotPublished( lexiconId )
                .Then( () => {
                    var updatedLabel = _lexiconLabelsQueriesService.Update( labelId, request );

                    SaveChanges();

                    return Ok( LexiconEntityMapper.Map( updatedLabel ) );
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Get a Label from a Category inside a existing Lexicon
        /// </summary>
        /// <param name="lexiconId">Lexicon identifier</param>
        /// <param name="categoryId">Category identifier</param>
        /// <param name="labelId">Label identifier</param>
        [HttpGet]
        [Route( "{lexiconId:guid}/{categoryId:guid}/{labelId:guid}" )]
        [Authorize( Policy = Policies.LexiconReadWrite )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( LexiconLabelModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.BadRequest, Type = typeof( ErrorModel ) )]
        public IActionResult GetLexiconLabel(
            Guid lexiconId, Guid categoryId, Guid labelId ) {
            Lexicon lexicon = null;
            LexiconCategory category = null;
            LexiconLabel label = null;

            return RulesHelper
                .IfLexiconIsValid( lexiconId, out lexicon )
                .IfLexiconCategoryIsValid( categoryId, out category )
                .IfLexiconLabelIsValid( labelId, out label )
                .Then( () => {
                    var updatedLabel = _lexiconLabelsQueriesService.Get( labelId );

                    SaveChanges();

                    return Ok( LexiconEntityMapper.Map( label ) );
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Get all Labela from Category inside a existing Lexicon
        /// </summary>
        /// <param name="lexiconId">Lexicon identifier</param>
        /// <param name="categoryId">Category identifier</param>
        [HttpGet]
        [Route( "{lexiconId:guid}/{categoryId:guid}" )]
        [Authorize( Policy = Policies.LexiconReadWrite )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( LexiconLabelModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.BadRequest, Type = typeof( ErrorModel ) )]
        public IActionResult GetAllLexiconLabel( Guid lexiconId, Guid categoryId ) {
            Lexicon lexicon = null;
            LexiconCategory category = null;

            return RulesHelper
                .IfLexiconIsValid( lexiconId, out lexicon )
                .IfLexiconCategoryIsValid( categoryId, out category )
                .Then( () => {
                    return Ok( LexiconEntityMapper.Map(
                        _lexiconLabelsQueriesService.GetAll( categoryId ) ) );
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Delete a Label from a Category inside a existing Lexicon
        /// </summary>
        /// <param name="lexiconId">Lexicon identifier</param>
        /// <param name="categoryId">Category identifier</param>
        /// <param name="labelId">Label identifier</param>
        [HttpDelete]
        [Route( "{lexiconId:guid}/{categoryId:guid}/{labelId:guid}" )]
        [Authorize( Policy = Policies.LexiconReadWrite )]
        [SwaggerResponse( (int)HttpStatusCode.OK )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.BadRequest, Type = typeof( ErrorModel ) )]
        public IActionResult DeleteLexiconLabel(
            Guid lexiconId, Guid categoryId, Guid labelId ) {
            Lexicon lexicon = null;
            LexiconCategory category = null;
            LexiconLabel label = null;

            return RulesHelper
                .IfLexiconIsValid( lexiconId, out lexicon )
                .IfLexiconCategoryIsValid( categoryId, out category )
                .IfLexiconLabelIsValid( labelId, out label )
                .IfLexiconIsNotPublished( lexiconId )
                .Then( () => {
                    _lexiconLabelsQueriesService.Delete( labelId );

                    SaveChanges();

                    return Ok();
                } )
                .ReturnResult();
        }
    }
}
