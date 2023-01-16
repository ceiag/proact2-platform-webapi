using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proact.Services.AuthorizationPolicies;
using Proact.Services.Entities;
using Proact.Services.EntitiesMapper;
using Proact.Services.Models;
using Proact.Services.QueriesServices;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Linq;
using System.Net;

namespace Proact.Services.Controllers {

    [ApiController]
    [Route( ProactRouteConfiguration.DefaultRoute )]
    [Authorize( Policy = Policies.MedicalTeamReadWrite )]
    public class NursesController : ProactBaseController {
        private readonly INurseQueriesService _nurseQueriesService;
        private readonly IMedicalTeamQueriesService _medicalTeamQueriesService;
        private readonly IUsersCreatorQueriesService _usersCreatorQueriesService;

        public NursesController( 
            IChangesTrackingService changesTrackingService,
            INurseQueriesService nurseQueriesService,
            IMedicalTeamQueriesService medicalTeamQueriesService,
            IUsersCreatorQueriesService usersCreatorQueriesService,
            ConsistencyRulesHelper consistencyRulesHelper ) 
            : base( changesTrackingService, consistencyRulesHelper ) {
            _nurseQueriesService = nurseQueriesService;
            _medicalTeamQueriesService = medicalTeamQueriesService;
            _usersCreatorQueriesService = usersCreatorQueriesService;
        }

        /// <summary>
        /// Create a Nurse
        /// </summary>
        /// <param name="request">Request model to create Nurse</param>
        /// <returns>Nurse informations</returns>
        [HttpPost]
        [Route( "create" )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( NurseModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        public IActionResult CreateNurse( CreateNurseRequest request ) {
            return RulesHelper
                .Then( async () => {
                    try {
                        var creationResult = await _usersCreatorQueriesService
                            .CreateNurse( GetInstituteId(), request );

                        SaveChanges();

                        return Ok( creationResult );
                    }
                    catch ( Exception ex ) {
                        return BadRequest( ex.Message );
                    }
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Assign nurse to a Medical Team
        /// </summary>
        /// <param name="request">Request Body for Medical Team Assign</param>
        /// <param name="medicalTeamId">Id of medical team</param>
        /// <returns>Medic informations</returns>
        [HttpPost]
        [Route( "{medicalTeamId:guid}/assign" )]
        [SwaggerResponse( (int)HttpStatusCode.OK )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        public IActionResult AssignNurseToMedicalTeam(
            Guid medicalTeamId, AssignNurseToMedicalTeamRequest request ) {
            User user = null;
            Nurse nurse = null;
            MedicalTeam medicalTeam = null;

            return RulesHelper
                .IfUserIsValid( request.UserId, out user )
                .IfUserIsInMyInstitute( GetInstituteId(), user )
                .IfMedicalTeamIsValid( medicalTeamId, out medicalTeam )
                .IfUserIsInMyInstitute( GetInstituteId(), user )
                .IfMedicalTeamIsOpen( medicalTeamId )
                .IfNurseIsValid( request.UserId, out nurse )
                .IfNurseIsNotIntoTheMedicalTeam( request.UserId, medicalTeamId )
                .IfIHavePermissionsToAssignUserToThisMedicalTeam( 
                    GetCurrentUser().Id, medicalTeamId, GetCurrentUserRoles() )
                .Then( () => {
                    _nurseQueriesService.AddToMedicalTeam( user.Id, medicalTeamId );

                    SaveChanges();

                    return Ok();
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Get list of nurses assigned to medical team
        /// </summary>
        /// <param name="medicalTeamId">Medical team identifier</param>
        /// <returns>The list of Nurses</returns>
        [HttpGet]
        [Route( "{medicalTeamId:guid}" )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( NurseModel[] ) )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        public IActionResult GetNurses( Guid medicalTeamId ) {
            MedicalTeam medicalTeam = null;

            return RulesHelper
                .IfMedicalTeamIsValid( medicalTeamId, out medicalTeam )
                .IfMedicalTeamIsInMyInstitute( GetCurrentInstitute().Id, medicalTeam )
                .Then( () => {
                    return Ok( NurseEntityMapper.Map(
                        _medicalTeamQueriesService.Get( medicalTeamId ).Nurses.ToList() ) );
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Return the list of Nurses into the System
        /// </summary>
        /// <returns>Nurse information</returns>
        [HttpGet]
        [Route( "all" )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( NurseModel[] ) )]
        public IActionResult GetNurseAll() {
            return RulesHelper
                .Then( () => {
                    return Ok( NurseEntityMapper.Map( 
                        _nurseQueriesService.GetAll( GetInstituteId() ) ) );
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Delete nurse from medical team
        /// </summary>
        /// <param name="medicalTeamId">Medical team identifier</param>
        /// <param name="userId">User identifier of a medic</param>
        [HttpDelete]
        [Route( "{medicalTeamId:guid}/{userId:guid}" )]
        [SwaggerResponse( (int)HttpStatusCode.OK )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        public IActionResult RemoveNurseFromMedicalTeam( Guid medicalTeamId, Guid userId ) {
            MedicalTeam medicalTeam = null;
            Nurse nurse = null;

            return RulesHelper
                .IfMedicalTeamIsValid( medicalTeamId, out medicalTeam )
                .IfMedicalTeamIsOpen( medicalTeamId )
                .IfNurseIsValid( userId, out nurse )
                .IfUserIsInMyInstitute( GetInstituteId(), nurse.User )
                .Then( () => {
                    _nurseQueriesService.RemoveFromMedicalTeam( userId, medicalTeam );
                    
                    SaveChanges();

                    return Ok();
                } )
                .ReturnResult();
        }
    }
}
