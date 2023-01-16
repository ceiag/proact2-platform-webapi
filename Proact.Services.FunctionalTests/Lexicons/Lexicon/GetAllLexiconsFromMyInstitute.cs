using Microsoft.AspNetCore.Mvc;
using Proact.Services.AuthorizationPolicies;
using Proact.Services.Entities;
using Proact.Services.Entities.MessageAnalysis;
using Proact.Services.Models;
using Proact.Services.Tests.Shared;
using System.Collections.Generic;
using Xunit;

namespace Proact.Services.FunctionalTests.Lexicons {
    public class GetAllLexiconsFromMyInstitute {
        [Fact]
        public void GetLexicons_MustReturnOnlyInsideMyInstitute() {
            var servicesProvider = new ProactServicesProvider();
            Institute institute_0 = null;
            Institute institute_1 = null;
            Project project = null;
            Lexicon lexicon_0 = null;
            Lexicon lexicon_1 = null;
            Lexicon lexicon_2 = null;
            User systemAdmin = null;

            new DatabaseSnapshotProvider( servicesProvider )
                .AddInstituteWithRandomValues( out institute_0 )
                .AddInstituteWithRandomValues( out institute_1 )
                .AddProjectWithRandomValues( institute_0, out project )
                .AddLexiconWithRandomValues( institute_0, out lexicon_0 )
                .AddLexiconWithRandomValues( institute_0, out lexicon_1 )
                .AddLexiconWithRandomValues( institute_1, out lexicon_2 )
                .AddUserWithRandomValues( institute_0, out systemAdmin );

            var lexiconController = new LexiconControllerProvider(
                servicesProvider, systemAdmin, Roles.SystemAdmin );
            var lexicons = lexiconController.Controller.GetLexicons() as OkObjectResult;

            Assert.Equal( 2, (lexicons.Value as List<LexiconModel>).Count );
        }
    }
}
