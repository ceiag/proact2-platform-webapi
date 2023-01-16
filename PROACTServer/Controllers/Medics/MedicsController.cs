using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proact.Services.AuthorizationPolicies;
using Proact.Services.Entities;
using Proact.Services.Models;
using Proact.Services.QueriesServices;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Linq;
using System.Net;

namespace Proact.Services.Controllers {

    [ApiController]
    [Route( ProactRouteConfiguration.DefaultRoute )]
    public class MedicsController : ProactBaseController {
        private readonly IMedicalTeamQueriesService _medicalTeamQueriesService;
        private readonly IMedicQueriesService _medicQueriesService;
        private readonly IUsersCreatorQueriesService _usersCreatorQueriesService;
        
        public MedicsController(
            IChangesTrackingService changesTrackingService, IMedicalTeamQueriesService medicalTeamQueriesService,
            IMedicQueriesService medicQueriesService, IUsersCreatorQueriesService usersCreatorQueriesService,
            ConsistencyRulesHelper consistencyRulesHelper )
            : base( changesTrackingService, consistencyRulesHelper ) {
            _medicalTeamQueriesService = medicalTeamQueriesService;
            _medicQueriesService = medicQueriesService;
            _usersCreatorQueriesService = usersCreatorQueriesService;
        }

        /// <summary>
        /// Get list of medics assigned to medical team
        /// </summary>
        /// <param name="medicalTeamId">Medical team identifier</param>
        /// <returns>The list of medics</returns>
        [HttpGet]
        [Route( "{medicalTeamId:guid}" )]
        [Authorize( Policy = Policies.MedicalTeamReadWrite )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( UserModel[] ) )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        public IActionResult GetMedics( Guid medicalTeamId ) {
            MedicalTeam medicalTeam = null;

            return RulesHelper
                .IfMedicalTeamIsValid( medicalTeamId, out medicalTeam )
                .IfMedicalTeamIsInMyInstitute( GetCurrentInstitute().Id, medicalTeam )
                .Then( () => {
                    return Ok( MedicEntityMapper.Map(
                        _medicalTeamQueriesService.Get( medicalTeamId ).Medics.ToList() ) );
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Get medic information
        /// </summary>
        /// <param name="medicalTeamId">Medical team identifier</param>
        /// <param name="userId">User identifier of a medic</param>
        /// <returns>Medic information</returns>
        [HttpGet]
        [Route( "{medicalTeamId:guid}/{userId:guid}" )]
        [Authorize( Policy = Policies.MedicalTeamReadWrite )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( MedicModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        public IActionResult GetMedic( Guid medicalTeamId, Guid userId ) {
            MedicalTeam medicalTeam = null;
            Medic medic = null;

            return RulesHelper
                .IfMedicalTeamIsValid( medicalTeamId, out medicalTeam )
                .IfMedicalTeamIsInMyInstitute( GetCurrentInstitute().Id, medicalTeam )
                .IfMedicIsValid( userId, out medic )
                .Then( () => { 
                    return Ok( MedicEntityMapper.Map( _medicQueriesService.Get( userId ) ) );
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Return the list of Medics into the System
        /// </summary>
        /// <returns>Medic information</returns>
        [HttpGet]
        [Route( "all" )]
        [Authorize( Policy = Policies.MedicalTeamReadWrite )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( MedicModel[] ) )]
        public IActionResult GetMedicAll() {
            return RulesHelper
                .Then( () => {
                    var currentInstitute = GetCurrentInstitute();
                    return Ok( MedicEntityMapper.Map( 
                        _medicQueriesService.GetsAll( currentInstitute.Id ).ToList() ) );
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Create a Medic
        /// </summary>
        /// <param name="createMedicRequest">Request model to create Medic</param>
        /// <returns>Medic informations</returns>
        [HttpPost]
        [Route( "create" )]
        [Authorize( Policy = Policies.MedicalTeamReadWrite )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( MedicModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        public IActionResult CreateMedic( CreateMedicRequest createMedicRequest ) {
            return RulesHelper
                .Then( async () => {
                    try {
                        var creationResult = await _usersCreatorQueriesService
                            .CreateMedic( GetInstituteId(), createMedicRequest );

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
        /// Assign medic to a Medical Team
        /// </summary>
        /// <param name="request">Request Body for Medical Team Assign</param>
        /// <param name="medicalTeamId">Id of medical team</param>
        /// <returns>Medic informations</returns>
        [HttpPost]
        [Route( "{medicalTeamId:guid}/assign" )]
        [Authorize( Policy = Policies.MedicalTeamReadWrite )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( UserModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        public IActionResult AssignMedicToMedicalTeam(
            Guid medicalTeamId, AssignMedicToMedicalTeamRequest request ) {
            User user = null;
            Medic medic = null;
            MedicalTeam medicalTeam = null;

            return RulesHelper
                .IfUserIsValid( request.UserId, out user )
                .IfMedicalTeamIsValid( medicalTeamId, out medicalTeam )
                .IfMedicalTeamIsOpen( medicalTeamId )
                .IfMedicIsValid( request.UserId, out medic )
                .IfMedicalTeamIsInMyInstitute( GetCurrentInstitute().Id, medicalTeam )
                .IfMedicIsNotAlreadyIntoTheMedicalTeam( request.UserId, medicalTeamId )
                .IfIHavePermissionsToAssignUserToThisMedicalTeam(
                    GetCurrentUser().Id, medicalTeamId, GetCurrentUserRoles() )
                .Then( () => {
                    _medicQueriesService.AddToMedicalTeam( request.UserId, medicalTeamId );

                    SaveChanges();

                    return Ok();
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Delete medic from medical team
        /// </summary>
        /// <param name="medicalTeamId">Medical team identifier</param>
        /// <param name="userId">User identifier of a medic</param>
        [HttpDelete]
        [Route( "{medicalTeamId:guid}/{userId:guid}" )]
        [Authorize( Policy = Policies.MedicalTeamReadWrite )]
        [SwaggerResponse( (int)HttpStatusCode.OK )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        public IActionResult RemoveMedicFromMedicalTeam( Guid medicalTeamId, Guid userId ) {
            MedicalTeam medicalTeam = null;
            Medic medic = null;

            return RulesHelper
                .IfMedicalTeamIsValid( medicalTeamId, out medicalTeam )
                .IfMedicalTeamIsInMyInstitute( GetCurrentInstitute().Id, medicalTeam )
                .IfMedicalTeamIsOpen( medicalTeamId )
                .IfMedicIsValid( userId, out medic )
                .Then( () => {
                    _medicQueriesService.RemoveFromMedicalTeam( userId, medicalTeam );
                    
                    SaveChanges();

                    return Ok();
                } )
                .ReturnResult();
        }
    }
}
