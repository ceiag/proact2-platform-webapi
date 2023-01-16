using Proact.Services.Controllers;
using Proact.Services.Entities;
using Proact.Services.QueriesServices;
using Proact.Services.Tests.Shared;

namespace Proact.Services.FunctionalTests.ProjectProperties.ControllerProvider;
public class ProjectPropertiesControllerProvider {
    private readonly ProjectPropertiesController _controller;

    public ProjectPropertiesController Controller {
        get { return _controller; }
    }

    public ProjectPropertiesControllerProvider(
        ProactServicesProvider servicesProvider, User user, string role ) {
        _controller = new ProjectPropertiesController(
            servicesProvider.GetQueriesService<IProjectPropertiesQueriesService>(),
            servicesProvider.ChangesTrackingService,
            servicesProvider.ConsistencyRulesHelper );

        HttpContextMocker.MockHttpContext( _controller, user, role );
    }
}
