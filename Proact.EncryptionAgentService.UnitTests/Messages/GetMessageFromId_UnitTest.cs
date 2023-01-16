using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System;
using System.Linq;
using Xunit;

namespace Proact.EncryptionAgentService.UnitTests.Messages {
    public class GetMessageFromId_UnitTest : MessagesDataController_UnitTest {
        [Fact]
        public void GetMessageFromId_ReturnOk() {
            using ( var database = MockedDatabaseFactory.GetMockedDatabase() ) {
                var messageId = database.Messages.ToList()[0].Id;

                var result = GetMockedMessageDataController( database )
                    .GetMessageFromId( messageId );

                var statusCode = (IStatusCodeActionResult)result;

                Assert.Equal( StatusCodes.Status200OK, statusCode.StatusCode );
            }
        }

        [Fact]
        public void GetMessageFromId_ReturnNotFound() {
            using ( var database = MockedDatabaseFactory.GetMockedDatabase() ) {
                var messageId = Guid.NewGuid();

                var result = GetMockedMessageDataController( database )
                    .GetMessageFromId( messageId );

                var statusCode = (IStatusCodeActionResult)result;

                Assert.Equal( StatusCodes.Status404NotFound, statusCode.StatusCode );
            }
        }
    }
}
