using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Proact.EncryptionAgentService.Configurations;
using Proact.EncryptionAgentService.Controllers;
using Proact.EncryptionAgentService.Decryption;
using Proact.EncryptionAgentService.Entities;
using Proact.EncryptionAgentService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Proact.EncryptionAgentService.UnitTests.Messages {
    public class GetMessagesUsingId_UnitTest : MessagesDataController_UnitTest {
        
        private List<MessageParameterModel> GetMessageParameterModels() {

            MessageParameterModel messageParameterModel_0 = new MessageParameterModel {
                MessageId = MessagesMockedDatabaseFactory.MessagesIds[0]
            };

            MessageParameterModel messageParameterModel_1 = new MessageParameterModel {
                MessageId = MessagesMockedDatabaseFactory.MessagesIds[1]
            };

            var messageParamaterModels = new List<MessageParameterModel>();
            messageParamaterModels.Add( messageParameterModel_0 );
            messageParamaterModels.Add( messageParameterModel_1 );

            return messageParamaterModels;
        }

        [Fact]
        public void GetMessagesUsingIds_ReturnOk() {
            using ( var database = MockedDatabaseFactory.GetMockedDatabase() ) {
                var result = GetMockedMessageDataController( database )
                    .GetMessagesUsingIds( GetMessageParameterModels().ToArray() );

                var statusCode = (IStatusCodeActionResult)result;

                Assert.Equal( StatusCodes.Status200OK, statusCode.StatusCode );
            }
        }

        [Fact]
        public void GetMessagesUsingIds_VerifyMessagesIds() {
            using ( var database = MockedDatabaseFactory.GetMockedDatabase() ) {
                var messagesParameters = GetMessageParameterModels();

                var result = GetMockedMessageDataController( database )
                    .GetMessagesUsingIds( messagesParameters.ToArray() );

                var messages = ( ( result as ObjectResult ).Value as MessageDataModel[] );

                Assert.Equal( messagesParameters.Count, messages.Length );

                foreach ( var messageParameter in messagesParameters ) {
                    var message = messages.ToList()
                        .FirstOrDefault( x => x.MessageId == messageParameter.MessageId );

                    Assert.NotNull( message );
                }
            }
        }
    }
}
