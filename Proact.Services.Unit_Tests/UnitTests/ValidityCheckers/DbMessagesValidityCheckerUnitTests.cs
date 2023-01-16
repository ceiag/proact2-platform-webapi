using Microsoft.AspNetCore.Mvc;
using Proact.Services.AuthorizationPolicies;
using Proact.Services.Entities;
using Proact.Services.QueriesServices;
using System;
using System.Collections.Generic;
using Xunit;

namespace Proact.Services.UnitTests.ValidityCheckers.Messages {
    public class DbMessagesValidityCheckerUnitTests {
        [Fact]
        public void IfMessageIsValidMustReturnOk() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var project = mockHelper.CreateDummyProject();
                var medicalTeam = mockHelper.CreateDummyMedicalTeam( project );
                var user = mockHelper.CreateDummyUser();
                var message = mockHelper.CreateDummyNewTopicMessage( user, medicalTeam );

                Message messageRetrieved = null;

                var result = mockHelper.ConsistencyRulesHelper
                    .IfMessageIsValid( message.MessageId, out messageRetrieved )
                    .Then( () => {
                        return new OkObjectResult( message );
                    } )
                    .ReturnResult();

                Assert.NotNull( result as OkObjectResult );
                Assert.NotNull( messageRetrieved );
            }
        }

        [Fact]
        public void IfMessageIsValidMustReturnNotFound() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var project = mockHelper.CreateDummyProject();
                var medicalTeam = mockHelper.CreateDummyMedicalTeam( project );
                var user = mockHelper.CreateDummyUser();
                var message = mockHelper.CreateDummyNewTopicMessage( user, medicalTeam );

                Message messageRetrieved = null;

                var result = mockHelper.ConsistencyRulesHelper
                    .IfMessageIsValid( Guid.NewGuid(), out messageRetrieved )
                    .Then( () => {
                        return new OkObjectResult( message );
                    } )
                    .ReturnResult();

                Assert.NotNull( result as NotFoundObjectResult );
                Assert.Null( messageRetrieved );
            }
        }

        [Fact]
        public void IfMessageCanBeRepliedByMedicMustReturnTrue_WithPatientRole() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var project = mockHelper.CreateDummyProject();
                var medicalTeam = mockHelper.CreateDummyMedicalTeam( project );
                var user = mockHelper.CreateDummyUser();
                var message = mockHelper.CreateDummyNewTopicMessage( user, medicalTeam );

                var userRoles = new UserRoles( new List<string>() { Roles.Patient } );

                var result = mockHelper.ConsistencyRulesHelper
                    .IfMessageCanBeRepliedByMedic( message.MessageId, userRoles )
                    .Then( () => {
                        return new OkObjectResult( message );
                    } )
                    .ReturnResult();

                Assert.NotNull( result as OkObjectResult );
            }
        }

        [Fact]
        public void IfMessageCanBeRepliedByMedicMustReturnTrue_WithMedicRole() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var project = mockHelper.CreateDummyProject();
                var projectProps = mockHelper.CreateDummyProjectProperties( project, true, 0, 0, 0 );
                var medicalTeam = mockHelper.CreateDummyMedicalTeam( project );
                var user = mockHelper.CreateDummyUser();
                var message = mockHelper.CreateDummyNewTopicMessage( user, medicalTeam );

                var userRoles = new UserRoles( new List<string>() { Roles.MedicalProfessional } );

                var result = mockHelper.ConsistencyRulesHelper
                    .IfMessageCanBeRepliedByMedic( message.MessageId, userRoles )
                    .Then( () => {
                        return new OkObjectResult( message );
                    } )
                    .ReturnResult();

                Assert.NotNull( result as OkObjectResult );
            }
        }

        [Fact]
        public void IfMessageCanBeRepliedByMedicMustReturnFalse_WithMedicRole() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var project = mockHelper.CreateDummyProject();
                var projectProps = mockHelper.CreateDummyProjectProperties( project, true, 0, 0, 1 );
                var medicalTeam = mockHelper.CreateDummyMedicalTeam( project );
                var user = mockHelper.CreateDummyUser();
                var message = mockHelper.CreateDummyNewTopicMessage( user, medicalTeam );

                var userRoles = new UserRoles( new List<string>() { Roles.MedicalProfessional } );

                var result = mockHelper.ConsistencyRulesHelper
                    .IfMessageCanBeRepliedByMedic( message.MessageId, userRoles )
                    .Then( () => {
                        return new OkObjectResult( message );
                    } )
                    .ReturnResult();

                Assert.NotNull( result as BadRequestObjectResult );
            }
        }

        [Fact]
        public void IfMessageCanBeDeleted_MustReturnTrue() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var project = mockHelper.CreateDummyProject();
                var projectProps = mockHelper.CreateDummyProjectProperties( project, true, 0, 1, 0 );
                var medicalTeam = mockHelper.CreateDummyMedicalTeam( project );
                var user = mockHelper.CreateDummyUser();
                var message = mockHelper.CreateDummyNewTopicMessage( user, medicalTeam );

                var userRoles = new UserRoles( new List<string>() { Roles.Patient } );

                var result = mockHelper.ConsistencyRulesHelper
                    .IfMessageCanBeDeleted( message.MessageId )
                    .Then( () => {
                        return new OkObjectResult( message );
                    } )
                    .ReturnResult();

                Assert.NotNull( result as OkObjectResult );
            }
        }

        [Fact]
        public void IfMessageCanBeDeleted_MustReturnFalse() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var project = mockHelper.CreateDummyProject();
                var projectProps = mockHelper.CreateDummyProjectProperties( project, true, 0, 0, 0 );
                var medicalTeam = mockHelper.CreateDummyMedicalTeam( project );
                var user = mockHelper.CreateDummyUser();
                var message = mockHelper.CreateDummyNewTopicMessage( user, medicalTeam );

                var userRoles = new UserRoles( new List<string>() { Roles.Patient } );

                var result = mockHelper.ConsistencyRulesHelper
                    .IfMessageCanBeDeleted( message.MessageId )
                    .Then( () => {
                        return new OkObjectResult( message );
                    } )
                    .ReturnResult();

                Assert.NotNull( result as BadRequestObjectResult );
            }
        }
    }
}
