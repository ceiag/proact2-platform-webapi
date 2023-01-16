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
    public class LexiconsCategoryController : ProactBaseController {
        private readonly ILexiconCategoriesQueriesService _lexiconCategoryQueriesService;

        public LexiconsCategoryController(
            ILexiconCategoriesQueriesService lexiconCategoryQueriesService,
            IChangesTrackingService changesTrackingService, ConsistencyRulesHelper consistencyRulesHelper )
            : base( changesTrackingService, consistencyRulesHelper ) {
            _lexiconCategoryQueriesService = lexiconCategoryQueriesService;
        }

        /// <summary>
        /// Add a Category inside a existing Lexicon
        /// </summary>
        /// <param name="request">Data for adding new Category</param>
        /// <param name="lexiconId">Lexicon identifier</param>
        [HttpPost]
        [Route( "{lexiconId:guid}" )]
        [Authorize( Policy = Policies.LexiconReadWrite )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( LexiconCategoryModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.BadRequest, Type = typeof( ErrorModel ) )]
        public IActionResult AddLexiconCategory( Guid lexiconId, LexiconCategoryAdditionRequest request ) {
            Lexicon lexicon = null;

            return RulesHelper
                .IfLexiconIsValid( lexiconId, out lexicon )
                .IfLexiconCategoryNameIsAvailable( lexicon, request.Name )
                .Then( () => {
                    _lexiconCategoryQueriesService.Add( lexicon, request );

                    SaveChanges();

                    return Ok( LexiconEntityMapper.Map(
                        _lexiconCategoryQueriesService.GetByName( lexicon, request.Name ) ) );
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Set Ordering of categories
        /// </summary>
        /// <param name="lexiconId">Lexicon identifier</param>
        /// <param name="request">The order of categories defined by id</param>
        [HttpPut]
        [Route( "{lexiconId:guid}" )]
        [Authorize( Policy = Policies.LexiconReadWrite )]
        [SwaggerResponse( (int)HttpStatusCode.OK )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.BadRequest, Type = typeof( ErrorModel ) )]
        public IActionResult SetOrderingOfCategories( 
            Guid lexiconId, LexiconCategorySetOrderingRequest request ) {
            Lexicon lexicon = null;

            return RulesHelper
                .IfLexiconIsValid( lexiconId, out lexicon )
                .Then( () => {
                    try {
                        _lexiconCategoryQueriesService.SetOrdering( request );

                        SaveChanges();
                        return Ok();
                    }
                    catch ( Exception ex ) {
                        return BadRequest( ex.Message );
                    }
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Update category informations
        /// </summary>
        /// <param name="lexiconId">Lexicon identifier</param>
        /// <param name="categoryId">Category identifier</param>
        /// <param name="request">Data for update Category</param>
        [HttpPut]
        [Route( "{lexiconId:guid}/{categoryId:guid}" )]
        [Authorize( Policy = Policies.LexiconReadWrite )]
        [SwaggerResponse( (int)HttpStatusCode.OK )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.BadRequest, Type = typeof( ErrorModel ) )]
        public IActionResult UpdateCategory(
            Guid lexiconId, Guid categoryId, LexiconCategoryUpdateRequest request ) {
            Lexicon lexicon = null;
            LexiconCategory category = null;

            return RulesHelper
                .IfLexiconIsValid( lexiconId, out lexicon )
                .IfLexiconCategoryIsValid( categoryId, out category )
                .IfLexiconIsNotPublished( lexiconId )
                .Then( () => {
                    try {
                        var updatedCategory = _lexiconCategoryQueriesService
                            .Update( categoryId, request );

                        SaveChanges();
                        return Ok( LexiconEntityMapper.Map( updatedCategory ) );
                    }
                    catch ( Exception ex ) {
                        return BadRequest( ex.Message );
                    }
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Get Category from a Lexicon
        /// </summary>
        /// <param name="lexiconId">Lexicon identifier</param>
        /// <param name="categoryId">Category identifier</param>
        [HttpGet]
        [Route( "{lexiconId:guid}/{categoryId:guid}" )]
        [Authorize( Policy = Policies.LexiconRead )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( LexiconCategoryModel  ) )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.BadRequest, Type = typeof( ErrorModel ) )]
        public IActionResult GetCategory( Guid lexiconId, Guid categoryId ) {
            Lexicon lexicon = null;
            LexiconCategory category = null;

            return RulesHelper
                .IfLexiconIsValid( lexiconId, out lexicon )
                .IfLexiconCategoryIsValid( categoryId, out category )
                .Then( () => {
                    return Ok( LexiconEntityMapper
                        .Map( _lexiconCategoryQueriesService.Get( categoryId ) ) );
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Get Categories from a Lexicon
        /// </summary>
        /// <param name="lexiconId">Lexicon identifier</param>
        [HttpGet]
        [Route( "{lexiconId:guid}" )]
        [Authorize( Policy = Policies.LexiconRead )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( List<LexiconCategoryModel> ) )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.BadRequest, Type = typeof( ErrorModel ) )]
        public IActionResult GetCategories( Guid lexiconId ) {
            Lexicon lexicon;

            return RulesHelper
                .IfLexiconIsValid( lexiconId, out lexicon )
                .Then( () => {
                    return Ok( LexiconEntityMapper.Map(
                        _lexiconCategoryQueriesService.GetAll( lexicon ) ) );
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Delete Category from a Lexicon
        /// </summary>
        /// <param name="lexiconId">Lexicon identifier</param>
        /// <param name="categoryId">Category identifier</param>
        [HttpDelete]
        [Route( "{lexiconId:guid}/{categoryId:guid}" )]
        [Authorize( Policy = Policies.LexiconRead )]
        [SwaggerResponse( (int)HttpStatusCode.OK )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.BadRequest, Type = typeof( ErrorModel ) )]
        public IActionResult DeleteCategory( Guid lexiconId, Guid categoryId ) {
            Lexicon lexicon;
            LexiconCategory category;

            return RulesHelper
                .IfLexiconIsValid( lexiconId, out lexicon )
                .IfLexiconCategoryIsValid( categoryId, out category )
                .IfLexiconIsNotPublished( lexiconId )
                .Then( () => {
                    _lexiconCategoryQueriesService.Delete( categoryId );

                    SaveChanges();

                    return Ok();
                } )
                .ReturnResult();
        }
    }
}
