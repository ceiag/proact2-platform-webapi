using Microsoft.AspNetCore.Mvc;
using Proact.Services.Controllers;
using Proact.Services.Entities;
using Proact.Services.QueriesServices;
using Proact.Services.Tests.Shared;
using System;

namespace Proact.Services.FunctionalTests.Medics {
    public class MedicsControllerProvider {
        private readonly MedicsController _medicsController;

        public MedicsController Controller {
            get { return _medicsController; }
        }

        public MedicsControllerProvider( ProactServicesProvider servicesProvider, User user, string role ) {
            var medicalTeamQueriesService = servicesProvider.GetQueriesService<IMedicalTeamQueriesService>();
            var medicQueriesService = servicesProvider.GetQueriesService<IMedicQueriesService>();

            _medicsController = new MedicsController(
                servicesProvider.ChangesTrackingService,
                medicalTeamQueriesService, medicQueriesService,
                servicesProvider.UsersCreatorQueriesService, servicesProvider.ConsistencyRulesHelper );

            HttpContextMocker.MockHttpContext( _medicsController, user, role );
        }
    }
}
