using Microsoft.AspNetCore.Mvc;
using Proact.Services.Entities;
using Proact.Services.Models;
using Proact.Services.Models.Stats;
using Proact.Services.QueriesServices;
using Proact.Services.QueriesServices.Stats;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Net;

namespace Proact.Services.Controllers.Stats {
    [ApiController]
    [Route( ProactRouteConfiguration.DefaultRoute )]
    public class StatsController : ProactBaseController {
        private readonly IMessagesStatsProviderService _messagesStatsProviderService;

        public StatsController(
            IMessagesStatsProviderService messagesStatsProviderService,
            IChangesTrackingService changesTrackingService, 
            ConsistencyRulesHelper consistencyRulesHelper ) 
            : base( changesTrackingService, consistencyRulesHelper ) {
            _messagesStatsProviderService = messagesStatsProviderService;
        }

        /// <summary>
        /// Get message stats present into PROACT
        /// </summary>
        [HttpGet]
        [Route( "messages" )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( MessagesStatsModel ) )]
        public IActionResult GetMessagesStats() {
            return Ok( _messagesStatsProviderService.GetMessagesStats() );
        }

        /// <summary>
        /// Get message stats present into an institute
        /// </summary>
        [HttpGet]
        [Route( "messages/institute/{instituteId:guid}" )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( MessagesStatsModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        public IActionResult GetMessagesStatsForInstitute( Guid instituteId ) {
            Institute institute = null;

            return RulesHelper
                .IfInstituteIsValid( instituteId, out institute )
                .Then( () => {
                    return Ok( _messagesStatsProviderService.GetMessagesStatsForInstitute( instituteId ) );
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Get message stats present into a project/study
        /// </summary>
        [HttpGet]
        [Route( "messages/project/{projectId:guid}" )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( MessagesStatsModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        public IActionResult GetMessagesStatsForProject( Guid projectId ) {
            Project project = null;

            return RulesHelper
                .IfProjectIsValid( projectId, out project )
                .Then( () => {
                    return Ok( _messagesStatsProviderService.GetMessagesStatsForProject( projectId ) );
                } )
                .ReturnResult();
        }
    }
}
