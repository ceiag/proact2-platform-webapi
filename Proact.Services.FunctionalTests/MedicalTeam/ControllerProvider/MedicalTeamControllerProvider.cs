using Moq;
using Proact.Services.Controllers;
using Proact.Services.Entities;
using Proact.Services.QueriesServices;
using Proact.Services.Services;
using Proact.Services.Tests.Shared;

namespace Proact.Services.FunctionalTests.Institutes {
    public class MedicalTeamControllerProvider {
        private readonly MedicalTeamController _medicalTeamController;

        public MedicalTeamController Controller {
            get { return _medicalTeamController; }
        }

        public MedicalTeamControllerProvider( ProactServicesProvider servicesProvider, User user, string role ) {
            var medicalTeamQueriesService = servicesProvider.GetQueriesService<IMedicalTeamQueriesService>();
            var medicQueriesService = servicesProvider.GetQueriesService<IMedicQueriesService>();
            var projectEditorService = servicesProvider.GetEditorService<IProjectStateEditorService>();
            var usersGroupService = new Mock<IGroupService>().Object;

            _medicalTeamController = new MedicalTeamController(
                servicesProvider.ChangesTrackingService,
                medicalTeamQueriesService, medicQueriesService, projectEditorService,
                servicesProvider.ConsistencyRulesHelper, usersGroupService );

            HttpContextMocker.MockHttpContext( _medicalTeamController, user, role );
        }
    }
}
