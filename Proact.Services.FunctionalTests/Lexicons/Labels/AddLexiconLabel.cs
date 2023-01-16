using Microsoft.AspNetCore.Mvc;
using Proact.Services.AuthorizationPolicies;
using Proact.Services.Entities;
using Proact.Services.Entities.MessageAnalysis;
using Proact.Services.Models;
using Proact.Services.Tests.Shared;
using Xunit;

namespace Proact.Services.FunctionalTests.Lexicons.Labels {
    public class AddLexiconLabel {
        [Fact]
        public void AddLexiconLabel_MustReturn_Ok() {
            var servicesProvider = new ProactServicesProvider();
            Institute institute = null;
            Project project = null;
            Lexicon lexicon = null;
            User systemAdmin = null;

            new DatabaseSnapshotProvider( servicesProvider )
                .AddInstituteWithRandomValues( out institute )
                .AddProjectWithRandomValues( institute, out project )
                .AddLexiconWithRandomValues( institute, out lexicon )
                .AddUserWithRandomValues( institute, out systemAdmin );

            var addingLabelRequest = new LexiconLabelCreationRequest() {
                Label = "new label",
                GroupName = "new group name"
            };

            var lexiconLabelController = new LexiconLabelsControllerProvider(
                servicesProvider, systemAdmin, Roles.SystemAdmin );
            var result = lexiconLabelController.Controller
                .AddLexiconLabel( lexicon.Id, lexicon.Categories[0].Id, addingLabelRequest );

            var labelModel = ( result as OkObjectResult ).Value as LexiconLabelModel;

            Assert.Equal( addingLabelRequest.Label, labelModel.Label );
            Assert.Equal( addingLabelRequest.GroupName, labelModel.GroupName );
        }
    }
}
