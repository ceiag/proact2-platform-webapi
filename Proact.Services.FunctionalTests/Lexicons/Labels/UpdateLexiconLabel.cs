using Microsoft.AspNetCore.Mvc;
using Proact.Services.AuthorizationPolicies;
using Proact.Services.Entities;
using Proact.Services.Entities.MessageAnalysis;
using Proact.Services.Models;
using Proact.Services.Tests.Shared;
using System;
using Xunit;

namespace Proact.Services.FunctionalTests.Lexicons.Labels {
    public class UpdateLexiconLabel {
        private LexiconLabelUpdateRequest _updateLabelRequest = new LexiconLabelUpdateRequest() {
            Label = "new label",
            GroupName = "new group name"
        };

        private void AssertUpdateCorrectness( 
            LexiconLabelsControllerProvider controller, Guid lexiconId, Guid categoryId, Guid labelId ) {
            var result = controller.Controller.GetLexiconLabel( lexiconId, categoryId, labelId );
            var labelModel = ( result as OkObjectResult ).Value as LexiconLabelModel;

            Assert.Equal( _updateLabelRequest.Label, labelModel.Label );
            Assert.Equal( _updateLabelRequest.GroupName, labelModel.GroupName );
        }

        [Fact]
        public void UpdateLexiconLabel_MustReturn_Ok() {
            var servicesProvider = new ProactServicesProvider();
            Project project = null;
            Lexicon lexicon = null;
            User systemAdmin = null;
            Institute institute = null;

            new DatabaseSnapshotProvider( servicesProvider )
                .AddInstituteWithRandomValues( out institute )
                .AddProjectWithRandomValues( institute, out project )
                .AddLexiconWithRandomValues( institute, out lexicon )
                .AddUserWithRandomValues( institute, out systemAdmin );

            var lexiconLabelController = new LexiconLabelsControllerProvider(
                servicesProvider, systemAdmin, Roles.SystemAdmin );
            var result = lexiconLabelController.Controller.UpdateLexiconLabel( 
                lexicon.Id, lexicon.Categories[0].Id, lexicon.Categories[0].Labels[0].Id, _updateLabelRequest );

            var labelModel = ( result as OkObjectResult ).Value as LexiconLabelModel;

            Assert.Equal( _updateLabelRequest.Label, labelModel.Label );
            Assert.Equal( _updateLabelRequest.GroupName, labelModel.GroupName );

            AssertUpdateCorrectness( 
                lexiconLabelController, lexicon.Id, 
                lexicon.Categories[0].Id, lexicon.Categories[0].Labels[0].Id );
        }
    }
}
