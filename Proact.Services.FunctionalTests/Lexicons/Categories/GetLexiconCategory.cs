using Microsoft.AspNetCore.Mvc;
using Proact.Services.AuthorizationPolicies;
using Proact.Services.Entities;
using Proact.Services.Entities.MessageAnalysis;
using Proact.Services.Models;
using Proact.Services.Tests.Shared;
using Xunit;

namespace Proact.Services.FunctionalTests.Lexicons.Categories {
    public class GetLexiconCategory {
        [Fact]
        public void GetLexiconCategory_MustReturn_Ok() {
            var servicesProvider = new ProactServicesProvider();
            Institute institute = null;
            Project project = null;
            Lexicon lexicon = null;
            User instituteAdmin = null;

            new DatabaseSnapshotProvider( servicesProvider )
                .AddInstituteWithRandomValues( out institute )
                .AddProjectWithRandomValues( institute, out project )
                .AddLexiconWithRandomValues( institute, out lexicon )
                .AddUserWithRandomValues( institute, out instituteAdmin );

            var lexiconCategoryController = new LexiconCategoryControllerProvider(
                servicesProvider, instituteAdmin, Roles.SystemAdmin );
            var result = lexiconCategoryController.Controller
                .GetCategory( lexicon.Id, lexicon.Categories[0].Id );

            var categoryModel = ( result as OkObjectResult ).Value as LexiconCategoryModel;

            Assert.Equal( lexicon.Categories[0].Id, categoryModel.Id );
            Assert.Equal( lexicon.Categories[0].MultipleSelection, categoryModel.MultipleSelection );
            Assert.Equal( lexicon.Categories[0].Name, categoryModel.Name );
        }
    }
}
