using Microsoft.AspNetCore.Mvc;
using Proact.Services.AuthorizationPolicies;
using Proact.Services.Entities;
using Proact.Services.Models;
using Proact.Services.Tests.Shared;
using System.Collections.Generic;
using Xunit;

namespace Proact.Services.FunctionalTests.Messages {
    public class SearchMessagesAsPatient {
        [Fact]
        public void _MustReturnAMessage() {
            User admin = null;
            Institute institute = null;
            Project project = null;
            Patient patient = null;
            Medic medic = null;
            MedicalTeam medicalTeam = null;
            MessageModel message_0 = null;
            MessageModel message_1 = null;

            var servicesProvider = new ProactServicesProvider();
            new DatabaseSnapshotProvider( servicesProvider )
                .AddSystemAdmin( out admin )
                .AddInstituteWithRandomValues( out institute )
                .AddProjectWithRandomValues( institute, out project )
                .AddMedicalTeamWithRandomValues( project, out medicalTeam )
                .AddPatientWithRandomValues( medicalTeam, out patient )
                .AddMedicWithRandomValues( medicalTeam, out medic )
                .AddMessageFromPatient( patient, "message content", out message_0 )
                .AddMessageFromPatient( patient, "this is another thing", out message_1 );

            string queryString = "?message=message";

            var analystConsoleController = new MessagesControllerProvider(
                servicesProvider, patient.User, Roles.Patient, queryString );
            var result = analystConsoleController.Controller
                .SearchMessagesAsPatient( project.Id, medicalTeam.Id );

            var messagesResult = ( result as OkObjectResult ).Value as List<MessageModel>;

            Assert.NotNull( messagesResult );
            Assert.Single( messagesResult );
            Assert.Equal( patient.User.Name, messagesResult[0].AuthorName );
        }
    }
}
