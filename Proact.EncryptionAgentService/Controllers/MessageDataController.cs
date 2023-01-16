using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Proact.EncryptionAgentService.Configurations;
using Proact.EncryptionAgentService.Decryption;
using Proact.EncryptionAgentService.Entities;
using Proact.EncryptionAgentService.Models;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;

namespace Proact.EncryptionAgentService.Controllers {
    [ApiController]
    [Route( "api/[controller]" )]
    public class MessageDataController : Controller {

        private ProactAgentDatabaseContext _database;
        private IDecryptionService _decryptionService;
        private IMapper _mapper = AutoMapperConfiguration.Mapper;

        public MessageDataController( 
            ProactAgentDatabaseContext database, IDecryptionService decryptionService ) {
            _database = database;
            _decryptionService = decryptionService;
        }

        /// <summary>
        /// Store a new message
        /// </summary>
        /// <param name="messageDataModel">CreateMessageDataModel</param>
        /// <returns>Message information</returns>
        [HttpPost]
        [SwaggerResponse( (int)HttpStatusCode.Conflict )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( MessageData ) )]
        public IActionResult StoreTextMessage( CreateMessageDataRequest messageDataModel ) {
            var existMessage = _database
                .Messages.Find( messageDataModel.MessageId );

            if ( existMessage != null ) {
                return Conflict();
            }

            var newMessage = _mapper.Map<MessageData>( messageDataModel );

            _decryptionService.DecryptMessageData( messageDataModel, newMessage );

            _database.Messages.Add( newMessage );
            _database.SaveChanges();

            return Ok( newMessage );
        }

        /// <summary>
        /// Edit a message from a conversation
        /// </summary>
        /// <param name="messageDataModel">CreateMessageDataModel</param>
        /// <returns>Message information</returns>
        [HttpPost]
        [SwaggerResponse( (int)HttpStatusCode.NotFound )]
        [Route( "EditTextMessage" )]
        public IActionResult EditTextMessage( CreateMessageDataRequest messageDataModel ) {
            var existMessage = _database.Messages
                .Find( messageDataModel.MessageId );

            if ( existMessage == null ) {
                return NotFound();
            }

            var editedMessage = _mapper.Map<MessageData>( messageDataModel );

            _decryptionService.DecryptMessageData( messageDataModel, editedMessage );

            existMessage.Title = editedMessage.Title;
            existMessage.Body = editedMessage.Body;
            
            _database.Messages.Update( existMessage );
            _database.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Retrive all messages
        /// </summary>
        /// <returns>Message information</returns>
        [HttpGet]
        public IActionResult GetAllMessages() {
            var messagesData = MessagesMapper.Map( _database.Messages.ToArray() );

            return Ok( messagesData );
        }

        /// <summary>
        /// Return messages with given Ids
        /// </summary>
        /// <returns>List of Messages</returns>
        [HttpPost]
        [Route( "GetMessagesUsingIds" )]
        public IActionResult GetMessagesUsingIds( MessageParameterModel[] messagesIds ) {
            var ids = new SortedSet<Guid>( messagesIds
                .Select( message => message.MessageId ) );

            var databaseMessages = _database
                .Messages.Where( message => ids.Contains( message.Id ) );

            var outputModels = MessagesMapper.Map( databaseMessages.ToArray() );

            return Ok( outputModels );
        }

        /// <summary>
        /// Return message with given Id
        /// </summary>
        /// <returns>Message</returns>
        [HttpGet]
        [Route( "GetMessageFromId" )]
        [SwaggerResponse( (int)HttpStatusCode.NotFound )]
        public IActionResult GetMessageFromId( Guid messageId ) {
            var databaseMessage = _database
                .Messages.FirstOrDefault( x => x.Id == messageId );

            var outputModels = MessagesMapper.Map( databaseMessage );

            if ( outputModels == null ) {
                return NotFound( String.Format( "MessageId {0} Not Found!", messageId ) );
            }

            return Ok( outputModels );
        }
    }
}
