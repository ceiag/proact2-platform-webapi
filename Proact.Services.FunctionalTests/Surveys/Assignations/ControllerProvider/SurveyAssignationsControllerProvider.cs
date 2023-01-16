using Microsoft.AspNetCore.Mvc;
using Moq;
using Proact.Services.Controllers;
using Proact.Services.Controllers.Surveys;
using Proact.Services.Entities;
using Proact.Services.Models;
using Proact.Services.QueriesServices;
using Proact.Services.QueriesServices.Surveys.Scheduler;
using Proact.Services.Tests.Shared;
using System;

namespace Proact.Services.FunctionalTests.Surveys.Assignations {
    public class SurveyAssignationControllerProvider {
        private readonly SurveyAssegnationsController _controller;

        public SurveyAssegnationsController Controller {
            get { return _controller; }
        }

        public SurveyAssignationControllerProvider( ProactServicesProvider servicesProvider, User user, string role ) {
            _controller = new SurveyAssegnationsController(
                servicesProvider.ChangesTrackingService,
                servicesProvider.GetQueriesService<ISurveyAssignationQueriesService>(),
                servicesProvider.GetEditorService<ISurveyAnswerToQuestionEditorService>(),
                servicesProvider.GetQueriesService<ISurveySchedulerQueriesService>(),
                servicesProvider.GetQueriesService<ISurveyQueriesService>(),
                servicesProvider.GetEditorService<ISurveySchedulerDispatcherService>(),
                servicesProvider.ConsistencyRulesHelper );

            HttpContextMocker.MockHttpContext( _controller, user, role );
        }

        public SurveyCompiledModel GetCompiledSurvey( Guid assignationId ) {
            return ( _controller.GetCompiledSurveyFromPatient( assignationId ) as OkObjectResult )
                .Value as SurveyCompiledModel;
        }
    }
}
