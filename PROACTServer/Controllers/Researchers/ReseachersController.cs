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
    public class ResearchersController : ProactBaseController {
        private readonly IMedicalTeamQueriesService _medicalTeamQueriesService;
        private readonly IResearcherQueriesService _reseracherQueriesService;
        private readonly IUsersCreatorQueriesService _usersCreatorQueriesService;

        public ResearchersController(
            IChangesTrackingService changesTrackingService, IMedicalTeamQueriesService medicalTeamQueriesService,
            IResearcherQueriesService reseracherQueriesService, IUsersCreatorQueriesService usersCreatorQueriesService,
            ConsistencyRulesHelper consistencyRulesHelper )
            : base( changesTrackingService, consistencyRulesHelper ) {
            _medicalTeamQueriesService = medicalTeamQueriesService;
            _reseracherQueriesService = reseracherQueriesService;
            _usersCreatorQueriesService = usersCreatorQueriesService;
        }

        /// <summary>
        /// Get list of researcher assigned to medical team
        /// </summary>
        /// <param name="medicalTeamId">Medical team identifier</param>
        /// <returns>The list of medics</returns>
        [HttpGet]
        [Route( "{medicalTeamId:guid}" )]
        [Authorize( Policy = Policies.MedicalTeamReadWrite )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( ResearcherModel[] ) )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        public IActionResult GetResearchers( Guid medicalTeamId ) {
            MedicalTeam medicalTeam = null;

            return RulesHelper
                .IfMedicalTeamIsValid( medicalTeamId, out medicalTeam )
                .IfMedicalTeamIsInMyInstitute( GetCurrentInstitute().Id, medicalTeam )
                .Then( () => {
                    return Ok( ResearcherEntityMapper.Map(
                        _medicalTeamQueriesService.Get( medicalTeamId ).Reseachers.ToList() ) );
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Get researcher information
        /// </summary>
        /// <param name = "medicalTeamId"> Medical team identifier</param>
        /// <param name = "userId"> User identifier of a medic</param>
        /// <returns>Medic information</returns>
        [HttpGet]
        [Route( "{medicalTeamId:guid}/{userId:guid}" )]
        [Authorize( Policy = Policies.MedicalTeamReadWrite )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( ResearcherModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        public IActionResult Get( Guid medicalTeamId, Guid userId ) {
            MedicalTeam medicalTeam = null;
            Researcher researcher = null;

            return RulesHelper
                .IfMedicalTeamIsValid( medicalTeamId, out medicalTeam )
                .IfMedicalTeamIsInMyInstitute( GetCurrentInstitute().Id, medicalTeam )
                .IfResearcherIsValid( userId, out researcher )
                .Then( () => {
                    return Ok( ResearcherEntityMapper.Map( _reseracherQueriesService.Get( userId ) ) );
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Return the list of Researcher into the institute
        /// </summary>
        /// <returns>Medic information</returns>
        [HttpGet]
        [Route( "all" )]
        [Authorize( Policy = Policies.MedicalTeamReadWrite )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( ResearcherModel[] ) )]
        public IActionResult GetAll() {
            return RulesHelper
                .Then( () => {
                    var currentInstitute = GetCurrentInstitute();
                    return Ok( ResearcherEntityMapper.Map(
                        _reseracherQueriesService.GetsAll( currentInstitute.Id ).ToList() ) );
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Create a Researcher
        /// </summary>
        /// <param name="createResearcherRequest">Request model to create Researcher</param>
        /// <returns>Researcher informations</returns>
        [HttpPost]
        [Route( "create" )]
        [Authorize( Policy = Policies.MedicalTeamReadWrite )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( ResearcherModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        public IActionResult Create( CreateResearcherRequest createResearcherRequest ) {
            return RulesHelper
                .Then( async () => {
                    try {
                        var creationResult = await _usersCreatorQueriesService
                            .CreateResearcher( GetInstituteId(), createResearcherRequest );

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
        /// Assign researcher to a Medical Team
        /// </summary>
        /// <param name="request">Request Body for Medical Team Assign</param>
        /// <param name="medicalTeamId">Id of medical team</param>
        /// <returns>Researcher informations</returns>
        [HttpPost]
        [Route( "{medicalTeamId:guid}/assign" )]
        [Authorize( Policy = Policies.MedicalTeamReadWrite )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( UserModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        public IActionResult AssignToMedicalTeam(
            Guid medicalTeamId, AssignResearcherToMedicalTeamRequest request ) {
            User user = null;
            Researcher researcher = null;
            MedicalTeam medicalTeam = null;

            return RulesHelper
                .IfUserIsValid( request.UserId, out user )
                .IfMedicalTeamIsValid( medicalTeamId, out medicalTeam )
                .IfMedicalTeamIsOpen( medicalTeamId )
                .IfResearcherIsValid( request.UserId, out researcher )
                .IfMedicalTeamIsInMyInstitute( GetCurrentInstitute().Id, medicalTeam )
                .IfResearcherIsNotAlreadyIntoTheMedicalTeam( request.UserId, medicalTeamId )
                .IfIHavePermissionsToAssignUserToThisMedicalTeam(
                    GetCurrentUser().Id, medicalTeamId, GetCurrentUserRoles() )
                .Then( () => {
                    _reseracherQueriesService.AddToMedicalTeam( request.UserId, medicalTeamId );

                    SaveChanges();

                    return Ok();
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Delete researcher from medical team
        /// </summary>
        /// <param name="medicalTeamId">Medical team identifier</param>
        /// <param name="userId">User identifier of a medic</param>
        [HttpDelete]
        [Route( "{medicalTeamId:guid}/{userId:guid}" )]
        [Authorize( Policy = Policies.MedicalTeamReadWrite )]
        [SwaggerResponse( (int)HttpStatusCode.OK )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        public IActionResult RemoveFromMedicalTeam( Guid medicalTeamId, Guid userId ) {
            MedicalTeam medicalTeam = null;
            Researcher researcher = null;

            return RulesHelper
                .IfMedicalTeamIsValid( medicalTeamId, out medicalTeam )
                .IfMedicalTeamIsInMyInstitute( GetCurrentInstitute().Id, medicalTeam )
                .IfMedicalTeamIsOpen( medicalTeamId )
                .IfResearcherIsValid( userId, out researcher )
                .Then( () => {
                    _reseracherQueriesService.RemoveFromMedicalTeam( userId, medicalTeam );

                    SaveChanges();

                    return Ok();
                } )
                .ReturnResult();
        }
    }
}
