using Microsoft.Extensions.Localization;
using Proact.Services.Entities;
using Proact.Services.EntitiesMapper;
using Proact.Services.Models;
using System.Collections.Generic;
using System.Linq;

namespace Proact.Services.Messages {
    public class OrganizedMessagesProvider : IOrganizedMessagesProvider {
        private readonly IStringLocalizer<Resource> _localizer;

        public OrganizedMessagesProvider( IStringLocalizer<Resource> localizer ) {
            _localizer = localizer;
        }

        public List<BranchedMessagesModel> GetBranchedMessages(
            List<Message> messages, int limitRepliesCount ) {
            List<BranchedMessagesModel> branchedMessages 
                = new List<BranchedMessagesModel>( messages.Count );

            foreach ( Message message in messages ) {
                branchedMessages.Add( Map( message, limitRepliesCount ) );
            }

            return branchedMessages;
        }

        public BranchedMessagesModel GetBranchedMessage( Message message, int limitRepliesCount ) {
            return Map( message, limitRepliesCount );
        }

        public List<MessageModel> GetAsMonodimensionalList( List<Message> messages ) {
            var messagesList = new List<MessageModel>( messages.Count );

            foreach ( Message message in messages ) {
                messagesList.Add( CustomizeIfBroadcastMessage( message ) );
            }

            return messagesList;
        }

        private BranchedMessagesModel Map( Message message, int limitRepliesCount ) {
            return new BranchedMessagesModel() {
                ReplyMessagesCount = message.Replies.Count,
                OriginalMessage = CustomizeIfBroadcastMessage( message ),
                ReplyMessages = MessagesEntityMapper.Map( 
                    message.Replies.Take( limitRepliesCount ).ToList() )
            };
        }

        private MessageModel CustomizeIfBroadcastMessage( Message message ) {
            var messageModel = MessagesEntityMapper.Map( message );

            if ( messageModel.MessageType == MessageType.Broadcast ) {
                messageModel = AdaptToBroadcastMessage( messageModel );
            }

            return messageModel;
        }

        public MessageModel AdaptToBroadcastMessage( MessageModel message ) {
            string medicalTeamAuthorName = _localizer["medical_team"];

            if ( !string.IsNullOrEmpty( medicalTeamAuthorName ) ) {
                message.AuthorName = medicalTeamAuthorName;
            }

            message.AvatarUrl = AvatarConfiguration.MedicAvatarDefaultUrl;
            return message;
        }
    }
}
