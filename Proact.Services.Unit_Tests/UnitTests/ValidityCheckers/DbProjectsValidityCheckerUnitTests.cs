using Microsoft.AspNetCore.Mvc;
using Proact.Services.Entities;
using Proact.Services.QueriesServices;
using System;
using Xunit;

namespace Proact.Services.UnitTests.ValidityCheckers.Projects {
    public class DbProjectsValidityCheckerUnitTests {
        [Fact]
        public void IfNurseIsValidMustReturnOk() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var project = mockHelper.CreateDummyProject();

                Project projectRetrieved = null;

                var result = mockHelper.ConsistencyRulesHelper
                    .IfProjectIsValid( project.Id, out projectRetrieved )
                    .Then( () => {
                        return new OkObjectResult( projectRetrieved );
                    } )
                    .ReturnResult();

                Assert.NotNull( result as OkObjectResult );
                Assert.NotNull( projectRetrieved );
            }
        }

        [Fact]
        public void IfNurseIsValidMustReturnNotFound() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var project = mockHelper.CreateDummyProject();

                Project projectRetrieved = null;

                var result = mockHelper.ConsistencyRulesHelper
                    .IfProjectIsValid( Guid.NewGuid(), out projectRetrieved )
                    .Then( () => {
                        return new OkObjectResult( projectRetrieved );
                    } )
                    .ReturnResult();

                Assert.NotNull( result as NotFoundObjectResult );
                Assert.Null( projectRetrieved );
            }
        }

        [Fact]
        public void IfProjectIsOpenedMustReturnTrue() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var project = mockHelper.CreateDummyProject();

                var result = mockHelper.ConsistencyRulesHelper
                    .IfProjectIsOpen( project.Id )
                    .Then( () => {
                        return new OkObjectResult( true );
                    } )
                    .ReturnResult();

                Assert.NotNull( result as OkObjectResult );
            }
        }

        [Fact]
        public void IfProjectNameAvailableMustReturnTrue() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var project = mockHelper.CreateDummyProject();

                var result = mockHelper.ConsistencyRulesHelper
                    .IfProjectNameAvailable( Guid.NewGuid().ToString() )
                    .Then( () => {
                        return new OkObjectResult( true );
                    } )
                    .ReturnResult();

                Assert.NotNull( result as OkObjectResult );
            }
        }

        [Fact]
        public void IfProjectNameAvailableMustReturnConflict() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var project = mockHelper.CreateDummyProject();

                var result = mockHelper.ConsistencyRulesHelper
                    .IfProjectNameAvailable( project.Name )
                    .Then( () => {
                        return new OkObjectResult( true );
                    } )
                    .ReturnResult();

                Assert.NotNull( result as ConflictObjectResult );
            }
        }
    }
}
