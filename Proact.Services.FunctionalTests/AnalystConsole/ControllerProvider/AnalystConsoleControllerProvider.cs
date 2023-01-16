using Proact.Services.Controllers.AnalystConsole;
using Proact.Services.Entities;
using Proact.Services.QueriesServices;
using Proact.Services.Tests.Shared;

namespace Proact.Services.AnalystConsole.ControllerProvider {
    public class AnalystConsoleControllerProvider {
        private readonly AnalystConsoleController _controller;

        public AnalystConsoleController Controller {
            get { return _controller; }
        }

        public AnalystConsoleControllerProvider( ProactServicesProvider servicesProvider, User user, string role ) {
            var messagesFormatterService = servicesProvider.GetEditorService<IMessageFormatterService>();
            
            _controller = new AnalystConsoleController(
                messagesFormatterService, 
                servicesProvider.ChangesTrackingService, servicesProvider.ConsistencyRulesHelper );

            HttpContextMocker.MockHttpContext( _controller, user, role );
        }
    }
}
