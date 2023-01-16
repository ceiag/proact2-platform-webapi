using Microsoft.AspNetCore.Mvc;
using Proact.Services.Entities;
using Proact.Services.QueriesServices;
using System;
using Xunit;

namespace Proact.Services.UnitTests.ValidityCheckers.Users {
    public class DbUsersDbValidityCheckerUnitTests {
        [Fact]
        public void IfUserIsValidMustReturnOk() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var user = mockHelper.CreateDummyUser();
                User retrivedUser = null;

                var result = mockHelper.ConsistencyRulesHelper
                    .IfUserIsValid( user.Id, out retrivedUser )
                    .Then( () => {
                        return new OkObjectResult( retrivedUser );
                    } )
                    .ReturnResult();

                Assert.NotNull( result as OkObjectResult );
                Assert.NotNull( retrivedUser );
            }
        }

        [Fact]
        public void IfUserIsValidMustReturnNotFound() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var user = mockHelper.CreateDummyUser();
                User retrivedUser = null;

                var result = mockHelper.ConsistencyRulesHelper
                    .IfUserIsValid( Guid.NewGuid(), out retrivedUser )
                    .Then( () => {
                        return new OkObjectResult( retrivedUser );
                    } )
                    .ReturnResult();

                Assert.NotNull( result as NotFoundObjectResult );
                Assert.Null( retrivedUser );
            }
        }

        [Fact]
        public void IfUserAccountIsValidMustReturnOk() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var user = mockHelper.CreateDummyUser();
                User retrivedUser = null;

                var result = mockHelper.ConsistencyRulesHelper
                    .IfUserAccountIsValid( user.AccountId, out retrivedUser )
                    .Then( () => {
                        return new OkObjectResult( retrivedUser );
                    } )
                    .ReturnResult();

                Assert.NotNull( result as OkObjectResult );
                Assert.NotNull( retrivedUser );
            }
        }

        [Fact]
        public void IfUserAccountIsValidMustReturnNotFound() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var user = mockHelper.CreateDummyUser();
                User retrivedUser = null;

                var result = mockHelper.ConsistencyRulesHelper
                    .IfUserAccountIsValid( Guid.NewGuid().ToString(), out retrivedUser )
                    .Then( () => {
                        return new OkObjectResult( retrivedUser );
                    } )
                    .ReturnResult();

                Assert.NotNull( result as NotFoundObjectResult );
                Assert.Null( retrivedUser );
            }
        }
    }
}
