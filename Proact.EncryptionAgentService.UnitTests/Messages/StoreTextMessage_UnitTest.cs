using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Proact.Configurations;
using Proact.EncryptionAgentService.Entities;
using Proact.EncryptionAgentService.Models;
using System;
using System.Linq;
using System.Text;
using Xunit;

namespace Proact.EncryptionAgentService.UnitTests.Messages {
    public class StoreTextMessage_UnitTest : MessagesDataController_UnitTest {

        private CreateMessageDataRequest GetCreateMessageDataModel() {
            string createMessageDataModelSerialized
                             = "{"
                             + "\"messageId\": \"947f35ab-02e2-4c6d-8b50-8da54feb4b58\","
                             + "\"EncryptedTitle\": \"d/2TJfsi4VZE1V3y6hT2qxis6eqo1D0u9Nsitk6odmwUGfLKL2Zw/1Z8iZCH8tAE\","
                             + "\"EncryptedBody\": \"3YYHs1tebId9Ar8xsZILmwrwoGVsiK3q4Y9NGPQM6vvaSXYjrIVJvrjM2PCIn0DJ\","
                             + "\"encryptedKey\": \"KIf37ADChdKbPKuyQuC1sOqk07hToTfr83h5ouPc40EIpUu4w0tAxChEzQ1lzusnFIliY+q9IhrWs9XMfw3GdQ4F7h8AgCdJAW0aHOXyEUQ6Zqwj1spjGIOAfb5os3eeD6w+oROMWKMrSUx0NKRuGZ5ZRNwFcmBon5zNsqRnpWs3ITZgjptaMXzKTdkI83bVeqwlrEVBO5azmrGG3nPGdLH0+Nmhh2TtUIBJMNV0F/HeDl4aPYwElaZB+AzDDIPbu2lTlQ4MboNO0O+U7yBO4h62DJjlJCtOX4+0H2iL2yhaJTPgdtlbEUcAMlz7/VchAl8z7CWX0eSpeECefIWGfg==\","
                             + "\"encryptedIV\": \"P2WOnI0b0JCHgsc2aOydBjYFh8wMBKnTYK+n/sx+zTtrC1L5O5J0lxCd+nsw2uEiAtHFKAbebB3PlKT29+sr2swDLgUDAOH0bDwV/oryx+y11lHxN2X1tXROz8CH9m4IxdWMOVG6JhBfYD+BMMLZHrEF2M+O1bgXGWU3w7KtounSgXpyybctBF5KhkSJcF278UzRlpbdtq90B9svk0ScUicK7ZLBgWA0bk+OgK9KjWU+LQVBVv5gEVugOlP09Dq3szgKpovPsvPrklj/xRdfq3wVA9lDw7tT6KHO3BakUFesQonwEfPQt99Y6hNpDmdAnVstTxwg2tDEPVNXhoS8uw==\""
                             + "}";

            var msg = JsonConvert
                .DeserializeObject<CreateMessageDataRequest>( createMessageDataModelSerialized );

            return msg;
        }

        [Fact]
        public void StoreTextMessage_ReturnOk() {
            using ( var database = MockedDatabaseFactory.GetMockedDatabase() ) {
                var config = new ConfigurationBuilder()
                    .AddJsonFile( "appsettings.UnitTest.json" )
                    .Build();

                ProactConfiguration.Init( config );

                var result = GetMockedMessageDataController( database )
                    .StoreTextMessage( GetCreateMessageDataModel() );

                var statusCode = (IStatusCodeActionResult)result;

                Assert.Equal( StatusCodes.Status200OK, statusCode.StatusCode );

                VerifyMessageCreatedIsTheSame( result, database );
            }
        }

        private void VerifyMessageCreatedIsTheSame( 
            IActionResult result, ProactAgentDatabaseContext database ) {
            var messageDataCreated
                    = ( ( result as ObjectResult ).Value as MessageData );

            var createdMsg = database.Messages
                .FirstOrDefault( x => x.Id == messageDataCreated.Id );

            Assert.NotNull( createdMsg );
            Assert.Equal( messageDataCreated.Id, createdMsg.Id );
        }

        [Fact]
        public void StoreTextMessage_ReturnConflict() {
            using ( var database = MockedDatabaseFactory.GetMockedDatabase() ) {
                var messageRequest = GetCreateMessageDataModel();
                messageRequest.MessageId = MessagesMockedDatabaseFactory.MessagesIds[0];

                var result = GetMockedMessageDataController( database )
                    .StoreTextMessage( messageRequest );

                var statusCode = (IStatusCodeActionResult)result;

                Assert.Equal( StatusCodes.Status409Conflict, statusCode.StatusCode );
            }
        }
    }
}
