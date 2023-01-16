using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Proact.Services.AuthorizationPolicies;
using Proact.Services.Models;
using Proact.Services.QueriesServices;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Proact.Services.Controllers {

    [ApiController]
    [Route( ProactRouteConfiguration.DefaultRoute )]
    public class UserAvatarController : ProactBaseController {
        private IAvatarProviderService _avatarProviderService;
        private IUserQueriesService _userQueriesService;

        public UserAvatarController( 
            IUserQueriesService usersQueriesService, IChangesTrackingService changesTrackingService, 
            ConsistencyRulesHelper consistencyRulesHelper, IAvatarProviderService avatarProviderService ) 
            : base( changesTrackingService, consistencyRulesHelper ) {
            _avatarProviderService = avatarProviderService;
            _userQueriesService = usersQueriesService;
        }

        private void SaveAvatarUrlOnDatabase( string avatarUrl ) {
            _userQueriesService.SetAvatarUrl( GetCurrentUser().Id, avatarUrl );

            SaveChanges();
        }

        /// <summary>
        /// Upload user avatar
        /// </summary>
        /// <param name="avatarFile">Media File</param>
        [HttpPost]
        [Authorize( Policy = Policies.UsersMeRead )]
        [Route( "upload" )]
        [SwaggerResponse( (int)HttpStatusCode.OK )]
        [SwaggerResponse( (int)HttpStatusCode.BadRequest, Type = typeof( ErrorModel ) )]
        public async Task<IActionResult> UploadAvatar( IFormFile avatarFile ) {
            try {
                if ( avatarFile == null ) {
                    return BadRequest( "avatarFile can not be null" );
                }

                var imageUploadResult = await _avatarProviderService
                    .UploadAvatar( GetCurrentUser().Id, avatarFile );

                SaveAvatarUrlOnDatabase( imageUploadResult.ContentUrl );

                return Ok();
            }
            catch ( Exception e ) {
                return StatusCode( 500, e.Message + " " + e.InnerException );
            }
        }

        /// <summary>
        /// Reset User Avatar
        /// </summary>
        [HttpPost]
        [Authorize( Policy = Policies.UsersMeRead )]
        [Route( "reset" )]
        [SwaggerResponse( (int)HttpStatusCode.OK )]
        [SwaggerResponse( (int)HttpStatusCode.BadRequest, Type = typeof( ErrorModel ) )]
        public IActionResult ResetAvatar() {
            var user = GetCurrentUser();
            string userDefaultAvatarUrl = AvatarConfiguration.PatientAvatarDefaultUrl;

            if ( HasRoleOf( Roles.MedicalProfessional )
                || HasRoleOf( Roles.MedicalTeamAdmin ) ) {
                userDefaultAvatarUrl = AvatarConfiguration.MedicAvatarDefaultUrl;
            }

            SaveAvatarUrlOnDatabase( userDefaultAvatarUrl );

            return Ok();
        }
    }
}
