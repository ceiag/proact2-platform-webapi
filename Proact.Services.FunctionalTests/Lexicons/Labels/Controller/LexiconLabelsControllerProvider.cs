using Proact.Services.Controllers;
using Proact.Services.Entities;
using Proact.Services.QueriesServices;
using Proact.Services.Tests.Shared;

namespace Proact.Services.FunctionalTests.Lexicons {
    public class LexiconLabelsControllerProvider {
        public readonly LexiconLabelController _lexiconLabelsController;

        public LexiconLabelController Controller {
            get { return _lexiconLabelsController; }
        }

        public LexiconLabelsControllerProvider(
            ProactServicesProvider servicesProvider, User user, string role ) {

            _lexiconLabelsController = new LexiconLabelController(
                servicesProvider.GetQueriesService<ILexiconLabelQueriesService>(),
                servicesProvider.ChangesTrackingService,
                servicesProvider.ConsistencyRulesHelper );

            HttpContextMocker.MockHttpContext( _lexiconLabelsController, user, role );
        }
    }
}
