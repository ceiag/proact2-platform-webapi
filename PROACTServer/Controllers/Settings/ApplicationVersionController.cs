using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proact.Services.AuthorizationPolicies;
using Proact.Services.Models;
using Proact.Services.QueriesServices;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace Proact.Services.Controllers.Settings {
    [ApiController]
    [Route( ProactRouteConfiguration.DefaultRoute )]
    public class ApplicationVersionController : ProactBaseController {
        private readonly IMobileAppsInfoQueriesService _mobileAppsInfoQueriesService;

        public ApplicationVersionController( 
            IChangesTrackingService changesTrackingService,
            ConsistencyRulesHelper consistencyRulesHelper,
            IMobileAppsInfoQueriesService mobileAppsInfoQueriesService )
            : base( changesTrackingService, consistencyRulesHelper ) {
            _mobileAppsInfoQueriesService = mobileAppsInfoQueriesService;
        }

        /// <summary>
        /// Set minimum version for iOS and Android App 
        /// </summary>
        /// <param name="request">body of request</param>
        [HttpPost]
        [Authorize( Policy = Policies.AppSettingsWrite )]
        [SwaggerResponse( (int)HttpStatusCode.OK )]
        public IActionResult CreateAppSettings( MobileAppsInfoCreationRequest request ) {
            return RulesHelper
                .Then( () => {
                    _mobileAppsInfoQueriesService.Set( request );
                    return Ok();
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Returns informations about the minimum version of mobile applications requested
        /// </summary>
        [HttpGet]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( MobileAppsInfoModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        public IActionResult GetAppSettings() {
            return RulesHelper
                .Then( () => {
                    return Ok( _mobileAppsInfoQueriesService.Get() );
                } )
                .ReturnResult();
        }
    }
}
