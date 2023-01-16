using Microsoft.AspNetCore.Mvc;
using Proact.Services.AuthorizationPolicies;
using Proact.Services.Entities;
using Proact.Services.Entities.MessageAnalysis;
using Proact.Services.Models;
using Proact.Services.Tests.Shared;
using System;
using System.Collections.Generic;
using Xunit;

namespace Proact.Services.FunctionalTests.Lexicons.Categories {
    public class SetOrderingOfCategories {
        [Fact]
        public void SetOrderingOfLexiconCategories_MustReturn_Ok() {
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

            var categoryNewOrder = new LexiconCategorySetOrderingRequest() {
                OrderedCategories = new List<Guid> {
                    lexicon.Categories[2].Id,
                    lexicon.Categories[0].Id,
                    lexicon.Categories[1].Id
                }
            };

            var lexiconCategoryController = new LexiconCategoryControllerProvider(
                servicesProvider, instituteAdmin, Roles.SystemAdmin );
            var result = lexiconCategoryController.Controller
                .SetOrderingOfCategories( lexicon.Id, categoryNewOrder );

            Assert.Equal( 200, ( result as OkResult ).StatusCode );
            Assert.Equal( categoryNewOrder.OrderedCategories[0], lexicon.Categories[2].Id );
            Assert.Equal( categoryNewOrder.OrderedCategories[1], lexicon.Categories[0].Id );
            Assert.Equal( categoryNewOrder.OrderedCategories[2], lexicon.Categories[1].Id );
        }
    }
}
