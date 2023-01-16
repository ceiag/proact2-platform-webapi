using Microsoft.AspNetCore.Mvc;
using Proact.Services.AnalystConsole.ControllerProvider;
using Proact.Services.AuthorizationPolicies;
using Proact.Services.Entities;
using Proact.Services.Models;
using Proact.Services.Tests.Shared;
using System.Collections.Generic;
using Xunit;

namespace Proact.Services.FunctionalTests.AnalystConsole {
    public class GetMessagesFromPatient {
        [Fact]
        public void _AsReseacher_CheckConsistency() {
            User admin = null;
            Institute institute = null;
            Project project = null;
            Patient patient = null;
            Medic medic = null;
            MedicalTeam medicalTeam = null;
            MessageModel message = null;
            MessageModel reply_0 = null;
            MessageModel reply_1 = null;

            var servicesProvider = new ProactServicesProvider();
            new DatabaseSnapshotProvider( servicesProvider )
                .AddSystemAdmin( out admin )
                .AddInstituteWithRandomValues( out institute )
                .AddProjectWithRandomValues( institute, out project )
                .AddMedicalTeamWithRandomValues( project, out medicalTeam )
                .AddPatientWithRandomValues( medicalTeam, out patient )
                .AddMedicWithRandomValues( medicalTeam, out medic )
                .AddMessageFromPatientWithRandomValues( patient, out message )
                .AddReplyFromMedicWithRandomValues( medic, message.MessageId, out reply_0 )
                .AddReplyFromPatientWithRandomValues( patient, message.MessageId, out reply_1 );

            var analystConsoleController = new AnalystConsoleControllerProvider(
                servicesProvider, medic.User, Roles.Researcher );
            var result = analystConsoleController.Controller
                .GetMessagesFromPatient( patient.UserId );

            var messagesResult = ( result as OkObjectResult ).Value as List<BranchedMessagesModel>;

            Assert.NotNull( messagesResult );
            Assert.Equal( patient.Code, messagesResult[0].OriginalMessage.AuthorName );
            Assert.Equal( medic.User.Name, messagesResult[0].ReplyMessages[0].AuthorName );
            Assert.Equal( patient.Code, messagesResult[0].ReplyMessages[1].AuthorName );
        }

        [Fact]
        public void _AsMedic_CheckConsistency() {
            User admin = null;
            Institute institute = null;
            Project project = null;
            Patient patient = null;
            Medic medic = null;
            MedicalTeam medicalTeam = null;
            MessageModel message = null;
            MessageModel reply_0 = null;
            MessageModel reply_1 = null;

            var servicesProvider = new ProactServicesProvider();
            new DatabaseSnapshotProvider( servicesProvider )
                .AddSystemAdmin( out admin )
                .AddInstituteWithRandomValues( out institute )
                .AddProjectWithRandomValues( institute, out project )
                .AddMedicalTeamWithRandomValues( project, out medicalTeam )
                .AddPatientWithRandomValues( medicalTeam, out patient )
                .AddMedicWithRandomValues( medicalTeam, out medic )
                .AddMessageFromPatientWithRandomValues( patient, out message )
                .AddReplyFromMedicWithRandomValues( medic, message.MessageId, out reply_0 )
                .AddReplyFromPatientWithRandomValues( patient, message.MessageId, out reply_1 );

            var analystConsoleController = new AnalystConsoleControllerProvider(
                servicesProvider, medic.User, Roles.MedicalProfessional );
            var result = analystConsoleController.Controller
                .GetMessagesFromPatient( patient.UserId );

            var messagesResult = ( result as OkObjectResult ).Value as List<BranchedMessagesModel>;

            Assert.NotNull( messagesResult );
            Assert.Equal( patient.User.Name, messagesResult[0].OriginalMessage.AuthorName );
            Assert.Equal( medic.User.Name, messagesResult[0].ReplyMessages[0].AuthorName );
            Assert.Equal( patient.User.Name, messagesResult[0].ReplyMessages[1].AuthorName );
        }
    }
}
