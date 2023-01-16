using Proact.Services.Models;
using Proact.Services.Models.Messages;
using Xunit;

namespace Proact.Services.FunctionalTests.Messages {
    public static class MessageEqual {
        public static void Equal(
            MessageRequestData request, MessageModel current ) {
            Assert.NotNull( current );
            Assert.Equal( request.Title, current.Title );
            Assert.Equal( request.Body, current.Body );
            Assert.Equal( request.Emotion, current.Emotion );
            Assert.Equal( request.MessageScope, current.MessageScope );
        }

        public static void Equal( MessageModel expected, MessageModel current ) {
            Assert.Equal( expected.MessageId, current.MessageId );
            Assert.Equal( expected.AuthorId, current.AuthorId );
            Assert.Equal( expected.AuthorName, current.AuthorName );
            Assert.Equal( expected.Title, current.Title );
            Assert.Equal( expected.Body, current.Body );
            Assert.Equal( expected.Emotion, current.Emotion );
            Assert.Equal( expected.MessageScope, current.MessageScope );
        }
    }
}
