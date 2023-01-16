using Proact.Services.Controllers;
using Proact.Services.Entities;
using Proact.Services.QueriesServices;
using Proact.Services.Tests.Shared;

namespace Proact.Services.FunctionalTests.Lexicons {
    public class LexiconControllerProvider {
        public readonly LexiconsController _lexiconController;

        public LexiconsController Controller {
            get { return _lexiconController; }
        }

        public LexiconControllerProvider(
            ProactServicesProvider servicesProvider, User user, string role ) {

            var lexiconQueriesService = servicesProvider.GetQueriesService<ILexiconQueriesService>();

            _lexiconController = new LexiconsController(
                lexiconQueriesService,
                servicesProvider.ChangesTrackingService,
                servicesProvider.ConsistencyRulesHelper );

            HttpContextMocker.MockHttpContext( _lexiconController, user, role );
        }
    }
}
