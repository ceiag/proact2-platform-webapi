using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proact.Services.AuthorizationPolicies;
using Proact.Services.Entities;
using Proact.Services.Models;
using Proact.Services.QueriesServices;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Net;

namespace Proact.Services.Controllers {
    [ApiController]
    [Route( ProactRouteConfiguration.DefaultRoute )]
    [Authorize( Policy = Policies.SurveysReadWrite )]
    public class SurveysQuestionsSetsController : ProactBaseController {
        private readonly ISurveyQuestionsSetQueriesService _surveyQuestionsSetQueriesService;

        public SurveysQuestionsSetsController( 
            IChangesTrackingService changesTrackingService, 
            ConsistencyRulesHelper consistencyRulesHelper,
            ISurveyQuestionsSetQueriesService surveyQuestionsSetQueriesService ) 
            : base( changesTrackingService, consistencyRulesHelper ) {
            _surveyQuestionsSetQueriesService = surveyQuestionsSetQueriesService;
        }

        /// <summary>
        /// Create a new Questions Set
        /// </summary>
        /// <param name="projectId">Project Identifier</param>
        /// <param name="request">Questions Set Properties</param>
        /// <returns>The Questions Set Container</returns>
        [HttpPost]
        [Route( "{projectId:guid}" )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( SurveyQuestionsSetModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.Conflict, Type = typeof( ErrorModel ) )]
        public IActionResult CreateQuestionsSet( Guid projectId, QuestionsSetCreationRequest request ) {
            Project project = null;
            var currentUser = GetCurrentUser();

            return RulesHelper
                .IfProjectIsValid( projectId, out project )
                .IfUserIsInProject( currentUser.Id, project.Id )
                .Then( () => {
                    var createdQuestionsSet = _surveyQuestionsSetQueriesService.Create( projectId, request );

                    SaveChanges();

                    return Ok( SurveysEntityMapper.Map( createdQuestionsSet ) );
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Get Questions Sets List
        /// </summary>
        /// <param name="projectId">Project Identifier</param>
        /// <returns>The list of Questions Sets</returns>
        [HttpGet]
        [Route( "{projectId:guid}/all" )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( SurveyQuestionsSetModel[] ) )]
        public IActionResult GetQuestionsSets( Guid projectId ) {
            Project project = null;

            return RulesHelper
                .IfProjectIsValid( projectId, out project )
                .IfUserIsInProject( GetCurrentUser().Id, project.Id )
                .Then( () => {
                    return Ok( SurveysEntityMapper.Map( 
                        _surveyQuestionsSetQueriesService.GetsAll( projectId ) ) );
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Get Questions Set Details
        /// </summary>
        /// <param name="questionsSetId">df of questions set</param>
        /// <returns>The list of medics</returns>
        [HttpGet]
        [Route( "{questionsSetId:guid}" )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( SurveyQuestionsSetModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.Conflict, Type = typeof( ErrorModel ) )]
        public IActionResult GetQuestionsSet( Guid questionsSetId ) {
            SurveyQuestionsSet questionsSet = null;
            
            return RulesHelper
                .IfSurveyQuestionsSetIsValid( questionsSetId, out questionsSet )
                .IfUserIsInProject( GetCurrentUser().Id, questionsSet.ProjectId )
                .Then( () => {
                    return Ok( SurveysEntityMapper.Map(
                        _surveyQuestionsSetQueriesService.Get( questionsSetId ) ) );
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Edit Questions Set
        /// </summary>
        /// <param name="request">Questions Set Properties</param>
        /// <param name="questionsSetId">Id of Questions Set</param>
        /// <returns></returns>
        [HttpPut]
        [Route( "{questionsSetId:guid}" )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( SurveyQuestionsSetModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        public IActionResult EditQuestionsSet( Guid questionsSetId, QuestionsSetEditRequest request ) {
            SurveyQuestionsSet questionsSet = null;

            return RulesHelper
                .IfSurveyQuestionsSetIsValid( questionsSetId, out questionsSet )
                .IfUserIsInProject( GetCurrentUser().Id, questionsSet.ProjectId )
                .IfSurveyQuestionsSetIsEditable( questionsSet )
                .Then( () => {
                    var questionsSetDeleted = _surveyQuestionsSetQueriesService
                        .Edit( questionsSetId, request );

                    SaveChanges();

                    return Ok();
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Delete Questions Set
        /// </summary>
        /// <param name="questionsSetId">If of questions set</param>
        /// <returns></returns>
        [HttpDelete]
        [Route( "{questionsSetId:guid}" )]
        [SwaggerResponse( (int)HttpStatusCode.OK )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        public IActionResult DeleteQuestionsSet( Guid questionsSetId ) {
            SurveyQuestionsSet questionsSet = null;

            return RulesHelper
                .IfSurveyQuestionsSetIsValid( questionsSetId, out questionsSet )
                .IfUserIsInProject( GetCurrentUser().Id, questionsSet.ProjectId )
                .IfSurveyQuestionsSetIsEditable( questionsSet )
                .Then( () => {
                    _surveyQuestionsSetQueriesService.Delete( questionsSetId );

                    SaveChanges();

                    return Ok();
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Set Questions Set as approved
        /// </summary>
        /// <param name="questionsSetId">Questions Set Identifier</param>
        /// <returns>200 Ok</returns>
        [HttpPost]
        [Route( "{questionsSetId:guid}/publish" )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( SurveyQuestionsSetModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.Conflict, Type = typeof( ErrorModel ) )]
        public IActionResult PublishQuestionsSet( Guid questionsSetId ) {
            SurveyQuestionsSet questionsSet = null;

            return RulesHelper
                .IfSurveyQuestionsSetIsValid( questionsSetId, out questionsSet )
                .IfUserIsInProject( GetCurrentUser().Id, questionsSet.ProjectId )
                .IfSurveyQuestionsSetIsEditable( questionsSet )
                .Then( () => {
                    _surveyQuestionsSetQueriesService.SetState(
                        questionsSetId, QuestionsSetsState.PUBLISHED );

                    SaveChanges();

                    return Ok();
                } )
                .ReturnResult();
        }
    }
}
