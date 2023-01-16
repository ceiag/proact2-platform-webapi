using Moq;
using Proact.Services.Controllers;
using Proact.Services.Entities;
using Proact.Services.QueriesServices;
using Proact.Services.Services.EmailSender;
using Proact.Services.Tests.Shared;

namespace Proact.Services.FunctionalTests.Patients {
    public class PatientControllerProvider {
        private readonly PatientController _patientController;

        public PatientController Controller {
            get { return _patientController; }
        }

        public PatientControllerProvider( ProactServicesProvider servicesProvider, User user, string role ) {
            var patientQueriesService = servicesProvider.GetQueriesService<IPatientQueriesService>();

            var emailService = new Mock<IEmailSenderService>();

            _patientController = new PatientController(
                servicesProvider.ChangesTrackingService, servicesProvider.ConsistencyRulesHelper,
                patientQueriesService, servicesProvider.UsersCreatorQueriesService, emailService.Object );

            HttpContextMocker.MockHttpContext( _patientController, user, role );
        }
    }
}
