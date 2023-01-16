using Microsoft.AspNetCore.Mvc;
using Proact.Services.Controllers;
using Proact.Services.Entities;
using Proact.Services.Models;
using Proact.Services.QueriesServices;
using Proact.Services.Tests.Shared;
using System;

namespace Proact.Services.FunctionalTests.Surveys.QuestionsSets {
    public class QuestionsSetsControllerProvider {
        private readonly SurveysQuestionsSetsController _controller;

        public SurveysQuestionsSetsController Controller {
            get { return _controller; }
        }

        public QuestionsSetsControllerProvider( ProactServicesProvider servicesProvider, User user, string role ) {
            var questionsSetService = servicesProvider
                .GetQueriesService<ISurveyQuestionsSetQueriesService>();
            
            _controller = new SurveysQuestionsSetsController(
                servicesProvider.ChangesTrackingService, 
                servicesProvider.ConsistencyRulesHelper, questionsSetService );

            HttpContextMocker.MockHttpContext( _controller, user, role );
        }

        public SurveyQuestionsSetModel GetQuestionsSetModel( Guid projectId, Guid questionsSetId ) {
            return ( _controller.GetQuestionsSet( questionsSetId ) as OkObjectResult )
                .Value as SurveyQuestionsSetModel;
        }
    }
}
