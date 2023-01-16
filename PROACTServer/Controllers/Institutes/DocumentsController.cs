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
    [Authorize( Policy = Policies.InstitutesMyReadWrite )]
    [Route( ProactRouteConfiguration.DefaultRoute )]
    public class DocumentsController : ProactBaseController {
        private readonly IDocumentsStorageService _documentsStorageService;

        public DocumentsController(
            IDocumentsStorageService documentsStorageService,
            IChangesTrackingService changesTrackingService,
            ConsistencyRulesHelper consistencyRulesHelper )
            : base( changesTrackingService, consistencyRulesHelper ) {
            _documentsStorageService = documentsStorageService;
        }

        /// <summary>
        /// Upload terms and conditions file for an Institute
        /// </summary>
        /// <param name="request">Body of the request</param>
        /// <returns>Institute information</returns>
        [HttpPost]
        [Route( "terms" )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( DocumentModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.BadRequest, Type = typeof( ErrorModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.Conflict, Type = typeof( ErrorModel ) )]
        public IActionResult UploadTermsAndConditions( [FromForm] DocumentCreationRequest request ) {
            Institute institute = null;

            return RulesHelper
                .IfInstituteIsValid( GetInstituteId(), out institute )
                .Then( async () => {
                    try {
                        var documentCreated = await _documentsStorageService.AddDocument(
                            institute.Id, DocumentType.TermsAndConditions, request );
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
        /// Upload privacy policy file for an Institute
        /// </summary>
        /// <param name="request">Body of the request</param>
        /// <returns>Institute information</returns>
        [HttpPost]
        [Route( "privacy" )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( DocumentModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.BadRequest, Type = typeof( ErrorModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.Conflict, Type = typeof( ErrorModel ) )]
        public IActionResult UploadPrivacyPolicy( [FromForm] DocumentCreationRequest request ) {
            Institute institite = null;

            return RulesHelper
                .IfInstituteIsValid( GetInstituteId(), out institite )
                .Then( async () => {
                    try {
                        var documentCreated = await _documentsStorageService.AddDocument(
                            institite.Id, DocumentType.Privacy, request );
                        SaveChanges();

                        return Ok( documentCreated );
                    }
                    catch ( Exception e ) {
                        return BadRequest( e.Message );
                    }
                } )
                .ReturnResult();
        }
    }
}
