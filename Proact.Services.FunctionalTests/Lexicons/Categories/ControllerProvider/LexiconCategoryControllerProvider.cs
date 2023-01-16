using Proact.Services.Controllers;
using Proact.Services.Entities;
using Proact.Services.QueriesServices;
using Proact.Services.Tests.Shared;

namespace Proact.Services.FunctionalTests.Lexicons.Categories {
    public class LexiconCategoryControllerProvider {
        public readonly LexiconsCategoryController _lexiconCategoryController;

        public LexiconsCategoryController Controller {
            get { return _lexiconCategoryController; }
        }

        public LexiconCategoryControllerProvider( 
            ProactServicesProvider servicesProvider, User user, string role ) {

            var lexiconCategoriesQueriesService = servicesProvider
                .GetQueriesService<ILexiconCategoriesQueriesService>();

            _lexiconCategoryController = new LexiconsCategoryController(
                lexiconCategoriesQueriesService, 
                servicesProvider.ChangesTrackingService, 
                servicesProvider.ConsistencyRulesHelper );

            HttpContextMocker.MockHttpContext( _lexiconCategoryController, user, role );
        }
    }
}
