using Proact.Services.Controllers;
using Proact.Services.Entities;
using Proact.Services.QueriesServices;
using Proact.Services.Tests.Shared;

namespace Proact.Services.FunctionalTests.Researchers {
    public class MedicsControllerProvider {
        private readonly ResearchersController _controller;

        public ResearchersController Controller {
            get { return _controller; }
        }

        public MedicsControllerProvider( ProactServicesProvider servicesProvider, User user, string role ) {
            var medicalTeamQueriesService = servicesProvider.GetQueriesService<IMedicalTeamQueriesService>();
            var researcherQueriesService = servicesProvider.GetQueriesService<IResearcherQueriesService>();

            _controller = new ResearchersController(
                servicesProvider.ChangesTrackingService,
                medicalTeamQueriesService, researcherQueriesService,
                servicesProvider.UsersCreatorQueriesService, servicesProvider.ConsistencyRulesHelper );

            HttpContextMocker.MockHttpContext( _controller, user, role );
        }
    }
}
