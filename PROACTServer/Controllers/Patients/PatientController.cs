using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proact.Services.AuthorizationPolicies;
using Proact.Services.Entities;
using Proact.Services.Models;
using Proact.Services.QueriesServices;
using Proact.Services.Services.EmailSender;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Net;

namespace Proact.Services.Controllers {
    [ApiController]
    [Route( ProactRouteConfiguration.DefaultRoute )]
    public class PatientController : ProactBaseController {
        private readonly IPatientQueriesService _patientQueriesService;
        private readonly IUsersCreatorQueriesService _usersCreatorQueriesService;
        private readonly IEmailSenderService _emailSenderService;

        public PatientController( 
            IChangesTrackingService changesTrackingService,
            ConsistencyRulesHelper consistencyRulesHelper,
            IPatientQueriesService patientQueriesService,
            IUsersCreatorQueriesService usersCreatorQueriesService,
            IEmailSenderService emailSenderService )
            : base( changesTrackingService, consistencyRulesHelper ) {
            _patientQueriesService = patientQueriesService;
            _usersCreatorQueriesService = usersCreatorQueriesService;
            _emailSenderService = emailSenderService;
        }

        /// <summary>
        /// Returns a list of patients associated with specified medical team
        /// </summary>
        /// <param name="medicalTeamId">Medical team identifier</param>
        /// <returns>The list of patients of a medical team</returns>
        [HttpGet]
        [Route( "{medicalTeamId:guid}" )]
        [Authorize( Policy = Policies.MedicalTeamRead )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( PatientModel[] ) )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        public IActionResult GetPatientsFromMedicalTeam( Guid medicalTeamId ) {
            MedicalTeam medicalTeam = null;

            return RulesHelper
                .IfMedicalTeamIsValid( medicalTeamId, out medicalTeam )
                .IfMedicalTeamIsInMyInstitute( GetCurrentInstitute().Id, medicalTeam )
                .Then( () => {
                    return Ok( PatientEntityMapper.Map( 
                        _patientQueriesService.GetFromMedicalTeam( medicalTeamId ), 
                            HasRoleOf( Roles.Researcher ) ) );
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Returns a list of patients associated within a Project
        /// </summary>
        /// <param name="projectId">Project identifier</param>
        /// <returns>The list of patients within a Project</returns>
        [HttpGet]
        [Route( "projects/{projectId:guid}" )]
        [Authorize( Policy = Policies.PatientsRead )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( PatientModel[] ) )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        public IActionResult GetPatientsFromProject( Guid projectId ) {
            Project project = null;

            return RulesHelper
                .IfProjectIsValid( projectId, out project )
                .IfProjectIsInMyInstitute( GetInstituteId(), project )
                .Then( () => {
                    return Ok( PatientEntityMapper.Map(
                        _patientQueriesService.GetFromProject( projectId ), 
                        HasRoleOf( Roles.Researcher ) ) );
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Returns a list of patients into the system
        /// </summary>
        /// <returns>The list of patients of a medical team</returns>
        [HttpGet]
        [Route( "all" )]
        [Authorize( Policy = Policies.MedicalTeamRead )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( PatientModel[] ) )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        public IActionResult GetPatientsAll() {
            return RulesHelper
                .Then( () => {
                    return Ok( PatientEntityMapper.Map( 
                        _patientQueriesService.GetsAll( GetInstituteId() ), 
                        HasRoleOf( Roles.Researcher ) ) );
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Get patient information
        /// </summary>
        /// <param name="medicalTeamId">Medical team identifier</param>
        /// <param name="userId">User identifier of a patient</param>
        /// <returns>Patient information</returns>
        [HttpGet]
        [Route( "{medicalTeamId:guid}/{userId:guid}" )]
        [Authorize( Policy = Policies.MedicalTeamRead )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( PatientModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        public IActionResult GetPatient( Guid medicalTeamId, Guid userId ) {
            MedicalTeam medicalTeam = null;
            Patient patient = null;

            return RulesHelper
                .IfMedicalTeamIsValid( medicalTeamId, out medicalTeam )
                .IfPatientIsValid( userId, out patient )
                .IfUserIsInMyInstitute( GetCurrentInstitute().Id, patient.User )
                .Then( () => {
                    return Ok( PatientEntityMapper.Map( 
                        _patientQueriesService.Get( userId ), HasRoleOf( Roles.Researcher ) ) );
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Create a Patient
        /// </summary>
        /// <param name="patientCreationRequest">Data for patient creation</param>
        /// <returns>Patient information</returns>
        [HttpPost]
        [Route( "create" )]
        [Authorize( Policy = Policies.MedicalTeamReadWrite )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( PatientModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        public IActionResult CreatePatient( PatientCreateRequest patientCreationRequest ) {
            return RulesHelper
                .Then( async () => {
                    try {
                        var creationResult = await _usersCreatorQueriesService
                            .CreatePatient( GetInstituteId(), patientCreationRequest );

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
        /// Suspend patients with dei treatment expired
        /// </summary>
        [HttpPost]
        [Route( "SuspendExpiredPatients" )]
        [SwaggerResponse( (int)HttpStatusCode.OK )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        public IActionResult SuspendExpiredPatients() {
            _patientQueriesService.SuspendAllPatientsWithTreatmentExpired();
            return Ok();
        }

        /// <summary>
        /// Assign patient to a Medical Team
        /// </summary>
        /// <param name="medicalTeamId">Medical team identifier</param>
        /// <param name="assignData">Data for patient assegnation</param>
        /// <returns>Patient information</returns>
        [HttpPost]
        [Route( "{medicalTeamId:guid}/assign" )]
        [Authorize( Policy = Policies.MedicalTeamReadWrite )]
        [SwaggerResponse( (int)HttpStatusCode.OK )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        public IActionResult AssignPatientToMedicalTeam( 
            Guid medicalTeamId, AssignPatientToMedicalTeamRequest assignData ) {
            Patient patient = null;
            MedicalTeam medicalTeam = null;

            return RulesHelper
                .IfPatientIsValid( assignData.UserId, out patient )
                .IfMedicalTeamIsValid( medicalTeamId, out medicalTeam )
                .IfMedicalTeamIsOpen( medicalTeamId )
                .IfUserIsInMyInstitute( GetCurrentInstitute().Id, patient.User )
                .IfIHavePermissionsToAssignUserToThisMedicalTeam( 
                    GetCurrentUser().Id, medicalTeamId, GetCurrentUserRoles() )
                .Then( async () => {
                    _patientQueriesService.AddToMedicalTeam( medicalTeamId, assignData );
                    SaveChanges();

                    try {
                        await _emailSenderService.SendWelcomeEmailTo( medicalTeam.ProjectId, patient );
                    }
                    catch {}

                    return Ok();
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Remove patient from medical team
        /// </summary>
        /// <param name="medicalTeamId">Medical team identifier</param>
        /// <param name="userId">User identifier of a patient</param>
        [HttpDelete]
        [Route( "{medicalTeamId:guid}/{userId:guid}" )]
        [Authorize( Policy = Policies.MedicalTeamReadWrite )]
        [SwaggerResponse( (int)HttpStatusCode.OK )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        public IActionResult RemovePatientFromMedicalTeam( Guid medicalTeamId, Guid userId ) {
            MedicalTeam medicalTeam = null;
            Patient patient = null;

            return RulesHelper
                .IfMedicalTeamIsValid( medicalTeamId, out medicalTeam )
                .IfMedicalTeamIsOpen( medicalTeamId )
                .IfPatientIsValid( userId, out patient )
                .IfUserIsInMyInstitute( GetCurrentInstitute().Id, patient.User )
                .Then( () => {
                    _patientQueriesService.RemoveFromMedicalTeam( userId );

                    SaveChanges();

                    return Ok();
                } )
                .ReturnResult();   
        }
    }
}
