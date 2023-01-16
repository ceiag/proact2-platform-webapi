using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Net;
using Proact.Services.Models;
using Microsoft.Extensions.Localization;
using Proact.Services.QueriesServices;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace Proact.Services.Controllers.System {
    [ApiController]
    [Route( ProactRouteConfiguration.DefaultRoute )]
    public class SystemConfigurationController : ProactBaseController {
        private readonly IProactSystemInitializerService _systemInitializer;
        private readonly IStringLocalizer _localizer;
        private readonly IConfiguration _configuration;

        private readonly string _systemAlreadyInitializedErrorMessage = "System already initialized.";

        public SystemConfigurationController(
            IChangesTrackingService changesTrackingService, 
            ConsistencyRulesHelper consistencyRulesHelper,
            IStringLocalizer<Resource> localizer,
            IProactSystemInitializerService proactSystemInitializerService,
            IConfiguration configuration ) 
            : base( changesTrackingService, consistencyRulesHelper ) {
            _systemInitializer = proactSystemInitializerService;
            _localizer = localizer;
            _configuration = configuration;
        }

        /// <summary>
        /// Initialize system and create a System Admin User
        /// </summary>
        /// <param name="request">System Admin information</param>
        /// <returns>User information</returns>
        [HttpPost]
        [SwaggerResponse( ( int )HttpStatusCode.OK )]
        [SwaggerResponse( ( int )HttpStatusCode.Conflict )]
        [SwaggerResponse( ( int )HttpStatusCode.BadRequest )]
        public async Task<IActionResult> InitializeSystem( SystemInitializationRequest request ) {
            try {
                if ( _systemInitializer.SystemAlreadyInitialized() ) {
                    return Conflict( _systemAlreadyInitializedErrorMessage );
                }

                await _systemInitializer.Initialize( request );
            }
            catch ( Exception ex ) {
                return BadRequest( ex.Message );
            }

            return Ok();
        }

        /// <summary>
        /// Culture request check
        /// </summary>
        [HttpGet]
        [Route( "CheckLanguage" )]
        public string CheckRequestLanguage() {
            return _localizer["CurrentLanguage"];
        }

        /// <summary>
        /// Check Server Info
        /// </summary>
        [HttpGet]
        [Route( "Environment" )]
        public string GetServerInfo() {
            var codeVersion = Assembly.GetEntryAssembly()
                .GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
            return $"Environment: {_configuration["Environment"]} Version: {codeVersion}";
        }
    }
}
