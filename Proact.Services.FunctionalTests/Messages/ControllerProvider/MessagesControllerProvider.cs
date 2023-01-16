using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Moq;
using Proact.Services.Controllers;
using Proact.Services.Entities;
using Proact.Services.Messages;
using Proact.Services.PushNotifications;
using Proact.Services.QueriesServices;
using Proact.Services.Tests.Shared;

namespace Proact.Services.FunctionalTests.Messages {
    public class MessagesControllerProvider {
        private MessagesController _messagesController;

        public MessagesController Controller {
            get { return _messagesController; }
        }

        private void CreateMessagesController(
            ProactServicesProvider servicesProvider, User user, string role ) {
            var stringLocalizer = servicesProvider.MockStringLocalizer();

            var branchedMessagesProvider = new OrganizedMessagesProvider( stringLocalizer );
            var messageEditorService = new MessageEditorService( servicesProvider.Database );

            var loggerService = new Mock<ILogger<MessagesController>>().Object;
            var notifierService = new Mock<IMessageNotifierService>().Object;
            var formatterService = new MessageFormatterService(
                servicesProvider.GetQueriesService<IMessagesQueriesService>(),
                branchedMessagesProvider,
                servicesProvider.GetQueriesService<IPatientQueriesService>() );
            var organizedMessageProvider = new OrganizedMessagesProvider( stringLocalizer );

            _messagesController = new MessagesController(
                servicesProvider.ChangesTrackingService,
                loggerService,
                notifierService,
                formatterService,
                messageEditorService,
                organizedMessageProvider,
                servicesProvider.ConsistencyRulesHelper );
        }

        public MessagesControllerProvider( ProactServicesProvider servicesProvider, User user, string role ) {
            CreateMessagesController( servicesProvider, user, role );
            HttpContextMocker.MockHttpContext( _messagesController, user, role );
        }

        public MessagesControllerProvider( 
            ProactServicesProvider servicesProvider, User user, string role, string queryString ) {
            CreateMessagesController( servicesProvider, user, role );
            HttpContextMocker.MockHttpContext( _messagesController, user, role, queryString );
        }
    }
}
