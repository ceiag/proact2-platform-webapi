using Microsoft.AspNetCore.Mvc;
using Proact.Services.AuthorizationPolicies;
using Proact.Services.Entities;
using Proact.Services.Entities.MessageAnalysis;
using Proact.Services.QueriesServices;
using Proact.Services.Tests.Shared;
using System;
using System.Linq;
using Xunit;

namespace Proact.Services.FunctionalTests.Lexicons.Categories {
    public class DeleteLexiconCategory {
        private void AssertCategoryDeletedCorrectness( 
            ProactServicesProvider servicesProvider, Guid deletedCategoryId ) {
            var category = servicesProvider
                .GetQueriesService<ILexiconCategoriesQueriesService>()
                .Get( deletedCategoryId );

            Assert.Null( category );
        }

        [Fact]
        public void DeleteCategoryToLexicon_MustReturn_Ok() {
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

            Guid categoryId = lexicon.Categories[0].Id;

            var lexiconCategoryController = new LexiconCategoryControllerProvider(
                servicesProvider, instituteAdmin, Roles.SystemAdmin );
            var result = lexiconCategoryController.Controller.DeleteCategory( lexicon.Id, categoryId );

            Assert.Equal( 200, ( result as OkResult ).StatusCode );
            AssertCategoryDeletedCorrectness( servicesProvider, categoryId );
        }
    }
}
