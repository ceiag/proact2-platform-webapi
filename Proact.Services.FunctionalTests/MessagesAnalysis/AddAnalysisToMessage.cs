using Microsoft.AspNetCore.Mvc;
using Proact.Services.AuthorizationPolicies;
using Proact.Services.Entities;
using Proact.Services.Entities.MessageAnalysis;
using Proact.Services.Models;
using Proact.Services.Tests.Shared;
using System.Collections.Generic;
using Xunit;

namespace Proact.Services.FunctionalTests.MessagesAnalysis {
    public class AddAnalysisToMessage {
        [Fact]
        public void AddAnalysisToMessage_ConsistencyCheck() {
            User instituteAdmin = null;
            Institute institute = null;
            Project project = null;
            MedicalTeam medicalTeam = null;
            Medic medic = null;
            Patient patient = null;
            MessageModel message = null;
            Lexicon lexicon;

            var servicesProvider = new ProactServicesProvider();
            new DatabaseSnapshotProvider( servicesProvider )
                .AddInstituteWithRandomValues( out institute )
                .AddInstituteAdminWithRandomValues( institute, out instituteAdmin )
                .AddLexiconWithRandomValues( institute, out lexicon )
                .AddProjectWithRandomValues( institute, out project )
                .AddMedicalTeamWithRandomValues( project, out medicalTeam )
                .AddMedicWithRandomValues( medicalTeam, out medic )
                .AddPatientWithRandomValues( medicalTeam, out patient )
                .AddMessageFromPatientWithRandomValues( patient, out message );

            var request = new AnalysisCreationRequest() {
                MessageId = message.MessageId,
                AnalysisResults = new List<AnalysisResultCreationRequest>() {
                    new AnalysisResultCreationRequest() {
                        LabelId = lexicon.Categories[0].Labels[0].Id
                    }
                }
            };

            var provider = new MessageAnalysisControllerProvider(
                servicesProvider, medic.User, Roles.MedicalTeamAdmin );
            var apiResult = provider.Controller.AddAnalysisToMessage( message.MessageId, request );

            Assert.Equal( 200, ( apiResult as OkObjectResult ).StatusCode );

            var analysisResult = ( provider.Controller
                .GetMessageAnalysisResume( project.Id, medicalTeam.Id, message.MessageId ) as OkObjectResult )
                .Value as AnalysisResumeModel;

            Assert.NotNull( analysisResult );
            Assert.Equal( 1, analysisResult.AnalysisCount );
        }
    }
}
