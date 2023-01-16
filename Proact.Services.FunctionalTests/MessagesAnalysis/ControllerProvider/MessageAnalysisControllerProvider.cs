using Proact.Services.Controllers.MessageAnalysis;
using Proact.Services.Entities;
using Proact.Services.QueriesServices;
using Proact.Services.Tests.Shared;

namespace Proact.Services.FunctionalTests.MessagesAnalysis {
    public class MessageAnalysisControllerProvider {
        private readonly MessageAnalysisController _controller;

        public MessageAnalysisController Controller {
            get { return _controller; }
        }

        public MessageAnalysisControllerProvider( ProactServicesProvider servicesProvider, User user, string role ) {
            _controller = new MessageAnalysisController(
                servicesProvider.GetQueriesService<IMessageAnalysisQueriesService>(),
                servicesProvider.ChangesTrackingService,
                servicesProvider.ConsistencyRulesHelper );

            HttpContextMocker.MockHttpContext( _controller, user, role );
        }
    }
}
