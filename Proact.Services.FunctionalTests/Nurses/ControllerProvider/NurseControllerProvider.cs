using Proact.Services.Controllers;
using Proact.Services.Entities;
using Proact.Services.QueriesServices;
using Proact.Services.Tests.Shared;

namespace Proact.Services.FunctionalTests.Nurses {
    public class NursesControllerProvider {
        private readonly NursesController _nursesController;

        public NursesController Controller {
            get { return _nursesController; }
        }

        public NursesControllerProvider( ProactServicesProvider servicesProvider, User user, string role ) {
            var medicalTeamQueriesService = servicesProvider.GetQueriesService<IMedicalTeamQueriesService>();
            var nursesQueriesService = servicesProvider.GetQueriesService<INurseQueriesService>();

            _nursesController = new NursesController(
                servicesProvider.ChangesTrackingService,
                nursesQueriesService, medicalTeamQueriesService,
                servicesProvider.UsersCreatorQueriesService, servicesProvider.ConsistencyRulesHelper );

            HttpContextMocker.MockHttpContext( _nursesController, user, role );
        }
    }
}
