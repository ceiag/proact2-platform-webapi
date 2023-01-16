using Microsoft.AspNetCore.Mvc;
using Moq;
using Proact.Services.Controllers;
using Proact.Services.Controllers.Institutes;
using Proact.Services.Entities;
using Proact.Services.Models;
using Proact.Services.QueriesServices;
using Proact.Services.Services;
using Proact.Services.Tests.Shared;
using System;

namespace Proact.Services.FunctionalTests.Institutes {
    public class InstitutesControllerProvider {
        private readonly InstitutesController _instituteController;

        public InstitutesController Controller {
            get { return _instituteController; }
        }

        public InstitutesControllerProvider( ProactServicesProvider servicesProvider, User user, string role ) {
            var instituteQueriesService = servicesProvider.GetQueriesService<IInstitutesQueriesService>();
            var usersCreatorService = new Mock<IUsersCreatorQueriesService>().Object;
            var usersGroupService = new Mock<IGroupService>().Object;

            _instituteController = new InstitutesController(
                instituteQueriesService, usersCreatorService, usersGroupService,
                servicesProvider.ChangesTrackingService, servicesProvider.ConsistencyRulesHelper );

            HttpContextMocker.MockHttpContext( _instituteController, user, role );
        }

        public InstituteModel GetInstitute( Guid instituteId ) {
            return ( _instituteController.Get( instituteId ) as OkObjectResult ).Value as InstituteModel;
        }
    }
}
