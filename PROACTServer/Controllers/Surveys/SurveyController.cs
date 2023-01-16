using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proact.Services.AuthorizationPolicies;
using Proact.Services.Entities;
using Proact.Services.Models;
using Proact.Services.Models.SurveyStats;
using Proact.Services.QueriesServices;
using Proact.Services.QueriesServices.Surveys.Stats;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Net;

namespace Proact.Services.Controllers.Surveys {
    [ApiController]
    [Route( ProactRouteConfiguration.DefaultRoute )]
    public class SurveyController : ProactBaseController {
        private readonly ISurveyQueriesService _surveyQueriesService;
        private readonly ISurveysStatsQueriesService _suveysStatsQueriesService;
        private readonly ISurveyStatsOverTimeQueriesService _surveyStatsOverTimeQueriesService;

        public SurveyController(
            IChangesTrackingService changesTrackingService,
            ConsistencyRulesHelper consistencyRulesHelper,
            ISurveyQueriesService surveyQueriesService,
            ISurveysStatsQueriesService suveysStatsQueriesService,
            ISurveyStatsOverTimeQueriesService surveyStatsOverTimeQueriesService )
            : base( changesTrackingService, consistencyRulesHelper ) {
            _surveyQueriesService = surveyQueriesService;
            _suveysStatsQueriesService = suveysStatsQueriesService;
            _surveyStatsOverTimeQueriesService = surveyStatsOverTimeQueriesService;
        }

        /// <summary>
        /// Create a new Survey
        /// </summary>
        /// <param name="projectId">Project Identifier</param>
        /// <param name="request">Creation request params</param>
        /// <returns>Survey created</returns>
        [HttpPost]
        [Route( "{projectId:guid}" )]
        [Authorize( Policy = Policies.SurveysWrite )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( SurveyModel ) )]
        public IActionResult CreateSurvey( Guid projectId, SurveyCreationRequest request ) {
            Project project = null;

            return RulesHelper
                .IfProjectIsValid( projectId, out project )
                .IfUserIsInProject( GetCurrentUser().Id, project.Id )
                .IfSurveyQuestionsSetIsPublished( request.QuestionsSetId )
                .Then( () => {
                    var surveyCreated = _surveyQueriesService.Create( projectId, request );

                    SaveChanges();

                    var addedQuestions = _surveyQueriesService
                        .AddQuestions( surveyCreated.Id, request.QuestionsIds );

                    SaveChanges();

                    return Ok( SurveysEntityMapper.Map( surveyCreated ) );
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Edit Survey 
        /// </summary>
        /// <param name="surveyId">Suvey idetifier</param>
        /// <param name="request">Body of request</param>
        /// <returns>Survey Model</returns>
        [HttpPut]
        [Authorize( Policy = Policies.SurveysWrite )]
        [SwaggerResponse( (int)HttpStatusCode.OK )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        public IActionResult EditSurvey( Guid surveyId, SurveyEditRequest request ) {
            Survey survey = null;

            return RulesHelper
                .IfSurveyIsValid( surveyId, out survey )
                .IfUserIsInProject( GetCurrentUser().Id, survey.ProjectId )
                .IfSurveyIsEditable( surveyId )
                .Then( () => {
                    _surveyQueriesService.Update( surveyId, request );
                    SaveChanges();

                    return Ok();
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Delete Survey by id
        /// </summary>
        /// <returns>Deleted Survey</returns>
        [HttpDelete]
        [Route( "{surveyId:guid}" )]
        [Authorize( Policy = Policies.SurveysWrite )]
        [SwaggerResponse( (int)HttpStatusCode.OK )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.BadRequest, Type = typeof( ErrorModel ) )]
        public IActionResult DeleteSurvey( Guid surveyId ) {
            Survey survey = null;

            return RulesHelper
                .IfSurveyIsValid( surveyId, out survey )
                .IfUserIsInProject( GetCurrentUser().Id, survey.ProjectId )
                .IfSurveyIsEditable( surveyId )
                .Then( () => {
                    var deletedSurvey = _surveyQueriesService.Delete( surveyId );

                    SaveChanges();

                    return Ok();
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Get Surveys
        /// </summary>
        /// <param name="projectId">Project Identifier</param> 
        /// <returns>Surveys created</returns>
        [HttpGet]
        [Route( "{projectId:guid}/all" )]
        [Authorize( Policy = Policies.SurveysRead )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( SurveyModel[] ) )]
        public IActionResult GetSurveys( Guid projectId ) {
            Project project = null;

            return RulesHelper
                .IfProjectIsValid( projectId, out project )
                .IfUserIsInProject( GetCurrentUser().Id, projectId )
                .Then( () => {
                    return Ok(
                        _suveysStatsQueriesService
                            .GetAllSurveysWithAssignedPatients( projectId ) );
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Get Survey by id
        /// </summary>
        /// <param name="surveyId">Survey Identifier</param> 
        /// <returns>Survey requested</returns>
        [HttpGet]
        [Route( "{surveyId:guid}" )]
        [Authorize( Policy = Policies.SurveysRead )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( SurveyModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        public IActionResult GetSurvey( Guid surveyId ) {
            Survey survey = null;

            return RulesHelper
                .IfSurveyIsValid( surveyId, out survey )
                .IfUserIsInProject( GetCurrentUser().Id, survey.ProjectId )
                .Then( () => {
                    return Ok( SurveysEntityMapper.Map( _surveyQueriesService.Get( surveyId ) ) );
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Get the stats for a survey
        /// </summary>
        /// <param name="surveyId">Id of the Surveys</param>
        /// <returns>Stats of Survey</returns>
        [HttpGet]
        [Route( "{surveyId:guid}/stats" )]
        [Authorize( Policy = Policies.SurveysRead )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( SurveyStatsResume ) )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        public IActionResult GetSurveyStats( Guid surveyId ) {
            Survey survey = null;

            return RulesHelper
                .IfSurveyIsValid( surveyId, out survey )
                .Then( () => {
                    var surveyStats = _suveysStatsQueriesService
                        .GetStatsResumeForSurvey( surveyId );
                    return Ok( surveyStats );
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Get the stats over the time about a patient
        /// </summary>
        /// <param name="surveyId">Id of the Surveys</param>
        /// <param name="userId">Id of the User</param>
        /// <returns>Stats of Survey</returns>
        [HttpGet]
        [Route( "{surveyId:guid}/user/{userId:guid}/stats/" )]
        [Authorize( Policy = Policies.SurveysRead )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( SurveyStatsResumeByTime ) )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        public IActionResult GetSurveyStatsOverTimeForPatient( Guid surveyId, Guid userId ) {
            Survey survey = null;

            return RulesHelper
                .IfSurveyIsValid( surveyId, out survey )
                .Then( () => {
                    var surveyStats = _surveyStatsOverTimeQueriesService
                        .Get( surveyId, userId );

                    return Ok( surveyStats );
                } )
                .ReturnResult();
        }
    }
}
