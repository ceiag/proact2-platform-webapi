using Proact.Services.Controllers;
using Proact.Services.Entities;
using Proact.Services.QueriesServices;
using Proact.Services.Tests.Shared;

namespace Proact.Services.FunctionalTests.Surveys.Questions {
    public class QuestionsControllerProvider {
        private readonly SurveysQuestionsController _controller;

        public SurveysQuestionsController Controller {
            get { return _controller; }
        }

        public QuestionsControllerProvider( ProactServicesProvider servicesProvider, User user, string role ) {
            var questionsService = servicesProvider.GetEditorService<ISurveyQuestionsEditorService>();

            _controller = new SurveysQuestionsController(
                servicesProvider.ChangesTrackingService,
                servicesProvider.ConsistencyRulesHelper, questionsService );

            HttpContextMocker.MockHttpContext( _controller, user, role );
        }
    }
}
