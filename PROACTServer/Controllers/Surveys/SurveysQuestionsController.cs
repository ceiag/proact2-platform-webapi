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
    public class SurveysQuestionsController : ProactBaseController {
        private readonly ISurveyQuestionsEditorService _surveyQuestionsEditorService;

        public SurveysQuestionsController( 
            IChangesTrackingService changesTrackingService, 
            ConsistencyRulesHelper consistencyRulesHelper,
            ISurveyQuestionsEditorService surveyQuestionsEditorService ) 
            : base( changesTrackingService, consistencyRulesHelper ) {
            _surveyQuestionsEditorService = surveyQuestionsEditorService;
        }

        /// <summary>
        /// Create a new Rating Question within a Questions Set
        /// </summary>
        /// <param name="questionsSetId">Id of Questions Set</param>
        /// <param name="request">Creation request params</param>
        /// <returns>Question created</returns>
        [HttpPost]
        [Route( "{questionsSetId:guid}/Rating" )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( SurveyQuestionModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.BadRequest, Type = typeof( ErrorModel ) )]
        public IActionResult CreateRatingQuestion( 
            Guid questionsSetId, RatingQuestionCreationRequest request ) {
            SurveyQuestionsSet questionsSet = null;

            return RulesHelper
                .IfSurveyQuestionsSetIsValid( questionsSetId, out questionsSet )
                .Then( () => {
                    var createdQuestion = _surveyQuestionsEditorService
                        .CreateRatingQuestion( request, questionsSet );

                    SaveChanges();

                    return Ok( createdQuestion );
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Edit Rating Question within a Questions Set
        /// </summary>
        /// <param name="questionsSetId">Id of Questions Set</param>
        /// <param name="request">Creation request params</param>
        /// <returns>Question created</returns>
        [HttpPut]
        [Route( "{questionsSetId:guid}/Rating" )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( SurveyQuestionModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.BadRequest, Type = typeof( ErrorModel ) )]
        public IActionResult EditRatingQuestion( Guid questionsSetId, RatingQuestionEditRequest request ) {
            SurveyQuestionsSet questionsSet = null;

            return RulesHelper
                .IfSurveyQuestionsSetIsValid( questionsSetId, out questionsSet )
                .IfSurveyQuestionsSetIsEditable( questionsSet )
                .Then( () => {
                    var editedQuestion = _surveyQuestionsEditorService
                        .EditRationQuestion( request, questionsSet );

                    SaveChanges();

                    return Ok( editedQuestion );
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Create a new Open Question within a Questions Set
        /// </summary>
        /// <param name="questionsSetId">Id of Questions Set</param>
        /// <param name="request">Creation request params</param>
        /// <returns>Question edited</returns>
        [HttpPost]
        [Route( "{questionsSetId:guid}/Open" )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( SurveyQuestionModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.BadRequest, Type = typeof( ErrorModel ) )]
        public IActionResult CreateOpenQuestion( Guid questionsSetId, OpenQuestionCreationRequest request ) {
            SurveyQuestionsSet questionsSet = null;

            return RulesHelper
                .IfSurveyQuestionsSetIsValid( questionsSetId, out questionsSet )
                .Then( () => {
                    var createdQuestion = _surveyQuestionsEditorService
                        .CreateOpenQuestion( request, questionsSet );

                    SaveChanges();

                    return Ok( createdQuestion );
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Edit Open Question within a Questions Set
        /// </summary>
        /// <param name="questionsSetId">Id of Questions Set</param>
        /// <param name="request">Creation request params</param>
        /// <returns>Question edited</returns>
        [HttpPut]
        [Route( "{questionsSetId:guid}/Open" )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( SurveyQuestionModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.BadRequest, Type = typeof( ErrorModel ) )]
        public IActionResult EditOpenQuestion( Guid questionsSetId, OpenQuestionEditRequest request ) {
            SurveyQuestionsSet questionsSet = null;

            return RulesHelper
                .IfSurveyQuestionsSetIsValid( questionsSetId, out questionsSet )
                .IfSurveyQuestionsSetIsEditable( questionsSet )
                .Then( () => {
                    var editedQuestion = _surveyQuestionsEditorService
                        .EditOpenQuestion( request, questionsSet );

                    SaveChanges();

                    return Ok( editedQuestion );
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Create a new Single Choice Question within a Questions Set
        /// </summary>
        /// <param name="questionsSetId">Id of Questions Set</param>
        /// <param name="request">Creation request params</param>
        /// <returns>Question created</returns>
        [HttpPost]
        [Route( "{questionsSetId:guid}/SingleChoice" )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( SurveyQuestionModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.BadRequest, Type = typeof( ErrorModel ) )]
        public IActionResult CreateSingleChoiceQuestion( Guid questionsSetId, SingleChoiceCreationRequest request ) {
            SurveyQuestionsSet questionsSet = null;

            return RulesHelper
                .IfSurveyQuestionsSetIsValid( questionsSetId, out questionsSet )
                .Then( () => {
                    var createdQuestion = _surveyQuestionsEditorService
                        .CreateSingleChoiceQuestion( request, questionsSet );

                    SaveChanges();

                    return Ok( createdQuestion );
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Edit Single Choice Question within a Questions Set
        /// </summary>
        /// <param name="questionsSetId">Id of Questions Set</param>
        /// <param name="request">Creation request params</param>
        /// <returns>Question created</returns>
        [HttpPut]
        [Route( "{questionsSetId:guid}/SingleChoice" )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( SurveyQuestionModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.BadRequest, Type = typeof( ErrorModel ) )]
        public IActionResult EditSingleChoiceQuestion( Guid questionsSetId, SingleChoiceEditRequest request ) {
            SurveyQuestionsSet questionsSet = null;

            return RulesHelper
                .IfSurveyQuestionsSetIsValid( questionsSetId, out questionsSet )
                .IfSurveyQuestionsSetIsEditable( questionsSet )
                .Then( () => {
                    var editedQuestion = _surveyQuestionsEditorService
                        .EditSingleChoiceQuestion( request, questionsSet );

                    SaveChanges();

                    return Ok( editedQuestion );
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Create a new Multiple Choice Question within a Questions Set
        /// </summary>
        /// <param name="questionsSetId">Id of Questions Set</param>
        /// <param name="request">Creation request params</param>
        /// <returns>Question edited</returns>
        [HttpPost]
        [Route( "{questionsSetId:guid}/MultipleChoice" )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( SurveyQuestionModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.BadRequest, Type = typeof( ErrorModel ) )]
        public IActionResult CreateMultipleChoiceQuestion(
           Guid questionsSetId, MultipleChoiceCreationRequest request ) {
            SurveyQuestionsSet questionsSet = null;

            return RulesHelper
                .IfSurveyQuestionsSetIsValid( questionsSetId, out questionsSet )
                .Then( () => {
                    var createdQuestion = _surveyQuestionsEditorService
                        .CreateMultipleChoiceQuestion( request, questionsSet );

                    SaveChanges();

                    return Ok( createdQuestion );
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Edit Multiple Choice Question within a Questions Set
        /// </summary>
        /// <param name="questionsSetId">Id of Questions Set</param>
        /// <param name="request">Creation request params</param>
        /// <returns>Question edited</returns>
        [HttpPut]
        [Route( "{questionsSetId:guid}/MultipleChoice" )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( SurveyQuestionModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.BadRequest, Type = typeof( ErrorModel ) )]
        public IActionResult EditedMultipleChoiceQuestion( 
            Guid questionsSetId, MultipleChoiceEditRequest request ) {
            SurveyQuestionsSet questionsSet = null;

            return RulesHelper
                .IfSurveyQuestionsSetIsValid( questionsSetId, out questionsSet )
                .IfSurveyQuestionsSetIsEditable( questionsSet )
                .Then( () => {
                    var editedQuestion = _surveyQuestionsEditorService
                        .EditMultipleChoiceQuestion( request, questionsSet );

                    SaveChanges();

                    return Ok( editedQuestion );
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Create a new Bool Question within a Questions Set
        /// </summary>
        /// <param name="questionsSetId">Id of Questions Set</param>
        /// <param name="request">Creation request params</param>
        /// <returns>Question created</returns>
        [HttpPost]
        [Route( "{questionsSetId:guid}/Bool" )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( SurveyQuestionModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.BadRequest, Type = typeof( ErrorModel ) )]
        public IActionResult CreateBoolQuestion( 
            Guid questionsSetId, BoolQuestionCreationRequest request ) {
            SurveyQuestionsSet questionsSet = null;

            return RulesHelper
                .IfSurveyQuestionsSetIsValid( questionsSetId, out questionsSet )
                .Then( () => {
                    var createdQuestion = _surveyQuestionsEditorService
                        .CreateBoolQuestion( request, questionsSet );

                    SaveChanges();

                    return Ok( createdQuestion );
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Edit a Bool Question within a Questions Set
        /// </summary>
        /// <param name="questionsSetId">Id of Questions Set</param>
        /// <param name="request">Creation request params</param>
        /// <returns>Question created</returns>
        [HttpPut]
        [Route( "{questionsSetId:guid}/Bool" )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( SurveyQuestionModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.BadRequest, Type = typeof( ErrorModel ) )]
        public IActionResult EditBoolQuestion( Guid questionsSetId, BoolQuestionEditRequest request ) {
            SurveyQuestionsSet questionsSet = null;

            return RulesHelper
                .IfSurveyQuestionsSetIsValid( questionsSetId, out questionsSet )
                .IfSurveyQuestionsSetIsEditable( questionsSet )
                .Then( () => {
                    var editedQuestion = _surveyQuestionsEditorService
                        .EditBoolQuestion( request, questionsSet );

                    SaveChanges();

                    return Ok( editedQuestion );
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Create a new Mood Question within a Questions Set
        /// </summary>
        /// <param name="questionsSetId">Id of Questions Set</param>
        /// <param name="request">Creation request params</param>
        /// <returns>Question created</returns>
        [HttpPost]
        [Route( "{questionsSetId:guid}/Mood" )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( SurveyQuestionModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.BadRequest, Type = typeof( ErrorModel ) )]
        public IActionResult CreateMoodQuestion( Guid questionsSetId, MoodQuestionCreationRequest request ) {
            SurveyQuestionsSet questionsSet = null;

            return RulesHelper
                .IfSurveyQuestionsSetIsValid( questionsSetId, out questionsSet )
                .Then( () => {
                    var createdQuestion = _surveyQuestionsEditorService
                        .CreateMoodQuestion( request, questionsSet );

                    SaveChanges();

                    return Ok( createdQuestion );
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Edit a Bool Question within a Questions Set
        /// </summary>
        /// <param name="questionsSetId">Id of Questions Set</param>
        /// <param name="request">Creation request params</param>
        /// <returns>Question created</returns>
        [HttpPut]
        [Route( "{questionsSetId:guid}/Mood" )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( SurveyQuestionModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.BadRequest, Type = typeof( ErrorModel ) )]
        public IActionResult EditMoodQuestion( Guid questionsSetId, MoodQuestionEditRequest request ) {
            SurveyQuestionsSet questionsSet = null;

            return RulesHelper
                .IfSurveyQuestionsSetIsValid( questionsSetId, out questionsSet )
                .IfSurveyQuestionsSetIsEditable( questionsSet )
                .Then( () => {
                    var editedQustion = _surveyQuestionsEditorService
                        .EditMoodQuestion( request, questionsSet );

                    SaveChanges();

                    return Ok( editedQustion );
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Delete Questions into questions set
        /// </summary>
        /// <param name="questionsSetId">Id of questionSet</param>
        /// <param name="questionId">Id of question</param>
        /// <returns></returns>
        [HttpDelete]
        [Route( "{questionsSetId:guid}/{questionId:guid}" )]
        [SwaggerResponse( (int)HttpStatusCode.OK )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.BadRequest, Type = typeof( ErrorModel ) )]
        public IActionResult DeleteQuestionsSet( Guid questionsSetId, Guid questionId ) {
            SurveyQuestionsSet questionsSet = null;
            SurveyQuestion question = null;

            return RulesHelper
                .IfSurveyQuestionsSetIsValid( questionsSetId, out questionsSet )
                .IfSurveyQuestionsSetIsEditable( questionsSet )
                .IfSurveyQuestionIsValid( questionId, out question )
                .Then( () => {
                    _surveyQuestionsEditorService.DeleteQuestion( questionId );

                    SaveChanges();

                    return Ok();
                } )
                .ReturnResult();
        }
    }
}
