using Microsoft.AspNetCore.Mvc;
using Proact.Services.Entities;
using Proact.Services.QueriesServices;
using System;
using Xunit;

namespace Proact.Services.UnitTests.ValidityCheckers.Nurses {
    public class DbNursesValidityCheckerUnitTests {
        [Fact]
        public void IfNurseIsValidMustReturnOk() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var project = mockHelper.CreateDummyProject();
                var medicalTeam = mockHelper.CreateDummyMedicalTeam( project );
                var user = mockHelper.CreateDummyUser();
                var nurse = mockHelper.CreateDummyNurse( user );

                Nurse nurseRetrieved = null;

                var result = mockHelper.ConsistencyRulesHelper
                    .IfNurseIsValid( user.Id, out nurseRetrieved )
                    .Then( () => {
                        return new OkObjectResult( nurseRetrieved );
                    } )
                    .ReturnResult();

                Assert.NotNull( result as OkObjectResult );
                Assert.NotNull( nurseRetrieved );
            }
        }

        [Fact]
        public void IfNurseIsValidMustReturnNotFound() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var project = mockHelper.CreateDummyProject();
                var medicalTeam = mockHelper.CreateDummyMedicalTeam( project );
                var user = mockHelper.CreateDummyUser();
                var nurse = mockHelper.CreateDummyNurse( user );

                Nurse nurseRetrieved = null;

                var result = mockHelper.ConsistencyRulesHelper
                    .IfNurseIsValid( Guid.NewGuid(), out nurseRetrieved )
                    .Then( () => {
                        return new OkObjectResult( nurseRetrieved );
                    } )
                    .ReturnResult();

                Assert.NotNull( result as NotFoundObjectResult );
                Assert.Null( nurseRetrieved );
            }
        }

        [Fact]
        public void IfNurseNotExistMustReturnTrue() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var project = mockHelper.CreateDummyProject();
                var medicalTeam = mockHelper.CreateDummyMedicalTeam( project );
                var user = mockHelper.CreateDummyUser();
                var nurse = mockHelper.CreateDummyNurse( user );

                var result = mockHelper.ConsistencyRulesHelper
                    .IfNurseNotExist( Guid.NewGuid() )
                    .Then( () => {
                        return new OkObjectResult( "" );
                    } )
                    .ReturnResult();

                Assert.NotNull( result as OkObjectResult );
            }
        }

        [Fact]
        public void IfNurseNotExistMustReturnConflict() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var project = mockHelper.CreateDummyProject();
                var medicalTeam = mockHelper.CreateDummyMedicalTeam( project );
                var user = mockHelper.CreateDummyUser();
                var nurse = mockHelper.CreateDummyNurse( user );

                var result = mockHelper.ConsistencyRulesHelper
                    .IfNurseNotExist( user.Id )
                    .Then( () => {
                        return new OkObjectResult( "" );
                    } )
                    .ReturnResult();

                Assert.NotNull( result as ConflictObjectResult );
            }
        }
    }
}
