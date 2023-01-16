using Proact.Services.Controllers;
using Proact.Services.Entities;
using Proact.Services.QueriesServices;
using Proact.Services.Tests.Shared;

namespace Proact.Services.FunctionalTests.Projects {
    public class ProjectsControllerProvider {
        private readonly ProjectsController _projectController;

        public ProjectsController Controller {
            get { return _projectController; }
        }

        public ProjectsControllerProvider( ProactServicesProvider servicesProvider, User user, string role ) {
            var projectQueriesServices = servicesProvider.GetQueriesService<IProjectQueriesService>();
            var projectEditorServices = servicesProvider.GetEditorService<IProjectStateEditorService>();
            var projectPropsQueriesServices = servicesProvider
                .GetQueriesService<IProjectPropertiesQueriesService>();
            var instituteQueriesServices = servicesProvider.GetQueriesService<IInstitutesQueriesService>();

            _projectController = new ProjectsController(
                servicesProvider.ChangesTrackingService,
                servicesProvider.ConsistencyRulesHelper,
                projectQueriesServices, projectPropsQueriesServices,
                instituteQueriesServices, projectEditorServices );

            HttpContextMocker.MockHttpContext( _projectController, user, role );
        }
    }
}
