using Microsoft.AspNetCore.Mvc;
using Proact.Services.Entities;
using Proact.Services.Tests.Shared;
using System;
using Xunit;

namespace Proact.Services.UnitTests.DbValidityCheckers.Projects {
    public class IfProjectIsInMyInstitute {
        [Fact]
        public void IfProjectIsInMyInstitute_ReturnTrue() {
            Institute institute = null;
            Project project = null;

            var servicesProvider = new ProactServicesProvider();
            new DatabaseSnapshotProvider( servicesProvider )
                .AddInstituteWithRandomValues( out institute )
                .AddProjectWithRandomValues( institute, out project );

            var result = servicesProvider.ConsistencyRulesHelper
                    .IfProjectIsInMyInstitute( institute.Id, project )
                    .Then( () => {
                        return new OkResult();
                    } )
                    .ReturnResult();

            Assert.NotNull( result as OkResult );
        }

        [Fact]
        public void IfProjectIsInMyInstitute_ReturnFalse() {
            Institute institute = null;
            Project project = null;

            var servicesProvider = new ProactServicesProvider();
            new DatabaseSnapshotProvider( servicesProvider )
                .AddInstituteWithRandomValues( out institute )
                .AddProjectWithRandomValues( institute, out project );

            var result = servicesProvider.ConsistencyRulesHelper
                    .IfProjectIsInMyInstitute( Guid.NewGuid(), project )
                    .Then( () => {
                        return new OkResult();
                    } )
                    .ReturnResult();

            Assert.NotNull( result as BadRequestObjectResult );
        }
    }
}
