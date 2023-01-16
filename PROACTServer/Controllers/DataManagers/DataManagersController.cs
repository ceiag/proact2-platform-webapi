using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proact.Services.AuthorizationPolicies;
using Proact.Services.DatabaseValidityChecker;
using Proact.Services.Entities;
using Proact.Services.Entities.Users;
using Proact.Services.EntitiesMapper.DataManagers;
using Proact.Services.Models;
using Proact.Services.Models.DataManagers;
using Proact.Services.QueriesServices;
using Proact.Services.QueriesServices.DataManagers;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Linq;
using System.Net;

namespace Proact.Services.Controllers {

    [ApiController]
    [Route( ProactRouteConfiguration.DefaultRoute )]
    [Authorize( Policy = Policies.MedicalTeamReadWrite )]
    public class DataManagersController : ProactBaseController {
        private readonly IDataManagerQueriesService _dataManagerQueriesService;
        private readonly IMedicalTeamQueriesService _medicalTeamQueriesService;
        private readonly IUsersCreatorQueriesService _usersCreatorQueriesService;

        public DataManagersController(
            IChangesTrackingService changesTrackingService,
            IDataManagerQueriesService dataManagerQueriesService,
            IMedicalTeamQueriesService medicalTeamQueriesService,
            IUsersCreatorQueriesService usersCreatorQueriesService,
            ConsistencyRulesHelper consistencyRulesHelper )
            : base( changesTrackingService, consistencyRulesHelper ) {
            _dataManagerQueriesService = dataManagerQueriesService;
            _medicalTeamQueriesService = medicalTeamQueriesService;
            _usersCreatorQueriesService = usersCreatorQueriesService;
        }

        /// <summary>
        /// Create a DataManager
        /// </summary>
        /// <param name="request">Request model to create DataManager</param>
        /// <returns>DataManager informations</returns>
        [HttpPost]
        [Route( "create" )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( DataManagerModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        public IActionResult CreateDataManager( CreateDataManagerRequest request ) {
            return RulesHelper
                .Then( async () => {
                    try {
                        var creationResult = await _usersCreatorQueriesService
                            .CreateDataManager( GetInstituteId(), request );

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
        /// Assign DataManager to a Medical Team
        /// </summary>
        /// <param name="request">Request Body for Medical Team Assign</param>
        /// <param name="medicalTeamId">Id of medical team</param>
        /// <returns>Medic informations</returns>
        [HttpPost]
        [Route( "{medicalTeamId:guid}/assign" )]
        [SwaggerResponse( (int)HttpStatusCode.OK )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        public IActionResult AssignDataManagerToMedicalTeam(
            Guid medicalTeamId, AssignDataManagerToMedicalTeamRequest request ) {
            User user = null;
            DataManager dataManager = null;
            MedicalTeam medicalTeam = null;

            return RulesHelper
                .IfUserIsValid( request.UserId, out user )
                .IfUserIsInMyInstitute( GetInstituteId(), user )
                .IfMedicalTeamIsValid( medicalTeamId, out medicalTeam )
                .IfUserIsInMyInstitute( GetInstituteId(), user )
                .IfMedicalTeamIsOpen( medicalTeamId )
                .IfDataManagerIsValid( request.UserId, out dataManager )
                .IfDataManagerIsNotIntoTheMedicalTeam( request.UserId, medicalTeamId )
                .IfIHavePermissionsToAssignUserToThisMedicalTeam(
                    GetCurrentUser().Id, medicalTeamId, GetCurrentUserRoles() )
                .Then( () => {
                    _dataManagerQueriesService.AddToMedicalTeam( user.Id, medicalTeamId );
                    _medicalTeamQueriesService.AddAdmin( medicalTeamId, user.Id );

                    SaveChanges();

                    return Ok();
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Get list of DataManagers assigned to medical team
        /// </summary>
        /// <param name="medicalTeamId">Medical team identifier</param>
        /// <returns>The list of DataManagers</returns>
        [HttpGet]
        [Route( "{medicalTeamId:guid}" )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( DataManagerModel[] ) )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        public IActionResult GetDataManagers( Guid medicalTeamId ) {
            MedicalTeam medicalTeam = null;

            return RulesHelper
                .IfMedicalTeamIsValid( medicalTeamId, out medicalTeam )
                .IfMedicalTeamIsInMyInstitute( GetCurrentInstitute().Id, medicalTeam )
                .Then( () => {
                    return Ok( DataManagerEntityMapper.Map(
                        _medicalTeamQueriesService.Get( medicalTeamId ).DataManagers.ToList() ) );
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Return the list of DataManagers into the System
        /// </summary>
        /// <returns>DataManager information</returns>
        [HttpGet]
        [Route( "all" )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( DataManagerModel[] ) )]
        public IActionResult GetDataManagerAll() {
            return RulesHelper
                .Then( () => {
                    return Ok( DataManagerEntityMapper.Map(
                        _dataManagerQueriesService.GetAll( GetInstituteId() ) ) );
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Delete DataManager from medical team
        /// </summary>
        /// <param name="medicalTeamId">Medical team identifier</param>
        /// <param name="userId">User identifier of a medic</param>
        [HttpDelete]
        [Route( "{medicalTeamId:guid}/{userId:guid}" )]
        [SwaggerResponse( (int)HttpStatusCode.OK )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        public IActionResult RemoveDataManagerFromMedicalTeam( Guid medicalTeamId, Guid userId ) {
            MedicalTeam medicalTeam = null;
            DataManager DataManager = null;

            return RulesHelper
                .IfMedicalTeamIsValid( medicalTeamId, out medicalTeam )
                .IfMedicalTeamIsOpen( medicalTeamId )
                .IfDataManagerIsValid( userId, out DataManager )
                .IfUserIsInMyInstitute( GetInstituteId(), DataManager.User )
                .Then( () => {
                    _dataManagerQueriesService.RemoveFromMedicalTeam( userId, medicalTeam );

                    SaveChanges();

                    return Ok();
                } )
                .ReturnResult();
        }
    }
}
