using Microsoft.AspNetCore.Mvc;
using Proact.Services.AuthorizationPolicies;
using Proact.Services.Entities;
using Proact.Services.Entities.MessageAnalysis;
using Proact.Services.Tests.Shared;
using System;
using Xunit;

namespace Proact.Services.FunctionalTests.Lexicons {
    public class DeleteLexicon {
        private void AssertDeleteCorrectness( LexiconControllerProvider controller, Guid lexiconId ) {
            var result = controller.Controller.GetLexicon( lexiconId );
            Assert.Equal( 404, ( result as NotFoundObjectResult ).StatusCode );
        }

        [Fact]
        public void DeleteLexicon_MustReturn_Ok() {
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

            var lexiconController = new LexiconControllerProvider( 
                servicesProvider, systemAdmin, Roles.SystemAdmin );
            var result = lexiconController.Controller.DeleteLexicon( lexicon.Id );

            Assert.Equal( 200, ( result as OkResult ).StatusCode );
            AssertDeleteCorrectness( lexiconController, lexicon.Id );
        }
    }
}
