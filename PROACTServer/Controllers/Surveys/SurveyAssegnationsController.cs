using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proact.Services.AuthorizationPolicies;
using Proact.Services.Entities;
using Proact.Services.Models;
using Proact.Services.QueriesServices;
using Proact.Services.QueriesServices.Surveys.Scheduler;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Net;

namespace Proact.Services.Controllers.Surveys;
[ApiController]
[Route( ProactRouteConfiguration.DefaultRoute )]
public class SurveyAssegnationsController : ProactBaseController {
    private readonly ISurveyAssignationQueriesService _surveyAssignmentsQueriesService;
    private readonly ISurveyAnswerToQuestionEditorService _surveyAnswerToQuestionEditorService;
    private readonly ISurveySchedulerQueriesService _surveySchedulerQueriesService;
    private readonly ISurveyQueriesService _surveyQueriesService;
    private readonly ISurveySchedulerDispatcherService _surveySchedulerDispatcherService;

    public SurveyAssegnationsController(
        IChangesTrackingService changesTrackingService,
        ISurveyAssignationQueriesService surveyAssignmentsQueriesService,
        ISurveyAnswerToQuestionEditorService surveyAnswerToQuestionEditorService,
        ISurveySchedulerQueriesService surveySchedulerQueriesService,
        ISurveyQueriesService surveyQueriesService,
        ISurveySchedulerDispatcherService surveySchedulerDispatcherService,
        ConsistencyRulesHelper consistencyRulesHelper )
        : base( changesTrackingService, consistencyRulesHelper ) {
        _surveyAssignmentsQueriesService = surveyAssignmentsQueriesService;
        _surveyAnswerToQuestionEditorService = surveyAnswerToQuestionEditorService;
        _surveySchedulerQueriesService = surveySchedulerQueriesService;
        _surveySchedulerDispatcherService = surveySchedulerDispatcherService;

        _surveyQueriesService = surveyQueriesService;
    }

    /// <summary>
    /// Assign a Survey to a list of Patients
    /// </summary>
    /// <param name="surveyId">Survey Identifier<param>
    /// <param name="request">Assignation request params</param>
    /// <returns>Assignment of Survey</returns>
    [HttpPost]
    [Route( "{surveyId:guid}" )]
    [Authorize( Policy = Policies.SurveysWrite )]
    [SwaggerResponse( (int)HttpStatusCode.OK )]
    [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
    [SwaggerResponse( (int)HttpStatusCode.BadRequest, Type = typeof( ErrorModel ) )]
    public IActionResult AssignSurveyToPatients(
        Guid surveyId, CreateScheduledSurveyRequest request ) {
        Survey survey = null;

        return RulesHelper
            .IfPatientsAreValid( request.UserIds )
            .IfSurveyIsValid( surveyId, out survey )
            .IfPatientsAreInProject( (Guid)survey.ProjectId, request.UserIds )
            .Then( async () => {
                var schedulers = _surveySchedulerQueriesService.Create( request );
                SaveChanges();

                _surveyQueriesService.SetState( surveyId, SurveyState.PUBLISHED );
                _surveyQueriesService.SetAssignationProperties(
                    surveyId, request.StartTime, request.ExpireTime, request.Reccurence );
                SaveChanges();

                if ( request.StartTime.Date == DateTime.UtcNow.Date ) {
                    await _surveySchedulerDispatcherService.SendSurveyToPatientsNow( schedulers );
                }

                return Ok();
            } )
            .ReturnResult();
    }

    /// <summary>
    /// Get Survey Assignments
    /// </summary>
    /// <param name="userId">UserId of patient</param>
    /// <returns>Surveys Assignments</returns>
    [HttpGet]
    [Route( "{userId:guid}" )]
    [Authorize( Policy = Policies.SurveysRead )]
    [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( List<SurveyAssignationModel> ) )]
    [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
    public IActionResult GetSurveysAssignedToPatient( Guid userId ) {
        Patient patient = null;

        return RulesHelper
            .IfPatientIsValid( userId, out patient )
            .Then( () => {
                return Ok( SurveyAssignationEntityMapper.Map(
                    _surveyAssignmentsQueriesService.GetById( userId ) ) );
            } )
            .ReturnResult();
    }

    /// <summary>
    /// Get Survey Assigned to me
    /// </summary>
    /// <returns>Surveys Assignments</returns>
    [HttpGet]
    [Route( "me" )]
    [Authorize( Policy = Policies.SurveysAnswerReadWrite )]
    [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( List<SurveyAssignationModel> ) )]
    [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
    public IActionResult GetSurveysAssignedToMe() {
        Patient patient = null;
        Guid userId = GetCurrentUser().Id;

        return RulesHelper
            .IfPatientIsValid( userId, out patient )
            .Then( () => {
                return Ok( SurveyAssignationEntityMapper.Map(
                    _surveyAssignmentsQueriesService.GetByUserId( userId ) ) );
            } )
            .ReturnResult();
    }

    /// <summary>
    /// Get Survey Assignments completed only
    /// </summary>
    /// <returns>Surveys completed by a patient</returns>
    [HttpGet]
    [Route( "completed/me" )]
    [Authorize( Policy = Policies.SurveysAnswerReadWrite )]
    [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( List<SurveyAssignationModel> ) )]
    [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
    public IActionResult GetCompletedSurveysAssignedToMe() {
        Patient patient = null;
        Guid userId = GetCurrentUser().Id;

        return RulesHelper
            .IfPatientIsValid( userId, out patient )
            .Then( () => {
                return Ok( SurveyAssignationEntityMapper.Map(
                    _surveyAssignmentsQueriesService.GetCompletedSurveysAssigned( userId ) ) );
            } )
            .ReturnResult();
    }

    /// <summary>
    /// Get Survey Assignments completed only
    /// </summary>
    /// <param name="userId">UserId of patient</param>
    /// <returns>Surveys completed by a patient</returns>
    [HttpGet]
    [Route( "{userId:guid}/completed" )]
    [Authorize( Policy = Policies.SurveysRead )]
    [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( List<SurveyAssignationModel> ) )]
    [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
    public IActionResult GetCompletedSurveysAssignedToPatient( Guid userId ) {
        Patient patient = null;

        return RulesHelper
            .IfPatientIsValid( userId, out patient )
            .IfMedicAndPatientAreIntoSameMedicalTeam(
                GetCurrentUser().Id, patient.UserId, GetCurrentUserRoles() )
            .Then( () => {
                return Ok( SurveyAssignationEntityMapper.Map(
                    _surveyAssignmentsQueriesService.GetCompletedSurveysAssigned( userId ) ) );
            } )
            .ReturnResult();
    }

    /// <summary>
    /// Get Surveys that patient must compile now
    /// </summary>
    /// <returns>Surveys that patient must compile now</returns>
    [HttpGet]
    [Route( "notcompleted/me" )]
    [Authorize( Policy = Policies.SurveysAnswerReadWrite )]
    [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( List<SurveyAssignationModel> ) )]
    [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
    public IActionResult GetNotCompletedSurveysAssignedToMe() {
        Patient patient = null;
        Guid userId = GetCurrentUser().Id;

        return RulesHelper
            .IfPatientIsValid( userId, out patient )
            .Then( () => {
                return Ok( SurveyAssignationEntityMapper.Map(
                    _surveyAssignmentsQueriesService.GetAvailableSurveysForUser( userId ) ) );
            } )
            .ReturnResult();
    }

    /// <summary>
    /// Get Survey Assignments not completed only
    /// </summary>
    /// <param name="userId">UserId of patient</param>
    /// <returns>Surveys not completed by a patient</returns>
    [HttpGet]
    [Route( "{userId:guid}/notcompleted" )]
    [Authorize( Policy = Policies.SurveysRead )]
    [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( List<SurveyAssignationModel> ) )]
    [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
    public IActionResult GetNotCompletedSurveysAssignedToPatient( Guid userId ) {
        Patient patient = null;

        return RulesHelper
            .IfPatientIsValid( userId, out patient )
            .IfMedicAndPatientAreIntoSameMedicalTeam(
                GetCurrentUser().Id, patient.UserId, GetCurrentUserRoles() )
            .Then( () => {
                return Ok( SurveyAssignationEntityMapper.Map(
                    _surveyAssignmentsQueriesService.GetNotCompletedSurveysAssigned( userId ) ) );
            } )
            .ReturnResult();
    }

    /// <summary>
    /// Get Patients assigned to a Survey
    /// </summary>
    /// <param name="surveyId">Id of the Survey</param>
    /// <returns>Patients assigned to a Survey</returns>
    [HttpGet]
    [Route( "{surveyId:guid}/patients" )]
    [Authorize( Policy = Policies.SurveysRead )]
    [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( List<SurveyAssignationModel> ) )]
    [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
    public IActionResult GetPatientsAssignedToSurvey( Guid surveyId ) {
        Survey survey = null;

        return RulesHelper
            .IfSurveyIsValid( surveyId, out survey )
            .Then( () => {
                return Ok( SurveyAssignationEntityMapper.Map(
                    _surveyAssignmentsQueriesService.GetFromSurveyId( surveyId ) ) );
            } )
            .ReturnResult();
    }

    /// <summary>
    /// Answer to a Survey
    /// </summary>
    /// <param name="surveyId">Id of the Survey</param>
    /// <param name="assegnationId">Id of the Survey Assignation</param>
    /// <param name="request">Request params to set the answer</param>
    /// <returns>Assignment of Survey</returns>
    [HttpPost]
    [Route( "{surveyId:guid}/{assegnationId:guid}/compile" )]
    [Authorize( Policy = Policies.SurveysAnswerReadWrite )]
    [SwaggerResponse( (int)HttpStatusCode.OK )]
    [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
    [SwaggerResponse( (int)HttpStatusCode.BadRequest, Type = typeof( ErrorModel ) )]
    public IActionResult SetCompiledSurvey(
        Guid surveyId, Guid assegnationId, SurveyCompileRequest request ) {
        Survey survey = null;
        Guid userId = GetCurrentUser().Id;

        return RulesHelper
            .IfSurveyIsValid( surveyId, out survey )
            .IfAssignationIsOwnedByPatient( userId, assegnationId )
            .Then( () => {
                try {
                    _surveyAnswerToQuestionEditorService
                        .SetCompiledSurveyFromPatient( assegnationId, request );

                    SaveChanges();

                    return Ok();
                }
                catch ( Exception ex ) {
                    return BadRequest( ex.Message );
                }
            } )
            .ReturnResult();
    }

    /// <summary>
    /// Get the compiled survey from the user
    /// </summary>
    /// <param name="assegnationId">Id of instance Survey assigned</param>
    /// <returns>Assignment of Survey</returns>
    [HttpGet]
    [Route( "{assegnationId:guid}/compiled" )]
    [Authorize( Policy = Policies.SurveysRead )]
    [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( SurveyCompiledModel ) )]
    [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
    public IActionResult GetCompiledSurveyFromPatient( Guid assegnationId ) {
        var currentRole = GetCurrentUserRoles();

        if ( currentRole.HasRoleOf( Roles.Patient ) ) {
            SurveysAssignationRelation assignmentRelation = null;

            return RulesHelper
                .IfSurveyAssignationIsValid( assegnationId, out assignmentRelation )
                .IfSurveyIsAssignedToUser( assegnationId, GetCurrentUser().Id )
                .Then( () => {
                    return Ok( _surveyAnswerToQuestionEditorService
                        .GetCompiledSurveyFromPatient( assegnationId ) );
                } )
                .ReturnResult();
        }
        else if ( currentRole.HasRoleOf( Roles.MedicalProfessional )
                || currentRole.HasRoleOf( Roles.MedicalTeamAdmin )
                || currentRole.HasRoleOf( Roles.Nurse )
                || currentRole.HasRoleOf( Roles.Researcher ) ) {
            SurveysAssignationRelation assignmentRelation = null;
            Patient patient = null;

            return RulesHelper
                .IfSurveyAssignationIsValid( assegnationId, out assignmentRelation )
                .IfPatientIsValid( assignmentRelation.UserId, out patient )
                .IfMedicAndPatientAreIntoSameMedicalTeam(
                    GetCurrentUser().Id, patient.UserId, GetCurrentUserRoles() )
                .Then( () => {
                    return Ok( _surveyAnswerToQuestionEditorService
                        .GetCompiledSurveyFromPatient( assegnationId ) );
                } )
                .ReturnResult();
        }

        return Forbid();
    }

    /// <summary>
    /// Get the compiled survey from the user
    /// </summary>
    /// <param name="assegnationId">Id of instance Survey assigned</param>
    /// <returns>Assignment of Survey</returns>
    [HttpGet]
    [Route( "{assegnationId:guid}/compiled/me" )]
    [Authorize( Policy = Policies.SurveysAnswerReadWrite )]
    [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( SurveyCompiledModel ) )]
    [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
    public IActionResult GetMyCompiledSurveyFromPatient( Guid assegnationId ) {
        SurveysAssignationRelation assignmentRelation = null;

        return RulesHelper
            .IfSurveyAssignationIsValid( assegnationId, out assignmentRelation )
            .IfSurveyIsAssignedToUser( assegnationId, GetCurrentUser().Id )
            .Then( () => {
                var compiledSurvey = _surveyAnswerToQuestionEditorService
                    .GetCompiledSurveyFromPatient( assegnationId );

                SaveChanges();

                return Ok( compiledSurvey );
            } )
            .ReturnResult();
    }
}