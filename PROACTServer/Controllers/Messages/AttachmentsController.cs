using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Proact.Services.AuthorizationPolicies;
using Proact.Services.Entities;
using Proact.Services.EntitiesMapper;
using Proact.Services.Models;
using Proact.Services.Models.Messages;
using Proact.Services.QueriesServices;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Net;

namespace Proact.Services.Controllers {
    [ApiController]
    [Route( ProactRouteConfiguration.DefaultRoute )]
    public class AttachmentsController : ProactBaseController {
        private readonly IMessageAttachmentManagerService _messageAttachmentManagerService;

        public AttachmentsController( 
            IChangesTrackingService changesTrackingService,
            IMessageAttachmentManagerService messageAttachmentManagerService,
            ConsistencyRulesHelper consistencyRulesHelper ) 
            : base( changesTrackingService, consistencyRulesHelper ) {
            _messageAttachmentManagerService = messageAttachmentManagerService;
        }

        /// <summary>
        /// Upload message content
        /// </summary>
        /// <param name="messageId">Message identifier</param>
        /// <param name="attachmentType">Type of Attachment: IMAGE: 0, VIDEO: 1, VOICE: 2 </param>
        /// <param name="mediaFile">Media File</param>
        /// <returns>Message information</returns>
        [HttpPost]
        [Authorize( Policy = Policies.MessagesReadWrite )]
        [Route( "{messageId:guid}/{attachmentType:int}" )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( MessageModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.BadRequest, Type = typeof( ErrorModel ) )]
        public IActionResult UploadAttachment(
            Guid messageId, AttachmentType attachmentType, IFormFile mediaFile ) {
            Message message = null;
            var currentUser = GetCurrentUser();

            return RulesHelper
                .IfMessageIsValid( messageId, out message )
                .IfUserIsAuthorOfMessage( message, currentUser.Id )
                .Then( async () => {
                    try {
                        if ( attachmentType == AttachmentType.VIDEO ) {
                            await _messageAttachmentManagerService
                                .UploadMediaFileOnTempFolder( mediaFile, message, AttachmentType.VIDEO );

                            BackgroundJob.Enqueue( () => _messageAttachmentManagerService
                                  .AttachVideoFileFromTempFolderToMessage( messageId ) );
                        }
                        else if ( attachmentType == AttachmentType.AUDIO ) {
                            await _messageAttachmentManagerService
                                .UploadMediaFileOnTempFolder( mediaFile, message, AttachmentType.AUDIO );

                            var request = new CreateAttachMediaFileRequest( mediaFile, attachmentType );

                            await _messageAttachmentManagerService
                                .AttachAudioFileFromTempFolderToMessage( message, request );
                        }
                        else if ( attachmentType == AttachmentType.IMAGE ) {
                            await _messageAttachmentManagerService
                                .AttachImageFileToMessage( message, mediaFile );
                        }
                        else {
                            return BadRequest( "Format is not supported" );
                        }

                        return Ok( MessagesEntityMapper.Map( message ) );
                    }
                    catch ( Exception e ) {
                        return BadRequest( e.Message );
                    }
                } )
                .ReturnResult();
        }

        /// <summary>
        /// Retrieve the message with SAS Token
        /// </summary>
        /// <param name="messageId">Message identifier</param>
        /// <returns>Uri with SAS token to get the media file</returns>
        [HttpGet]
        [Authorize( Policy = Policies.MessagesPatientRead )]
        [Route( "{messageId:guid}" )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( MediaFileSecurityInfosModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound, Type = typeof( ErrorModel ) )]
        [SwaggerResponse( (int)HttpStatusCode.BadRequest, Type = typeof( ErrorModel ) )]
        public IActionResult GetMediaFileSASUriAms( Guid messageId ) {
            Message message = null;
            var currentUser = GetCurrentUser();

            return RulesHelper
                .IfMessageIsValid( messageId, out message )
                .IfUserCanDecryptMedia( message, GetCurrentUserRoles(), currentUser.Id )
                .Then( async () => {
                    var messageAttachment = message.MessageAttachment;

                    if ( messageAttachment.AttachmentType != AttachmentType.VIDEO
                        && messageAttachment.AttachmentType != AttachmentType.AUDIO ) {
                        return BadRequest( "This message don't have a Video or an Audio attached" );
                    }

                    return Ok( await _messageAttachmentManagerService
                        .GetAttachmentUrlWithSASToken( messageAttachment ) );
                } )
                .ReturnResult();
        }
    }
}
