using Microsoft.AspNetCore.Mvc;
using Proact.Services.Controllers.Surveys;
using Proact.Services.Entities;
using Proact.Services.Models;
using Proact.Services.QueriesServices;
using Proact.Services.QueriesServices.Surveys.Stats;
using Proact.Services.Tests.Shared;
using System;

namespace Proact.Services.FunctionalTests.Surveys.Surveys {
    public class SurveyControllerProvider {
        private readonly SurveyController _controller;

        public SurveyController Controller {
            get { return _controller; }
        }

        public SurveyControllerProvider( ProactServicesProvider servicesProvider, User user, string role ) {
            var questionsSetService = servicesProvider
                .GetQueriesService<ISurveyQueriesService>();
            var surveyStatsService = servicesProvider
                .GetEditorService<ISurveysStatsQueriesService>();
            var surveyStatsOverTimeService = servicesProvider
                .GetEditorService<ISurveyStatsOverTimeQueriesService>();

            _controller = new SurveyController(
                servicesProvider.ChangesTrackingService,
                servicesProvider.ConsistencyRulesHelper, 
                questionsSetService, surveyStatsService, surveyStatsOverTimeService );

            HttpContextMocker.MockHttpContext( _controller, user, role );
        }

        public SurveyModel Get( Guid surveyId ) {
            return ( _controller.GetSurvey( surveyId ) as OkObjectResult ).Value as SurveyModel;
        }
    }
}
