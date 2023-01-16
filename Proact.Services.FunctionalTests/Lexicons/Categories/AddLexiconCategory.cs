using Microsoft.AspNetCore.Mvc;
using Proact.Services.AuthorizationPolicies;
using Proact.Services.Entities;
using Proact.Services.Entities.MessageAnalysis;
using Proact.Services.Models;
using Proact.Services.Tests.Shared;
using Xunit;

namespace Proact.Services.FunctionalTests.Lexicons.Categories {
    public class AddLexiconCategory {
        [Fact]
        public void AddNewCategoryToLexicon_MustReturn_Ok() {
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

            var addingCategoryRequest = new LexiconCategoryAdditionRequest() {
                Name = "new category name",
                MultipleSelection = true
            };

            var lexiconCategoryController = new LexiconCategoryControllerProvider(
                servicesProvider, instituteAdmin, Roles.SystemAdmin );
            var result = lexiconCategoryController.Controller
                .AddLexiconCategory( lexicon.Id, addingCategoryRequest );

            var categoryModel = ( result as OkObjectResult ).Value as LexiconCategoryModel;

            Assert.Equal( addingCategoryRequest.Name, categoryModel.Name );
            Assert.Equal( addingCategoryRequest.MultipleSelection, categoryModel.MultipleSelection );
        }
    }
}
