using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proact.Services.Models;
using Proact.Services.QueriesServices;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Net;

namespace Proact.Services.Controllers {
    [ApiController]
    [Authorize]
    [Route( ProactRouteConfiguration.DefaultRoute )]
    public class PushNotificationsController : ProactBaseController {
        private readonly IUserNotificationsSettingsEditorService _userNotificationsSettingsEditorService;

        public PushNotificationsController( 
            IChangesTrackingService changesTrackingService, 
            ConsistencyRulesHelper consistencyRulesHelper,
            IUserNotificationsSettingsEditorService userNotificationsSettingsEditorService,
            IDeviceQueriesService deviceQueriesService ) 
            : base( changesTrackingService, consistencyRulesHelper ) {
            _userNotificationsSettingsEditorService = userNotificationsSettingsEditorService;
        }

        /// <summary>
        /// Register device for push notification
        /// </summary>
        /// <param name="request">Body of request</param>
        /// <returns>Ok</returns>
        [HttpPost]
        [Route( "register" )]
        [SwaggerResponse( (int)HttpStatusCode.OK )]
        public IActionResult RegisterDevice( DeviceRegistrationRequest request ) {
            var currentUser = GetCurrentUser();

            _userNotificationsSettingsEditorService.AddDevice( currentUser.Id, request.PlayerId );

            SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Remove device from push notification
        /// </summary>
        /// <returns>Ok</returns>
        [HttpDelete]
        [Route( "remove/{playerId:guid}" )]
        [SwaggerResponse( (int)HttpStatusCode.OK )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound )]
        public IActionResult RemoveDevice( Guid playerId ) {
            var currentUser = GetCurrentUser();

            var result = _userNotificationsSettingsEditorService.RemoveDevice( currentUser.Id, playerId );

            SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Active or deactive Push Notification for an user
        /// </summary>
        /// <param name="request">Body of request</param>
        /// <returns>Ok</returns>
        [HttpPut]
        [Route( "active" )]
        [SwaggerResponse( (int)HttpStatusCode.OK )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound )]
        public IActionResult ActiveNotification( ActiveNotificationRequest request ) {
            var currentUser = GetCurrentUser();

            _userNotificationsSettingsEditorService.SetActive( currentUser.Id, request.Active );

            SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Reset notification settings
        /// </summary>
        /// <returns>Ok</returns>
        [HttpPut]
        [Route( "reset" )]
        [SwaggerResponse( (int)HttpStatusCode.OK )]
        public IActionResult SetNotificationDayAsAllDay() {
            var currentUser = GetCurrentUser();

            _userNotificationsSettingsEditorService.ResetConfiguration( currentUser.Id );

            SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Set notifications Range
        /// </summary>
        /// <param name="request">Body of request</param>
        /// <returns>Ok</returns>
        [HttpPut]
        [Route( "SetRange" )]
        [SwaggerResponse( (int)HttpStatusCode.OK )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound )]
        public IActionResult SetNotificationsRange( SetNotificationRangeRequest request ) {
            var currentUser = GetCurrentUser();

            var startAt = new TimeSpan( request.StartAtUtc.Hour, request.StartAtUtc.Minute, 0 );
            var stopAt = new TimeSpan( request.StopAtUtc.Hour, request.StopAtUtc.Minute, 0 );

            _userNotificationsSettingsEditorService.SetRange( currentUser.Id, startAt, stopAt );

            SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Get Notifications Settings
        /// </summary>
        /// <returns>Ok</returns>
        [HttpGet]
        [Route( "Settings" )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( NotificationSettingsModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound )]
        public IActionResult GetNotificationsSettings() {
            var currentUser = GetCurrentUser();

            return Ok( _userNotificationsSettingsEditorService
                .GetNotificationSettingsByUserId( currentUser.Id ) );
        }
    }
}
