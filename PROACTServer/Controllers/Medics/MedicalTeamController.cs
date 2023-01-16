using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proact.Services.AuthorizationPolicies;
using Proact.Services.Entities;
using Proact.Services.EntitiesMapper;
using Proact.Services.Models;
using Proact.Services.QueriesServices;
using Proact.Services.QueriesServices.DataManagers;
using Proact.Services.Services;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Proact.Services.Controllers {

    [ApiController]
    [Route( ProactRouteConfiguration.DefaultRoute )]
    public class MedicalTeamController : ProactBaseController {
        private readonly IGroupService _groupService;
        private readonly IMedicalTeamQueriesService _medicalTeamQueriesService;
        private readonly IMedicQueriesService _medicQueriesService;
        private readonly IProjectStateEditorService _projectTerminatorService;

        public MedicalTeamController( 
            IChangesTrackingService changesTrackingService,
            IMedicalTeamQueriesService medicalTeamQueriesService, IMedicQueriesService medicQueriesService,
            IProjectStateEditorService projectEditorService,
            ConsistencyRulesHelper consistencyRulesHelper, IGroupService groupService ) 
            : base( changesTrackingService, consistencyRulesHelper ) {
            _groupService = groupService;
            _medicalTeamQueriesService = medicalTeamQueriesService;
            _medicQueriesService = medicQueriesService;
            _projectTerminatorService = projectEditorService;
        }

        /// <summary>
        /// Returns Medical Teams associated to a Project
        /// </summary>
        /// <param name="projectId">Project identifier</param>
        /// <returns>The list of medical teams</returns>
        [HttpGet]
        [Authorize( Policy = Policies.MedicalTeamReadWrite )]
        [Route( "{projectId:guid}" )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( MedicalTeamModel[] ) )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        public IActionResult GetMedicalTeamsAssociatedToAProject( Guid projectId ) {
            Project project = null;

            return RulesHelper
                .IfProjectIsValid( projectId, out project )
                .IfProjectIsInMyInstitute( GetCurrentInstitute().Id, project )
                .Then( () => {
                    return Ok( MedicalTeamEntityMapper.Map( 
                        _medicalTeamQueriesService.GetAssociatedToAProject( projectId ).ToList() ) );
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Returns information about medical team current user is member of either as a medic or a patient
        /// </summary>
        /// <returns>The list of medical teams</returns>
        [HttpGet]
        [Authorize( Policy = Policies.MedicalTeamRead )]
        [Route( "my" )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( List<MedicalTeamModel> ) )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        public IActionResult GetMedicalTeamAssociatedToMe() {
            return RulesHelper
                .Then( () => {
                    var user = GetCurrentUser();

                    if ( HasRoleOf( Roles.Patient ) ) {
                        var patient = RulesHelper
                            .GetQueriesService<IPatientQueriesService>()
                            .Get( user.Id );

                        return Ok( MedicalTeamEntityMapper
                            .Map( new List<MedicalTeam>() { patient.MedicalTeam } ) );
                    }

                    if ( HasRoleOf( Roles.MedicalProfessional ) || HasRoleOf( Roles.MedicalTeamAdmin ) ) {
                        var medic = RulesHelper
                            .GetQueriesService<IMedicQueriesService>()
                            .Get( user.Id );

                        return Ok( MedicalTeamEntityMapper
                            .Map( medic.MedicalTeams ).ToList() );
                    }

                    if ( HasRoleOf( Roles.MedicalTeamDataManager ) ) {
                        var dataManager = RulesHelper
                            .GetQueriesService<IDataManagerQueriesService>()
                            .Get( user.Id );

                        return Ok( MedicalTeamEntityMapper
                            .Map( dataManager.MedicalTeams ).ToList() );
                    }

                    if ( HasRoleOf( Roles.Nurse ) ) {
                        var nurse = RulesHelper
                            .GetQueriesService<INurseQueriesService>()
                            .Get( user.Id );

                        return Ok( MedicalTeamEntityMapper
                            .Map( nurse.MedicalTeams )
                            .ToList() );
                    }

                    if ( HasRoleOf( Roles.Researcher ) ) {
                        var researcher = RulesHelper
                            .GetQueriesService<IResearcherQueriesService>()
                            .Get( user.Id );

                        return Ok( MedicalTeamEntityMapper
                            .Map( researcher.MedicalTeams )
                            .ToList() );
                    }

                    return NotFound();
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Returns information about medical team current user is member of either as a medic or a patient
        /// </summary>
        /// <param name="projectId">Project identifier</param>
        /// <returns>The list of medical teams</returns>
        [HttpGet]
        [Authorize( Policy = Policies.MedicalTeamRead )]
        [Route( "{projectId:guid}/my" )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( List<MedicalTeamModel> ) )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        public IActionResult GetMedicalTeamAssociatedToMe( Guid projectId ) {
            Project project = null;

            return RulesHelper
                .IfProjectIsValid( projectId, out project )
                .IfProjectIsInMyInstitute( GetCurrentInstitute().Id, project )
                .Then( () => {
                    var user = GetCurrentUser();

                    if ( HasRoleOf( Roles.Patient ) ) {
                        var patient = RulesHelper.GetQueriesService<IPatientQueriesService>().Get( user.Id );

                        return Ok( MedicalTeamEntityMapper.Map( 
                            new List<MedicalTeam>() { patient.MedicalTeam } ) );
                    }

                    if ( HasRoleOf( Roles.MedicalProfessional ) || HasRoleOf( Roles.MedicalTeamAdmin ) ) {
                        var medic = RulesHelper.GetQueriesService<IMedicQueriesService>().Get( user.Id );

                        return Ok( MedicalTeamEntityMapper.Map( medic.MedicalTeams )
                            .Where( x => x.Project.ProjectId == projectId ).ToList() );
                    }

                    if ( HasRoleOf( Roles.MedicalTeamDataManager ) ) {
                        var dataManager = RulesHelper
                            .GetQueriesService<IDataManagerQueriesService>()
                            .Get( user.Id );

                        return Ok( MedicalTeamEntityMapper.Map( dataManager.MedicalTeams )
                            .Where( x => x.Project.ProjectId == projectId ).ToList() );
                    }

                    if ( HasRoleOf( Roles.Nurse ) ) {
                        var nurse = RulesHelper.GetQueriesService<INurseQueriesService>().Get( user.Id );

                        return Ok( MedicalTeamEntityMapper.Map( nurse.MedicalTeams )
                            .Where( x => x.Project.ProjectId == projectId ).ToList() );
                    }

                    if ( HasRoleOf( Roles.Researcher ) ) {
                        var researcher = RulesHelper.GetQueriesService<IResearcherQueriesService>().Get( user.Id );

                        return Ok( MedicalTeamEntityMapper.Map( researcher.MedicalTeams )
                            .Where( x => x.Project.ProjectId == projectId ).ToList() );
                    }

                    return NotFound();
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Returns list of MedicalTeam where I'm administrator
        /// </summary>
        /// <returns>The list of medical teams</returns>
        [HttpGet]
        [Authorize( Policy = Policies.MedicalTeamReadWrite )]
        [Route( "admins/my" )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( MedicalTeamModel[] ) )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        public IActionResult GetMedicalTeamsWhereImAdmin() {
            var user = GetCurrentUser();
            MedicAdmin medicAdmin = null;

            return RulesHelper
                .IfMedicAdminIsValid( user.Id, out medicAdmin )
                .Then( () => {
                    return Ok( MedicalTeamEntityMapper.Map(
                        _medicalTeamQueriesService
                            .GetMedicalTeamsWhereUserIsAdmin( user.Id )
                            .ToList() ) );
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Returns information about medical team
        /// </summary>
        /// <param name="projectId">Project identifier</param>
        /// <param name="medicalTeamId">Medical team identifier</param>
        /// <returns>Medical team information</returns>
        [HttpGet]
        [Authorize( Policy = Policies.MedicalTeamRead )]
        [Route( "{projectId:guid}/{medicalTeamId:guid}" )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( MedicalTeamModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        public IActionResult GetMedicalTeam( Guid projectId, Guid medicalTeamId ) {
            Project project = null;
            MedicalTeam medicalTeam = null;

            return RulesHelper
                .IfProjectIsValid( projectId, out project )
                .IfProjectIsInMyInstitute( GetCurrentInstitute().Id, project )
                .IfMedicalTeamIsValid( medicalTeamId, out medicalTeam )
                .Then( () => {
                    return Ok( MedicalTeamEntityMapper.Map( medicalTeam ) );
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Create a new medical team within the project
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /Todo
        ///     {
        ///         "phone": "3282612469",
        ///         "addressLine1": "via stella 2",
        ///         "addressLine2": "string",
        ///         "city": "Cerignola",
        ///         "stateOrProvince": "FG",
        ///         "regionCode": "IT-GE",
        ///         "postalCode": "71042",
        ///         "country": "Italy",
        ///         "timeZone": "GMT+1",
        ///         "name": "TeamMedico00"
        ///     }
        ///     
        /// </remarks>
        /// <param name="projectId">Project identifier</param>
        /// <param name="medicalTeamData">Medical team information</param>
        /// <returns>Medical team information</returns>
        [HttpPost]
        [Authorize( Policy = Policies.MedicalTeamReadWrite )]
        [Route( "{projectId:guid}" )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( MedicalTeamModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.Conflict, Type = typeof( ErrorModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.BadRequest, Type = typeof( ErrorModel ) )]
        public IActionResult CreateMedicalTeam(
            Guid projectId, [FromBody] MedicalTeamCreateRequest medicalTeamData ) {

            Project project = null;

            return RulesHelper
                .IfProjectIsValid( projectId, out project )
                .IfProjectIsInMyInstitute( GetCurrentInstitute().Id, project )
                .IfProjectIsOpen( projectId )
                .Then( () => {
                    var medicalTeam =_medicalTeamQueriesService.Create( projectId, medicalTeamData );

                    SaveChanges();

                    return Ok( MedicalTeamEntityMapper.Map( medicalTeam ) );
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Updates medical team information
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /Todo
        ///     {
        ///         "phone": "3282612469",
        ///         "addressLine1": "via le mani dal naso",
        ///         "addressLine2": "string",
        ///         "city": "Cerignola",
        ///         "stateOrProvince": "FG",
        ///         "regionCode": "IT-GE",
        ///         "postalCode": "71042",
        ///         "country": "Italy",
        ///         "timeZone": "GMT+1",
        ///         "name": "TeamMedico00"
        ///     }
        ///     
        /// </remarks>
        /// <param name="projectId">Project identifier</param>
        /// <param name="medicalTeamId">Medical team identifier</param>
        /// <param name="medicalTeamData">Medical team information</param>
        /// <returns>Medical team information</returns>
        [HttpPut]
        [Authorize( Policy = Policies.MedicalTeamReadWrite )]
        [Route( "{projectId:guid}/{medicalTeamId:guid}" )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( MedicalTeamModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.Conflict, Type = typeof( ErrorModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.BadRequest, Type = typeof( ErrorModel ) )]
        public IActionResult UpdateMedicalTeam(
            Guid projectId, Guid medicalTeamId, [FromBody] MedicalTeamUpdateRequest medicalTeamData ) {
            Project project = null;
            MedicalTeam medicalTeam = null;

            return RulesHelper
                .IfProjectIsValid( projectId, out project )
                .IfProjectIsInMyInstitute( GetCurrentInstitute().Id, project )
                .IfMedicalTeamIsValid( medicalTeamId, out medicalTeam )
                .IfNameAvailableForExistingMedicalTeam( medicalTeamData.Name, medicalTeamId )
                .Then( () => {
                    var medicalTeamUpdated = _medicalTeamQueriesService
                        .Update( medicalTeamId, medicalTeamData );

                    SaveChanges();

                    return Ok( MedicalTeamEntityMapper.Map( medicalTeamUpdated ) );
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Get info of the user administrating medical team
        /// </summary>
        /// <param name="projectId">Project identifier</param>
        /// <param name="medicalTeamId">Medical team identifier</param>
        /// <returns>Medical team admin information</returns>
        [HttpGet]
        [Authorize( Policy = Policies.MedicalTeamReadWrite )]
        [Route( "{projectId:guid}/{medicalTeamId:guid}/getadmins" )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( MedicModel[] ) )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        public IActionResult GetAdmins( Guid projectId, Guid medicalTeamId ) {
            Project project = null;
            MedicalTeam medicalTeam = null;

            return RulesHelper
                .IfProjectIsValid( projectId, out project )
                .IfProjectIsInMyInstitute( GetCurrentInstitute().Id, project )
                .IfMedicalTeamIsValid( medicalTeamId, out medicalTeam )
                .Then( () => {
                    return Ok( MedicEntityMapper.Map( 
                        _medicalTeamQueriesService.GetAdmins( medicalTeamId ).ToList() ) );
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Assign medical team administrator
        /// </summary>
        /// <param name="projectId">Project identifier</param>
        /// <param name="medicalTeamId">Medical team identifier</param>
        /// <param name="userId">UserId</param>
        /// <returns>Administrator information</returns>
        [HttpPut]
        [Authorize( Policy = Policies.MedicalTeamReadWrite )]
        [Route( "{projectId:guid}/{medicalTeamId:guid}/setadmin" )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( MedicModel[] ) )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.BadRequest, Type = typeof( ErrorModel ) )]
        public IActionResult AssignAdmin( Guid projectId, Guid medicalTeamId, Guid userId ) {
            Project project = null;
            MedicalTeam medicalTeam = null;
            Medic medic = null;

            return RulesHelper
                .IfProjectIsValid( projectId, out project )
                .IfMedicalTeamIsValid( medicalTeamId, out medicalTeam )
                .IfMedicalTeamIsInMyInstitute( GetCurrentInstitute().Id, medicalTeam )
                .IfMedicalTeamIsOpen( medicalTeamId )
                .IfMedicIsValid( userId, out medic )
                .IfMedicIsNotAlreadyMedicalTeamAdmin( userId, medicalTeamId )
                .Then( async () => {
                    _medicQueriesService.AddToMedicalTeam( userId, medicalTeamId );

                    SaveChanges();

                    _medicalTeamQueriesService.AddAdmin( medicalTeamId, userId );

                    await AssignMedicalTeamAdminAzureADRoleIfNotAssigned( medic );
                    SaveChanges();

                    return Ok( MedicEntityMapper.Map(
                        _medicalTeamQueriesService.GetAdmins( medicalTeamId ).ToList() ) );
                } )
                .ReturnResult();
        }

        private async Task AssignMedicalTeamAdminAzureADRoleIfNotAssigned( Medic medic ) {
            try {
                var groupId = await _groupService.GetGroupIdByName( Roles.MedicalTeamAdmin );
                await _groupService.AddMember( groupId, medic.User.AccountId );
            }
            catch ( Exception ex ) { }
        }

        /// <summary>
        /// Delete medical team administrator
        /// </summary>
        /// <param name="projectId">Project identifier</param>
        /// <param name="medicalTeamId">Medical team identifier</param>
        /// <param name="userId">UserId</param>
        /// <returns>Administrator information</returns>
        [HttpDelete]
        [Authorize( Policy = Policies.MedicalTeamReadWrite )]
        [Route( "{projectId:guid}/{medicalTeamId:guid}/admin" )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( UserModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.BadRequest, Type = typeof( ErrorModel ) )]
        public IActionResult DeleteAdmin( Guid projectId, Guid medicalTeamId, Guid userId ) {
            Project project = null;
            MedicalTeam medicalTeam = null;
            Medic medic = null;

            return RulesHelper
                .IfProjectIsValid( projectId, out project )
                .IfMedicalTeamIsValid( medicalTeamId, out medicalTeam )
                .IfMedicalTeamIsInMyInstitute( GetCurrentInstitute().Id, medicalTeam )
                .IfMedicalTeamIsOpen( medicalTeamId )
                .IfMedicIsValid( userId, out medic )
                .IfMedicIsMedicalTeamAdmin( userId, medicalTeamId )
                .IfMedicalTeamHasAlmostTwoOrMoreAdmins( medicalTeamId )
                .Then( () => {
                    _medicalTeamQueriesService.RemoveAdmin( medicalTeamId, userId );

                    SaveChanges();

                    return Ok();
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Close MedicalTeam
        /// </summary>
        /// <param name="medicalTeamId">Medical team identifier</param>
        /// <param name="projectId">Project identifier</param>
        [HttpPut]
        [Route( "{projectId:guid}/{medicalTeamId:guid}/close" )]
        [Authorize( Policy = Policies.MedicalTeamReadWrite )]
        [SwaggerResponse( (int)HttpStatusCode.OK )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        public IActionResult CloseMedicalTeam( Guid projectId, Guid medicalTeamId ) {
            MedicalTeam medicalTeam = null;
            Project project = null;

            return RulesHelper
                .IfProjectIsValid( projectId, out project )
                .IfProjectIsOpen( projectId )
                .IfMedicalTeamIsValid( medicalTeamId, out medicalTeam )
                .IfMedicalTeamIsInMyInstitute( GetCurrentInstitute().Id, medicalTeam )
                .IfMedicalTeamIsOpen( medicalTeamId )
                .Then( () => {
                    _medicalTeamQueriesService.Close( medicalTeamId );

                    SaveChanges();

                    return Ok();
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Open MedicalTeam
        /// </summary>
        /// <param name="medicalTeamId">Medical team identifier</param>
        /// <param name="projectId">Project identifier</param>
        [HttpPut]
        [Route( "{projectId:guid}/{medicalTeamId:guid}/open" )]
        [Authorize( Policy = Policies.MedicalTeamReadWrite )]
        [SwaggerResponse( (int)HttpStatusCode.OK )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        public IActionResult OpenMedicalTeam( Guid projectId, Guid medicalTeamId ) {
            MedicalTeam medicalTeam = null;
            Project project = null;

            return RulesHelper
                .IfProjectIsValid( projectId, out project )
                .IfProjectIsOpen( projectId )
                .IfMedicalTeamIsValid( medicalTeamId, out medicalTeam )
                .IfMedicalTeamIsInMyInstitute( GetCurrentInstitute().Id, medicalTeam )
                .Then( () => {
                    _medicalTeamQueriesService.Open( medicalTeamId );

                    SaveChanges();

                    return Ok();
                } )
                .ReturnResult();
        }
    }
}
