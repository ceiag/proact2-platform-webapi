using Proact.Services.Controllers;
using Proact.Services.Entities;
using Proact.Services.QueriesServices;
using Proact.Services.QueriesServices.DataManagers;
using Proact.Services.Tests.Shared;

namespace Proact.Services.FunctionalTests.DataManagers.ControllerProvider;
internal class DataManagersControllerProvider {
    private readonly DataManagersController _controller;

    public DataManagersController Controller {
        get { return _controller; }
    }

    public DataManagersControllerProvider( 
        ProactServicesProvider servicesProvider, User user, string role ) {
        var medicalTeamQueriesService = servicesProvider
            .GetQueriesService<IMedicalTeamQueriesService>();
        var dataManagersQueriesService = servicesProvider
            .GetQueriesService<IDataManagerQueriesService>();

        _controller = new DataManagersController(
            servicesProvider.ChangesTrackingService,
            dataManagersQueriesService,
            medicalTeamQueriesService,
            servicesProvider.UsersCreatorQueriesService,
            servicesProvider.ConsistencyRulesHelper );

        HttpContextMocker.MockHttpContext( _controller, user, role );
    }
}
