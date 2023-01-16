using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Proact.Services.AuthorizationPolicies;
using Proact.Services.Entities;
using Proact.Services.Messages;
using Proact.Services.Models;
using Proact.Services.Models.Messages;
using Proact.Services.PushNotifications;
using Proact.Services.QueriesServices;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Proact.Services.Controllers {
    [ApiController]
    [Route( ProactRouteConfiguration.DefaultRoute )]
    public class MessagesController : ProactBaseController {
        private readonly int _repliesLimitCount = 3;
        private readonly int _searchingMessagesMaxRows = 20;
        private readonly ILogger<MessagesController> _logger;
        private readonly IMessageNotifierService _messageNotifierService;
        private readonly IMessageFormatterService _messageFormatterService;
        private readonly IMessageEditorService _messageEditorService;
        private readonly IOrganizedMessagesProvider _organizedMessageProvider;

        public MessagesController(
            IChangesTrackingService changesTrackingService, ILogger<MessagesController> logger,
            IMessageNotifierService messageNotifierService, IMessageFormatterService messageFormatterService,
            IMessageEditorService messageEditorService, IOrganizedMessagesProvider organizedMessageProvider,
            ConsistencyRulesHelper consistencyRulesHelper )
            : base( changesTrackingService, consistencyRulesHelper ) {
            _logger = logger;
            _messageNotifierService = messageNotifierService;
            _messageFormatterService = messageFormatterService;
            _messageEditorService = messageEditorService;
            _organizedMessageProvider = organizedMessageProvider;
        }

        /// <summary>
        /// Get a message of current user related to specified project
        /// </summary>
        /// <param name="projectId">Project identifier</param>
        /// <param name="medicalTeamId">Medical team identifier</param>
        /// <param name="messageId">MessageId to retrieve</param>
        /// <returns>Message</returns>
        [HttpGet]
        [Authorize( Policy = Policies.MessagesPatientRead )]
        [Route( "{projectId:guid}/{medicalTeamId:guid}/{messageId:guid}" )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( BranchedMessagesModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.Unauthorized, Type = typeof( ErrorModel ) )]
        public IActionResult GetMessage(
            Guid projectId, Guid medicalTeamId, Guid messageId ) {
            MedicalTeam medicalTeam = null;
            Project project = null;
            Message message = null;

            var currentUser = GetCurrentUser();

            return RulesHelper
                .IfProjectIsValid( projectId, out project )
                .IfMedicalTeamIsValid( medicalTeamId, out medicalTeam )
                .IfMessageIsValid( messageId, out message )
                .IfMessageIsInMyInstitute( GetCurrentInstitute().Id, message )
                .Then( () => {
                    BranchedMessagesModel branchedMessagesModel = null;

                    if ( HasRoleOf( Roles.Patient ) ) {
                        branchedMessagesModel = _messageFormatterService
                            .GetMessageAsPatient( messageId, int.MaxValue );

                        if ( branchedMessagesModel.OriginalMessage.AuthorId != currentUser.Id ) {
                            return Unauthorized( "You can retrieve only your message" );
                        }
                    }
                    else {
                        branchedMessagesModel = _messageFormatterService
                            .GetMessageAsMedic( messageId, int.MaxValue );
                    }

                    return Ok( branchedMessagesModel );
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Get all messages of current user related to specified project without a reply
        /// </summary>
        /// <param name="projectId">Project identifier</param>
        /// <param name="medicalTeamId">Medical team identifier</param>
        /// <param name="pagingCount">Paging Count</param>
        /// <returns>Messages list</returns>
        [HttpGet]
        [Authorize( Policy = Policies.MessagesPatientRead )]
        [Route( "{projectId:guid}/{medicalTeamId:guid}/unreplied/{pagingCount:int}" )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( BranchedMessagesModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.Unauthorized, Type = typeof( ErrorModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.Forbidden, Type = typeof( ErrorModel ) )]
        public IActionResult GetUnrepliedMessages( Guid projectId, Guid medicalTeamId, int pagingCount ) {
            MedicalTeam medicalTeam = null;
            Project project = null;

            var currentUser = GetCurrentUser();

            return RulesHelper
                .IfProjectIsValid( projectId, out project )
                .IfMedicalTeamIsValid( medicalTeamId, out medicalTeam )
                .IfProfessionistIsIntoTheMedicalTeam( currentUser.Id, medicalTeamId, GetCurrentUserRoles() )
                .IfMedicalTeamIsInMyInstitute( GetCurrentInstitute().Id, medicalTeam )
                .Then( () => {
                    if ( HasRoleOf( Roles.MedicalProfessional ) ) {
                        var branchedMessages = _messageFormatterService.GetMessagesAsMedicUnreplied(
                            medicalTeamId, pagingCount, _repliesLimitCount );

                        return Ok( branchedMessages );
                    }
                    else if ( HasRoleOf( Roles.Researcher ) ) {
                        var branchedMessages = _messageFormatterService.GetMessagesAsResearcher(
                            medicalTeamId, pagingCount, _repliesLimitCount );

                        return Ok( branchedMessages );
                    }

                    return Unauthorized();
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Get all messages from a patient
        /// </summary>
        /// <param name="projectId">Project identifier</param>
        /// <param name="medicalTeamId">Medical team identifier</param>
        /// <param name="userId">Id of the patient</param>
        /// <param name="pagingCount">Paging Count</param>
        /// <returns>Messages list</returns>
        [HttpGet]
        [Authorize( Policy = Policies.MessagesPatientRead )]
        [Route( "{projectId:guid}/{medicalTeamId:guid}/patient/{userId:guid}/{pagingCount:int}" )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( BranchedMessagesModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.Unauthorized, Type = typeof( ErrorModel ) )]
        public IActionResult GetMessagesListForPatient(
            Guid projectId, Guid medicalTeamId, Guid userId, int pagingCount ) {
            MedicalTeam medicalTeam = null;
            Patient patient = null;
            Project project = null;

            return RulesHelper
                .IfProjectIsValid( projectId, out project )
                .IfMedicalTeamIsValid( medicalTeamId, out medicalTeam )
                .IfPatientIsValid( userId, out patient )
                .IfProfessionistIsIntoTheMedicalTeam( GetCurrentUser().Id, medicalTeamId, GetCurrentUserRoles() )
                .IfMedicalTeamIsInMyInstitute( GetCurrentInstitute().Id, medicalTeam )
                .Then( () => {
                    var branchedMessages = _messageFormatterService
                        .GetMessagesAsPatient( patient, pagingCount, _repliesLimitCount );

                    return Ok( branchedMessages );
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Get all messages from a patient using search parameters
        /// </summary>
        /// <param name="projectId">Project identifier</param>
        /// <param name="medicalTeamId">Medical team identifier</param>
        /// <returns>Messages list</returns>
        [HttpGet]
        [Authorize( Policy = Policies.MessagesPatientRead )]
        [Route( "{projectId:guid}/{medicalTeamId:guid}/patient/search" )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( List<MessageModel> ) )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.Unauthorized, Type = typeof( ErrorModel ) )]
        public IActionResult SearchMessagesAsPatient( Guid projectId, Guid medicalTeamId ) {
            MedicalTeam medicalTeam = null;
            Patient patient = null;
            Project project = null;

            return RulesHelper
                .IfProjectIsValid( projectId, out project )
                .IfMedicalTeamIsValid( medicalTeamId, out medicalTeam )
                .IfPatientIsValid( GetCurrentUser().Id, out patient )
                .IfUserIsInMyInstitute( GetCurrentInstitute().Id, patient.User )
                .Then( () => {
                    var messages = _messageFormatterService.SearchMessagesAsPatient(
                        patient, HttpContext.Request.QueryString.Value, _searchingMessagesMaxRows );

                    return Ok( messages );
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Get all messages from a patient using search parameters
        /// </summary>
        /// <param name="projectId">Project identifier</param>
        /// <param name="medicalTeamId">Medical team identifier</param>
        /// <param name="message">Message to search</param>
        /// <param name="fromDate">Starting date</param>
        /// <param name="toDate">Ending date</param>
        /// <returns>Messages list</returns>
        [HttpGet]
        [Authorize( Policy = Policies.MessagesPatientRead )]
        [Route( "{projectId:guid}/{medicalTeamId:guid}/medic/search" )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( List<MessageModel> ) )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.Unauthorized, Type = typeof( ErrorModel ) )]
        public IActionResult SearchMessagesAsMedicalProfessionalWith( Guid projectId, Guid medicalTeamId ) {
            MedicalTeam medicalTeam = null;
            Project project = null;

            return RulesHelper
                .IfProjectIsValid( projectId, out project )
                .IfMedicalTeamIsValid( medicalTeamId, out medicalTeam )
                .IfProfessionistIsIntoTheMedicalTeam( GetCurrentUser().Id, medicalTeamId, GetCurrentUserRoles() )
                .IfMedicalTeamIsInMyInstitute( GetCurrentInstitute().Id, medicalTeam )
                .Then( () => {
                    if ( HasRoleOf( Roles.MedicalProfessional ) ) {
                        var messages = _messageFormatterService.SearchMessagesAsMedic(
                            medicalTeamId, HttpContext.Request.QueryString.Value, _searchingMessagesMaxRows );
                        return Ok( messages );
                    }
                    else if ( HasRoleOf( Roles.Researcher ) ) {
                        var messages = _messageFormatterService.SearchMessagesAsResearcher(
                            medicalTeamId, HttpContext.Request.QueryString.Value, _searchingMessagesMaxRows );
                        return Ok( messages );
                    }

                    return Unauthorized();
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Get all messages inside a project
        /// </summary>
        /// <param name="medicalTeamId">MedicalTeam identifier</param>
        /// <returns>Messages list</returns>
        [HttpGet]
        [Authorize( Policy = Policies.DataExporterRead )]
        [Route( "{medicalTeamId:guid}/allmessages" )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( List<BranchedMessagesModel> ) )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        public IActionResult GetMessages( Guid medicalTeamId ) {
            MedicalTeam medicalTeam = null;
            User currentUser = GetCurrentUser();

            return RulesHelper
                .IfMedicalTeamIsValid( medicalTeamId, out medicalTeam )
                .IfUserIsInMedicalTeam( currentUser.Id, medicalTeamId )
                .Then( () => {
                    List<BranchedMessagesModel> branchedMessages =
                        _messageFormatterService.GetAllMessagesForMedicalTeam( medicalTeamId );

                    return Ok( branchedMessages );
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Get all messages of current user related to specified project
        /// </summary>
        /// <param name="projectId">Project identifier</param>
        /// <param name="medicalTeamId">Medical team identifier</param>
        /// <param name="pagingCount">Paging Count</param>
        /// <returns>Messages list</returns>
        [HttpGet]
        [Authorize( Policy = Policies.MessagesMineRead )]
        [Route( "{projectId:guid}/{medicalTeamId:guid}/{pagingCount:int}" )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( List<BranchedMessagesModel> ) )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        public IActionResult GetMessages( Guid projectId, Guid medicalTeamId, int pagingCount ) {
            MedicalTeam medicalTeam = null;
            Project project = null;

            var currentUser = GetCurrentUser();

            return RulesHelper
                .IfProjectIsValid( projectId, out project )
                .IfMedicalTeamIsValid( medicalTeamId, out medicalTeam )
                .IfMedicalTeamIsInMyInstitute( GetCurrentInstitute().Id, medicalTeam )
                .Then( () => {
                    List<BranchedMessagesModel> branchedMessages = null;

                    if ( HasRoleOf( Roles.Patient ) ) {
                        Patient patient = RulesHelper
                            .GetQueriesService<IPatientQueriesService>().Get( currentUser.Id );

                        if ( patient == null ) {
                            return NotFound( $"patient with user id {currentUser.Id} not found!" );
                        }

                        branchedMessages = _messageFormatterService
                            .GetMessagesAsPatient( patient, pagingCount, _repliesLimitCount );
                    }
                    else if ( HasRoleOf( Roles.MedicalProfessional ) ) {
                        branchedMessages = _messageFormatterService
                            .GetMessagesAsMedic( medicalTeam, pagingCount, _repliesLimitCount );
                    }
                    else if ( HasRoleOf( Roles.Nurse ) ) {
                        branchedMessages = _messageFormatterService
                            .GetMessagesAsMedic( medicalTeam, pagingCount, _repliesLimitCount );
                    }
                    else if ( HasRoleOf( Roles.Researcher ) ) {
                        branchedMessages = _messageFormatterService
                            .GetMessagesAsResearcher( medicalTeam, pagingCount, _repliesLimitCount );
                    }
                    else {
                        return Unauthorized();
                    }

                    return Ok( branchedMessages );
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Create a new message within a project where medical team can reply
        /// </summary>
        /// <param name="projectId">Project identifier</param>
        /// <param name="medicalTeamId">Medical team identifier</param>
        /// <param name="messageData">Message information</param>
        /// <returns>Message information</returns>
        [HttpPost]
        [Authorize( Policy = Policies.MessagesNewTopicWrite )]
        [Route( "{projectId:guid}/{medicalTeamId:guid}/create" )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( MessageModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.BadRequest, Type = typeof( ErrorModel ) )]
        public IActionResult CreateNewTopicMessage(
            Guid projectId, Guid medicalTeamId, MessageRequestData messageData ) {
            MedicalTeam medicalTeam = null;
            Patient patient = null;
            Project project = null;

            var currentUser = GetCurrentUser();

            return RulesHelper
                .IfProjectIsValid( projectId, out project )
                .IfProjectIsOpen( projectId )
                .IfMedicalTeamIsValid( medicalTeamId, out medicalTeam )
                .IfMedicalTeamIsOpen( medicalTeamId )
                .IfPatientIsValid( currentUser.Id, out patient )
                .IfUserIsInMyInstitute( GetCurrentInstitute().Id, currentUser )
                .IfUserIsActive( currentUser )
                .Then( async () => {
                    var messageCreationParams = new MessageCreationParams() {
                        MedicalTeam = medicalTeam,
                        ShowAfterCreation = !messageData.HasAttachment,
                        User = currentUser,
                        UserRoles = GetCurrentUserRoles(),
                        MessageRequestData = messageData
                    };

                    try {
                        var messageCreated = _messageEditorService
                            .CreateNewTopicMessage( messageCreationParams );

                        if ( !messageData.HasAttachment ) {
                            await _messageNotifierService
                                .PerformPatientCreateNewTopic( currentUser.Id, messageCreated );
                        }

                        _logger.LogInformation(
                            $"(create): " +
                            $"New topic was created with id: {messageCreated.MessageId} " +
                            $"from user: {messageCreated.AuthorId}" );

                        return Ok( _messageFormatterService
                            .GetMessageAsPatient( messageCreated.MessageId, 0 ).OriginalMessage );
                    }
                    catch ( Exception e ) {
                        _logger.LogError(
                            $"(create): " +
                            $"New topic was not created, exception: {e.Message} " +
                            $"inner: {e?.InnerException}" );

                        return BadRequest( e.Message );
                    }
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Reply to a Message
        /// </summary>
        /// <param name="projectId">Project identifier</param>
        /// <param name="medicalTeamId">Medical team identifier</param>
        /// <param name="originalMessageId">Id of the original message to reply</param>
        /// <param name="messageData">Message information</param>
        /// <returns>Message information</returns>
        [HttpPost]
        [Authorize( Policy = Policies.MessagesReadWrite )]
        [Route( "{projectId:guid}/{medicalTeamId:guid}/replyTo/{originalMessageId:guid}" )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( MessageModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.BadRequest, Type = typeof( ErrorModel ) )]
        public IActionResult ReplyMessage(
            Guid projectId, Guid medicalTeamId, Guid originalMessageId, MessageRequestData messageData ) {

            MedicalTeam medicalTeam = null;
            Project project = null;
            Message message = null;

            var currentUser = GetCurrentUser();

            return RulesHelper
                .IfProjectIsValid( projectId, out project )
                .IfProjectIsOpen( projectId )
                .IfMedicalTeamIsValid( medicalTeamId, out medicalTeam )
                .IfMedicalTeamIsOpen( medicalTeamId )
                .IfMessageIsValid( originalMessageId, out message )
                .IfMessageCanBeRepliedByMedic( originalMessageId, GetCurrentUserRoles() )
                .IfMessageIsInMyInstitute( GetCurrentInstitute().Id, message )
                .IfUserIsActive( currentUser )
                .Then( async () => {
                    if ( !message.IsStartingMessage ) {
                        return BadRequest( "You can reply only to a OriginalMessage!" );
                    }

                    if ( message.MessageType == MessageType.Broadcast ) {
                        return BadRequest( "You can't reply to BroadcastMessage!" );
                    }

                    var messageCreationParams = new MessageCreationParams() {
                        MedicalTeam = medicalTeam,
                        ShowAfterCreation = !messageData.HasAttachment,
                        User = currentUser,
                        UserRoles = GetCurrentUserRoles(),
                        MessageRequestData = messageData
                    };

                    try {
                        var messageCreated = _messageEditorService
                            .ReplyToMessage( messageCreationParams, originalMessageId );

                        if ( !messageData.HasAttachment ) {
                            await _messageNotifierService.PerformUserRepliedToMessage(
                                currentUser.Id, messageCreated );
                        }

                        _logger.LogInformation(
                            $"(replyTo): " +
                            $"New reply was created with id: {messageCreated.MessageId} " +
                            $"from user: {messageCreated.AuthorId} " +
                            $"to message with id: {messageCreated.OriginalMessageId}" );

                        if ( HasRoleOf( Roles.Patient ) ) {
                            var reply = _messageFormatterService
                                .GetMessageAsPatient( message.Id, int.MaxValue )
                                .ReplyMessages
                                .FirstOrDefault( x => x.MessageId == messageCreated.MessageId );

                            return Ok( reply );
                        }
                        else {
                            var reply = _messageFormatterService
                                .GetMessageAsMedic( message.Id, int.MaxValue )
                                .ReplyMessages
                                .FirstOrDefault( x => x.MessageId == messageCreated.MessageId );

                            return Ok( reply );
                        }
                    }
                    catch ( Exception e ) {
                        _logger.LogError(
                            $"(replyTo): " +
                            $"New reply was not created, exception: {e.Message} " +
                            $"inner: {e?.InnerException}" );

                        return BadRequest( e.Message );
                    }
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Send a Broadcast Message
        /// </summary>
        /// <param name="projectId">Project identifier</param>
        /// <param name="medicalTeamId">Medical team identifier</param>
        /// <param name="messageData">Message information</param>
        /// <returns>Message information</returns>
        [HttpPost]
        [Authorize( Policy = Policies.MessagesBroadcastWrite )]
        [Route( "{projectId:guid}/{medicalTeamId:guid}/createbroadcast" )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( MessageModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.BadRequest, Type = typeof( ErrorModel ) )]
        public IActionResult CreateBroadcastMessage(
            Guid projectId, Guid medicalTeamId, MessageRequestData messageData ) {

            MedicalTeam medicalTeam = null;
            Project project = null;

            var currentUser = GetCurrentUser();

            return RulesHelper
                .IfProjectIsValid( projectId, out project )
                .IfProjectIsOpen( projectId )
                .IfMedicalTeamIsValid( medicalTeamId, out medicalTeam )
                .IfMedicalTeamIsOpen( medicalTeamId )
                .IfMedicalTeamIsInMyInstitute( GetCurrentInstitute().Id, medicalTeam )
                .Then( async () => {
                    var messageCreationParams = new MessageCreationParams() {
                        MedicalTeam = medicalTeam,
                        ShowAfterCreation = true,
                        User = currentUser,
                        UserRoles = GetCurrentUserRoles(),
                        MessageRequestData = messageData
                    };

                    try {
                        var messageCreated = _messageEditorService
                            .CreateBroadcastMessage( messageCreationParams );

                        await _messageNotifierService
                            .PerformCreateNewBroadcastMessage( currentUser.Id, messageCreated );

                        _logger.LogInformation(
                            $"(createbroadcast): " +
                            $"New broadcast message was created with id: {messageCreated.MessageId} " +
                            $"from user: {messageCreated.AuthorId}" );

                        return Ok( _organizedMessageProvider.AdaptToBroadcastMessage( messageCreated ) );
                    }
                    catch ( Exception e ) {
                        _logger.LogError(
                            $"(createbroadcast): " +
                            $"New broadcast message was not created, exception: {e.Message} " +
                            $"inner: {e?.InnerException}" );

                        return BadRequest( e.Message );
                    }
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Delete the message
        /// </summary>
        /// <param name="projectId">Project identifier</param>
        /// <param name="medicalTeamId">Medical team identifier</param>
        /// <param name="messageId">Message identifier</param>
        [HttpDelete]
        [Authorize( Policy = Policies.MessagesReadWrite )]
        [Route( "{projectId:guid}/{medicalTeamId:guid}/{messageId:guid}" )]
        [SwaggerResponse( (int)HttpStatusCode.OK )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.Conflict, Type = typeof( ErrorModel ) )]
        public IActionResult DeleteMessage( Guid projectId, Guid medicalTeamId, Guid messageId ) {
            MedicalTeam medicalTeam = null;
            Project project = null;
            Message message = null;

            var currentUser = GetCurrentUser();

            return RulesHelper
                .IfProjectIsValid( projectId, out project )
                .IfProjectIsOpen( projectId )
                .IfMedicalTeamIsValid( medicalTeamId, out medicalTeam )
                .IfMedicalTeamIsOpen( medicalTeamId )
                .IfMessageIsValid( messageId, out message )
                .IfUserIsActive( GetCurrentUser() )
                .IfMessageCanBeDeleted( messageId )
                .IfMessageIsInMyInstitute( GetCurrentInstitute().Id, message )
                .Then( () => {
                    _messageEditorService.RemoveMessage( message );

                    SaveChanges();

                    return Ok();
                } )
                .ReturnResult();
        }
    }
}