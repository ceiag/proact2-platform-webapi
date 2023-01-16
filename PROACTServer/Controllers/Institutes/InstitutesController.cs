using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proact.Services.AuthorizationPolicies;
using Proact.Services.Entities;
using Proact.Services.EntitiesMapper;
using Proact.Services.Models;
using Proact.Services.QueriesServices;
using Proact.Services.Services;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Net;

namespace Proact.Services.Controllers.Institutes {
    [ApiController]
    [Route( ProactRouteConfiguration.DefaultRoute )]
    public class InstitutesController : ProactBaseController {
        private readonly IInstitutesQueriesService _institutesQueriesService;
        private readonly IUsersCreatorQueriesService _usersCreatorQueriesService;
        private readonly IGroupService _userGroupService;

        public InstitutesController(
            IInstitutesQueriesService institutesQueriesService,
            IUsersCreatorQueriesService usersCreatorQueriesService,
            IGroupService userGroupService,
            IChangesTrackingService changesTrackingService,
            ConsistencyRulesHelper consistencyRulesHelper ) 
            : base( changesTrackingService, consistencyRulesHelper ) {
            _institutesQueriesService = institutesQueriesService;
            _usersCreatorQueriesService = usersCreatorQueriesService;
            _userGroupService = userGroupService;
        }

        /// <summary>
        /// Create a new Institute in Open State
        /// </summary>
        /// <param name="instituteCreateRequest">Institute informations</param>
        /// <returns>Institute information</returns>
        [HttpPost]
        [Authorize( Policy = Policies.InstitutesReadWrite )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( InstituteModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.BadRequest, Type = typeof( ErrorModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.Conflict, Type = typeof( ErrorModel ) )]
        public IActionResult Create( InstituteCreationRequest instituteCreateRequest ) {
            return RulesHelper
                .IfInstituteNameIsAvailable( instituteCreateRequest.Name )
                .Then( () => {
                    var institute = _institutesQueriesService.Create( instituteCreateRequest );
                    SaveChanges();

                    return Ok( InstituteEntityMapper.Map( institute ) );
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Update an Institute in Open State
        /// </summary>
        /// <param name="instituteId">Institute Identifier</param>
        /// <param name="instituteUpdateRequest">Institute informations</param>
        /// <returns>Institute information</returns>
        [HttpPut]
        [Route("{instituteId:guid}")]
        [Authorize( Policy = Policies.InstitutesReadWrite )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( InstituteModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.BadRequest, Type = typeof( ErrorModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.Conflict, Type = typeof( ErrorModel ) )]
        public IActionResult Update( Guid instituteId, InstituteUpdateRequest instituteUpdateRequest ) {
            return RulesHelper
                .IfInstituteNameIsAvailable( instituteUpdateRequest.Name )
                .Then( () => {
                    var institute = _institutesQueriesService.Update( instituteId, instituteUpdateRequest );
                    SaveChanges();

                    return Ok( InstituteEntityMapper.Map( institute ) );
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Close an Institute in Open State
        /// </summary>
        /// <param name="instituteId">Institute Identifier</param>
        [HttpPut]
        [Route( "{instituteId:guid}/close" )]
        [Authorize( Policy = Policies.InstitutesReadWrite )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( InstituteModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.BadRequest, Type = typeof( ErrorModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.Conflict, Type = typeof( ErrorModel ) )]
        public IActionResult Close( Guid instituteId ) {
            return RulesHelper
                .Then( () => {
                    _institutesQueriesService.Close( instituteId );
                    SaveChanges();

                    return Ok();
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Open an Institute in Close State
        /// </summary>
        /// <param name="instituteId">Institute Identifier</param>
        [HttpPut]
        [Route( "{instituteId:guid}/open" )]
        [Authorize( Policy = Policies.InstitutesReadWrite )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( InstituteModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.BadRequest, Type = typeof( ErrorModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.Conflict, Type = typeof( ErrorModel ) )]
        public IActionResult Open( Guid instituteId ) {
            return RulesHelper
                .Then( () => {
                    _institutesQueriesService.Open( instituteId );
                    SaveChanges();

                    return Ok();
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Create Administrator for an Institute
        /// </summary>
        /// <param name="instituteId">Institute identifier</param>
        /// <param name="adminCreationRequest">Admin assignation informations</param>
        [HttpPost]
        [Route( "{instituteId:guid}/admin" )]
        [Authorize( Policy = Policies.InstitutesReadWrite )]
        [SwaggerResponse( (int)HttpStatusCode.OK )]
        [SwaggerResponse( (int)HttpStatusCode.BadRequest, Type = typeof( ErrorModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.Conflict, Type = typeof( ErrorModel ) )]
        public IActionResult CreateAdmin( 
            Guid instituteId, InstituteAdminCreationRequest adminCreationRequest ) {
            Institute institute = null;

            return RulesHelper
                .IfInstituteIsValid( instituteId, out institute )
                .Then( async () => {
                    try {
                        var adminUser = await _usersCreatorQueriesService.CreateBasicUser( 
                            instituteId, adminCreationRequest.User.FirstName, 
                            adminCreationRequest.User.Lastname, adminCreationRequest.User.Email );

                        SaveChanges();

                        var groupId = await _userGroupService.GetGroupIdByName( Roles.InstituteAdmin );
                        await _userGroupService.AddMember( groupId, adminUser.AccountId );

                        _institutesQueriesService.AssignAdmin( 
                            UserEntityMapper.Map( adminUser ), institute.Id );
                        SaveChanges();

                        return Ok();
                    }
                    catch( Exception e ) {
                        return BadRequest( e.Message );
                    }
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Returns details about Institute
        /// </summary>
        /// <param name="instituteId">Institute identifier</param>
        /// <returns>Institute information</returns>
        [HttpGet]
        [Route( "{instituteId:guid}" )]
        [Authorize( Policy = Policies.InstitutesRead )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( InstituteModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.BadRequest, Type = typeof( ErrorModel ) )]
        public IActionResult Get( Guid instituteId ) {
            Institute institute = null;

            return RulesHelper
                .IfInstituteIsValid( instituteId, out institute )
                .Then( () => {
                    return Ok( InstituteEntityMapper.Map( institute ) );
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Returns all Institutes
        /// </summary>
        /// <returns>List of Institutes present into the system</returns>
        [HttpGet]
        [Authorize( Policy = Policies.InstitutesRead )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( List<InstituteModel> ) )]
        [SwaggerResponse( (int)HttpStatusCode.BadRequest, Type = typeof( ErrorModel ) )]
        public IActionResult GetAll() {
            return RulesHelper
                .Then( () => {
                    return Ok( InstituteEntityMapper.Map( _institutesQueriesService.GetAll() ) );
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Return Institute where I'm Admin
        /// </summary>
        /// <returns>Institute information</returns>
        [HttpGet]
        [Route( "admin/me" )]
        [Authorize( Policy = Policies.InstitutesMyReadWrite )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( InstituteModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.BadRequest, Type = typeof( ErrorModel ) )]
        public IActionResult GetWhereImAdmin() {
            return RulesHelper
                .Then( () => {
                    try {
                        var institute = _institutesQueriesService.GetWhereImAdmin( GetCurrentUser().Id );
                        return Ok( InstituteEntityMapper.Map( institute ) );
                    }
                    catch {
                        return new NotFoundObjectResult( "You are not admin for any Institute" );
                    }
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Return Institute where I'm in
        /// </summary>
        /// <returns>Institute information</returns>
        [HttpGet]
        [Route( "me" )]
        [Authorize( Policy = Policies.UsersMeRead )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( InstituteModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.BadRequest, Type = typeof( ErrorModel ) )]
        public IActionResult GetInstituteWhereImIn() {
            return RulesHelper
                .Then( () => {
                    try {
                        var institute = GetCurrentInstitute();
                        institute.Admins.Clear();
                        return Ok( InstituteEntityMapper.Map( institute ) );
                    }
                    catch {
                        return new NotFoundObjectResult( "You are not in any Institute" );
                    }
                } )
                .ReturnResult();
        }
    }
}
