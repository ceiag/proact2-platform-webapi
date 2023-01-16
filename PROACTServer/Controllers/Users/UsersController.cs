using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Proact.Services.Entities;
using Proact.Services.Models;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Authorization;
using Proact.Services.AuthorizationPolicies;
using Proact.Services.Services;
using Proact.Services.QueriesServices;
using Proact.Services.Services.EmailSender;

namespace Proact.Services.Controllers.Users {
    [ApiController]
    [Route( ProactRouteConfiguration.DefaultRoute )]
    /// <summary>
    /// Create a new user
    /// </summary>
    public class UsersController : ProactBaseController {
        private readonly IUserIdentityService _userIdentityService;
        private readonly IGroupService _groupService;
        private readonly IUserQueriesService _usersQueriesService;
        private readonly IUsersCreatorQueriesService _usersCreatorQueriesService;
        private readonly IEmailSenderService _emailSenderService;

        public UsersController( 
            IUserQueriesService usersQueriesService, IChangesTrackingService changesTrackingService,
            ConsistencyRulesHelper consistencyRulesHelper,
            IUserIdentityService userIdentityService, IGroupService groupService,
            IUsersCreatorQueriesService usersCreatorQueriesService,
            IEmailSenderService emailSenderService ) 
            : base( changesTrackingService, consistencyRulesHelper ) {
            _userIdentityService = userIdentityService;
            _groupService = groupService;
            _usersQueriesService = usersQueriesService;
            _usersCreatorQueriesService = usersCreatorQueriesService;
            _emailSenderService = emailSenderService;
        }

        /// <summary>
        /// Create a new user
        /// </summary>
        /// <param name="request">User information</param>
        /// <returns>User information</returns>
        [HttpPost]
        [Authorize( Policy = Policies.MedicalTeamReadWrite )]
        [SwaggerResponse( ( int )HttpStatusCode.Created, Type = typeof( UserModel ) )]
        [SwaggerResponse( ( int )HttpStatusCode.BadRequest, Type = typeof( ErrorModel ) )]
        [SwaggerResponse( ( int )HttpStatusCode.Conflict, Type = typeof( ErrorModel ) )]
        public IActionResult CreateUser( UserCreateRequest request ) {
            return RulesHelper
                .Then( async () => {
                    try {
                        var creationResult = await _usersCreatorQueriesService.CreateBasicUser( 
                            GetInstituteId(), request.FirstName, request.Lastname, request.Email );

                        SaveChanges();

                        return Ok( creationResult );
                    }
                    catch ( Exception ex ) {
                        return BadRequest( ex.Message );
                    }

                } )
                .ReturnResult();
        }

        private void AssignDefaultAvatarToUser( User user, string role ) {
            if ( role == Roles.MedicalProfessional || role == Roles.MedicalTeamAdmin ) {
                user.AvatarUrl = AvatarConfiguration.MedicAvatarDefaultUrl;
            }
            else {
                user.AvatarUrl = AvatarConfiguration.PatientAvatarDefaultUrl;
            }

            SaveChanges();
        }

        /// <summary>
        /// Assign a role to a user
        /// </summary>
        /// <param name="assignData">UserId and Role name</param>
        [HttpPost]
        [Authorize( Policy = Policies.UsersReadWrite )]
        [Route( "AssignRole" )]
        [SwaggerResponse( ( int )HttpStatusCode.OK )]
        [SwaggerResponse( ( int )HttpStatusCode.BadRequest, Type = typeof( ErrorModel ) )]
        public IActionResult AssignRole( AssignRoleToUserRequest assignData ) {
            User user = null;

            return RulesHelper
                .IfUserIsValid( assignData.UserId, out user )
                .Then( async () => {
                    var groupId = await _groupService.GetGroupIdByName( assignData.Role );

                    if ( groupId == null ) {
                        return NotFound( string.Format( "Role with id {0} not found!", assignData.Role ) );
                    }

                    try {
                        await _groupService.AddMember( groupId, user.AccountId );
                    }
                    catch ( Exception ex ) {
                        return BadRequest( ex.Message );
                    }

                    AssignDefaultAvatarToUser( user, assignData.Role );

                    return Ok();
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Provides a list of users
        /// </summary>
        /// <returns>List of users</returns>
        [HttpGet]
        [Authorize( Policy = Policies.UsersReadWrite )]
        [SwaggerResponse( ( int )HttpStatusCode.OK, Type = typeof( UserModel[] ) )]
        public IActionResult GetUsers() {
            return Ok( UserEntityMapper.Map( _usersQueriesService.GetsAll() ) );
        }

        /// <summary>
        /// Gets user info info by its identifier
        /// </summary>
        /// <param name="userId">The identifier of the user</param>
        /// <returns>User information</returns>
        [HttpGet]
        [Authorize( Policy = Policies.UsersReadWrite )]
        [Route( "{userId:guid}" )]
        [SwaggerResponse( ( int )HttpStatusCode.OK, Type = typeof( UserModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        public IActionResult GetUser( Guid userId ) {
            User user = null;

            return RulesHelper
                .IfUserIsValid( userId, out user )
                .Then( async () => {
                    List<string> roles = await _groupService
                        .GetGroupsAssociatedWithTheUser( user.AccountId );

                    var userModel = UserEntityMapper.Map( user );
                    userModel.Roles = roles;

                    return Ok( userModel );
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Gets current user data info
        /// </summary>
        /// <returns>User information</returns>
        [HttpGet]
        [Authorize( Policy = Policies.UsersMeRead )]
        [Route( "me" )]
        [SwaggerResponse( ( int )HttpStatusCode.OK, Type = typeof( UserModel ) )]
        [SwaggerResponse( ( int )HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        public IActionResult GetUser() {
            User user = null;

            return RulesHelper
                .IfUserAccountIsValid( GetCurrentUser().AccountId, out user )
                .Then( async () => {
                    List<string> roles = await _groupService
                        .GetGroupsAssociatedWithTheUser( user.AccountId.ToString() );

                    var userModel = UserEntityMapper.Map( user );
                    userModel.Roles = roles;

                    return Ok( userModel );
                } )
                .ReturnResult();
        }

        /// <summary>
        ///  Delete a user from proact db and remove Azure AD user
        /// </summary>
        /// <param name="userId">User identifier</param>
        [HttpDelete]
        [Authorize( Policy = Policies.UsersReadWrite )]
        [Route( "{userId:guid}" )]
        [SwaggerResponse( (int)HttpStatusCode.OK )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        public IActionResult DeleteUser( Guid userId ) {
            User user = null;

            return RulesHelper
                .IfUserIsValid( userId, out user )
                .Then( async () => {
                    var azureADUserDeleted = await _userIdentityService.Delete( user.AccountId );

                    if ( azureADUserDeleted ) {
                        _usersQueriesService.Delete( userId );

                        SaveChanges();
                    }

                    return Ok();
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Active a user and enable azure AD signin 
        /// </summary>
        /// <param name="userId">User identifier</param>
        [HttpPut]
        [Authorize( Policy = Policies.UsersReadWrite )]
        [Route( "activate/{userId:guid}" )]
        [SwaggerResponse( (int)HttpStatusCode.OK )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        public IActionResult ActivateUser( Guid userId ) {
            User user = null;

            return RulesHelper
                .IfUserIsValid( userId, out user )
                .Then( async () => {
                    bool azureADResult = await _userIdentityService.Activate( user.AccountId );

                    if ( azureADResult ) {
                        user.State = UserSubscriptionState.Active;
                        
                        SaveChanges();
                    }

                    return Ok();
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Suspend a patient from proact db and disable Azure AD signin
        /// </summary>
        /// <param name="userId">User identifier</param>
        [HttpPut]
        [Authorize( Policy = Policies.UsersReadWrite )]
        [Route( "suspend/{userId:guid}" )]
        [SwaggerResponse( (int)HttpStatusCode.OK )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        public IActionResult SuspendPatient( Guid userId ) {
            User user = null;
            Patient patient = null;

            return RulesHelper
                .IfUserIsValid( userId, out user )
                .IfPatientIsValid( userId, out patient )
                .Then( async () => {
                    user.State = UserSubscriptionState.Suspended;
                    SaveChanges();

                    await _emailSenderService.SendSuspendedEmailTo( patient.MedicalTeam.ProjectId, patient );
                    return Ok();
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Disable a patient from proact db and disable Azure AD signin
        /// </summary>
        /// <param name="userId">User identifier</param>
        [HttpPut]
        [Authorize( Policy = Policies.UsersReadWrite )]
        [Route( "deactivate/{userId:guid}" )]
        [SwaggerResponse( (int)HttpStatusCode.OK )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        public IActionResult DeactivatePatient( Guid userId ) {
            User user = null;
            Patient patient = null;

            return RulesHelper
                .IfUserIsValid( userId, out user )
                .IfPatientIsValid( userId, out patient )
                .Then( async () => {
                    bool azureADResult = await _userIdentityService.Suspend( user.AccountId );

                    if ( azureADResult ) {
                        user.State = UserSubscriptionState.Deactivated;
                        SaveChanges();
                    }

                    await _emailSenderService.SendDeactivatedEmailTo( patient.MedicalTeam.ProjectId, patient );
                    return Ok();
                } )
                .ReturnResult();
        }
    }
}
