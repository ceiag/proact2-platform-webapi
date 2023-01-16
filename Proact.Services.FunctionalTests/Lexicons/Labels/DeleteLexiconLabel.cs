using Microsoft.AspNetCore.Mvc;
using Proact.Services.AuthorizationPolicies;
using Proact.Services.Entities;
using Proact.Services.Entities.MessageAnalysis;
using Proact.Services.Models;
using Proact.Services.Tests.Shared;
using System;
using Xunit;

namespace Proact.Services.FunctionalTests.Lexicons.Labels {
    public class DeleteLexiconLabel {
        private void AssertDeleteCorrectness(
            LexiconLabelsControllerProvider controller, Guid lexiconId, Guid categoryId, Guid labelId ) {
            var result = controller.Controller.GetLexiconLabel( lexiconId, categoryId, labelId );
            Assert.Equal( 404, ( result as NotFoundObjectResult ).StatusCode );
        }

        [Fact]
        public void DeleteLexiconLabel_MustReturn_Ok() {
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

            Guid labelId = lexicon.Categories[0].Labels[0].Id;

            var lexiconLabelController = new LexiconLabelsControllerProvider(
                servicesProvider, systemAdmin, Roles.SystemAdmin );
            var result = lexiconLabelController.Controller.DeleteLexiconLabel(
                lexicon.Id, lexicon.Categories[0].Id, lexicon.Categories[0].Labels[0].Id );

            Assert.Equal( 200, ( result as OkResult ).StatusCode );

            AssertDeleteCorrectness( lexiconLabelController, lexicon.Id, lexicon.Categories[0].Id, labelId );
        }
    }
}
