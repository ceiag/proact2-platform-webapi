using Microsoft.AspNetCore.Mvc;
using Proact.Services.AuthorizationPolicies;
using Proact.Services.Entities;
using Proact.Services.Models;
using Proact.Services.Tests.Shared;
using System.Collections.Generic;
using Xunit;

namespace Proact.Services.FunctionalTests.Messages {
    public class SearchMessagesAsMedicalProfesional {
        [Fact]
        public void _MustReturnAMessage_WithNotAnonymousPatients() {
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
                servicesProvider, medic.User, Roles.MedicalProfessional, queryString );
            var result = analystConsoleController.Controller
                .SearchMessagesAsMedicalProfessionalWith( project.Id, medicalTeam.Id );

            var messagesResult = ( result as OkObjectResult ).Value as List<MessageModel>;

            Assert.NotNull( messagesResult );
            Assert.Single( messagesResult );
            Assert.Equal( patient.User.Name, messagesResult[0].AuthorName );
        }

        [Fact]
        public void _MustReturnAMessage_WithAnonymousPatients() {
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
                .AddPatientAnonymousWithRandomValues( medicalTeam, out patient )
                .AddMedicWithRandomValues( medicalTeam, out medic )
                .AddMessageFromPatient( patient, "message content", out message_0 )
                .AddMessageFromPatient( patient, "this is another thing", out message_1 );

            string queryString = "?message=message";

            var analystConsoleController = new MessagesControllerProvider(
                servicesProvider, medic.User, Roles.MedicalProfessional, queryString );
            var result = analystConsoleController.Controller
                .SearchMessagesAsMedicalProfessionalWith( project.Id, medicalTeam.Id );

            var messagesResult = ( result as OkObjectResult ).Value as List<MessageModel>;

            Assert.NotNull( messagesResult );
            Assert.Single( messagesResult );
            Assert.Equal( patient.Code, messagesResult[0].AuthorName );
        }

        [Fact]
        public void AsResearcher_MustReturnAMessage_MustBeAnonymous() {
            User admin = null;
            Institute institute = null;
            Project project = null;
            Patient patient = null;
            Researcher researcher = null;
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
                .AddResearcherWithRandomValues( medicalTeam, out researcher )
                .AddMessageFromPatient( patient, "message content", out message_0 )
                .AddMessageFromPatient( patient, "this is another thing", out message_1 );

            string queryString = "?message=message";

            var analystConsoleController = new MessagesControllerProvider(
                servicesProvider, researcher.User, Roles.Researcher, queryString );
            var result = analystConsoleController.Controller
                .SearchMessagesAsMedicalProfessionalWith( project.Id, medicalTeam.Id );

            var messagesResult = ( result as OkObjectResult ).Value as List<MessageModel>;

            Assert.NotNull( messagesResult );
            Assert.Single( messagesResult );
            Assert.Equal( patient.Code, messagesResult[0].AuthorName );
        }
    }
}
