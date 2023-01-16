using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proact.Services.AuthorizationPolicies;
using Proact.Services.Entities;
using Proact.Services.Models;
using Proact.Services.QueriesServices;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Net;

namespace Proact.Services.Controllers.Surveys {
    [ApiController]
    [Route( ProactRouteConfiguration.DefaultRoute )]
    [Authorize( Policy = Policies.SurveysReadWrite )]
    public class SurveyAnswersBlocksController : ProactBaseController {
        private readonly ISurveyAnswersQueriesService _surveyAnswersQueriesService;
        private readonly ISurveyAnswersBlockQueriesService _surveyAnswersBlockQueriesService;

        public SurveyAnswersBlocksController( 
            IChangesTrackingService changesTrackingService,
            ISurveyAnswersQueriesService surveyAnswersQueriesService,
            ISurveyAnswersBlockQueriesService surveyAnswersBlockQueriesService,
            ConsistencyRulesHelper consistencyRulesHelper ) 
            : base( changesTrackingService, consistencyRulesHelper ) {
            _surveyAnswersQueriesService = surveyAnswersQueriesService;
            _surveyAnswersBlockQueriesService = surveyAnswersBlockQueriesService;
        }

        /// <summary>
        /// Create a new block of answers
        /// </summary>
        /// <param name="projectId">Project Identifier</param>
        /// <param name="request">Creation request params</param>
        /// <returns>Answer created</returns>
        [HttpPost]
        [Route("{projectId:guid}")]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( SurveyAnswersBlockModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.BadRequest, Type = typeof( ErrorModel ) )]
        public IActionResult CreateAnswerBlock( Guid projectId, AnswersBlockCreationRequest request ) {
            Project project = null;

            return RulesHelper
                .IfProjectIsValid( projectId, out project )
                .IfUserIsInProject( GetCurrentUser().Id,  project.Id )
                .Then( () => {
                    var answersBlock = _surveyAnswersBlockQueriesService.Create( projectId, request );

                    SaveChanges();

                    return Ok( SurveyAnswersEntityMapper.Map( answersBlock ) );
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Get list of answers blocks within a project
        /// </summary>
        /// <param name="projectId">Project Identifier</param>
        /// <returns>Answer created</returns>
        [HttpGet]
        [Route( "{projectId:guid}/all" )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( SurveyAnswersBlockModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.BadRequest, Type = typeof( ErrorModel ) )]
        public IActionResult GetAnswersBlocks( Guid projectId ) {
            Project project = null;

            return RulesHelper
                .IfProjectIsValid( projectId, out project )
                .IfUserIsInProject( GetCurrentUser().Id, projectId )
                .Then( () => {
                    return Ok( SurveyAnswersEntityMapper.Map(
                        _surveyAnswersBlockQueriesService.GetsAll( projectId ) ) );
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Get single answer block
        /// </summary>
        /// <param name="answersBlockId">Id of Answers Block</param>
        /// <returns>Answer created</returns>
        [HttpGet]
        [Route( "{answersBlockId:guid}" )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( SurveyAnswersBlockModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.BadRequest, Type = typeof( ErrorModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        public IActionResult GetAnswersBlock( Guid answersBlockId ) {
            SurveyAnswersBlock answersBlock = null;

            return RulesHelper
                .IfAnswersBlockIsValid( answersBlockId, out answersBlock )
                .IfUserIsInProject( GetCurrentUser().Id, answersBlock.ProjectId )
                .Then( () => {
                    return Ok( SurveyAnswersEntityMapper.Map( 
                        _surveyAnswersQueriesService.Get( answersBlockId ) ) );
                } )
                .ReturnResult();
        }
    }
}
