using Microsoft.AspNetCore.Mvc;
using Proact.Services.Entities;
using Proact.Services.QueriesServices;
using System;
using System.Collections.Generic;
using Xunit;

namespace Proact.Services.UnitTests.ValidityCheckers.Patients {
    public class DbPatientsValidityCheckerUnitTests {
        [Fact]
        public void IfNurseIsValidMustReturnOk() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var project = mockHelper.CreateDummyProject();
                var medicalTeam = mockHelper.CreateDummyMedicalTeam( project );
                var user = mockHelper.CreateDummyUser();
                var patient = mockHelper.CreateDummyPatient( user );

                Patient patientRetrieved = null;

                var result = mockHelper.ConsistencyRulesHelper
                    .IfPatientIsValid( user.Id, out patientRetrieved )
                    .Then( () => {
                        return new OkObjectResult( patientRetrieved );
                    } )
                    .ReturnResult();

                Assert.NotNull( result as OkObjectResult );
                Assert.NotNull( patientRetrieved );
            }
        }

        [Fact]
        public void IfNurseIsValidMustReturnNotFound() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var project = mockHelper.CreateDummyProject();
                var medicalTeam = mockHelper.CreateDummyMedicalTeam( project );
                var user = mockHelper.CreateDummyUser();
                var patient = mockHelper.CreateDummyPatient( user );

                Patient patientRetrieved = null;

                var result = mockHelper.ConsistencyRulesHelper
                    .IfPatientIsValid( Guid.NewGuid(), out patientRetrieved )
                    .Then( () => {
                        return new OkObjectResult( patientRetrieved );
                    } )
                    .ReturnResult();

                Assert.NotNull( result as NotFoundObjectResult );
                Assert.Null( patientRetrieved );
            }
        }

        [Fact]
        public void IfPatientNotExistMustReturnTrue() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var project = mockHelper.CreateDummyProject();
                var medicalTeam = mockHelper.CreateDummyMedicalTeam( project );
                var user = mockHelper.CreateDummyUser();
                var patient = mockHelper.CreateDummyPatient( user );

                var result = mockHelper.ConsistencyRulesHelper
                    .IfPatientNotExist( Guid.NewGuid() )
                    .Then( () => {
                        return new OkObjectResult( patient );
                    } )
                    .ReturnResult();

                Assert.NotNull( result as OkObjectResult );
            }
        }

        [Fact]
        public void IfPatientNotExistMustReturnFalse() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var project = mockHelper.CreateDummyProject();
                var medicalTeam = mockHelper.CreateDummyMedicalTeam( project );
                var user = mockHelper.CreateDummyUser();
                var patient = mockHelper.CreateDummyPatient( user );

                var result = mockHelper.ConsistencyRulesHelper
                    .IfPatientNotExist( user.Id )
                    .Then( () => {
                        return new OkObjectResult( patient );
                    } )
                    .ReturnResult();

                Assert.NotNull( result as ConflictObjectResult );
            }
        }

        [Fact]
        public void IfPatientsAreValidMustReturnTrue() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var project = mockHelper.CreateDummyProject();
                var medicalTeam = mockHelper.CreateDummyMedicalTeam( project );
                var user_0 = mockHelper.CreateDummyUser();
                var patient_0 = mockHelper.CreateDummyPatient( user_0 );
                var user_1 = mockHelper.CreateDummyUser();
                var patient_1 = mockHelper.CreateDummyPatient( user_1 );

                var result = mockHelper.ConsistencyRulesHelper
                    .IfPatientsAreValid( new List<Guid> { user_0.Id, user_1.Id } )
                    .Then( () => {
                        return new OkObjectResult( patient_0 );
                    } )
                    .ReturnResult();

                Assert.NotNull( result as OkObjectResult );
            }
        }

        [Fact]
        public void IfPatientsAreValidMustReturnFalse() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var project = mockHelper.CreateDummyProject();
                var medicalTeam = mockHelper.CreateDummyMedicalTeam( project );
                var user_0 = mockHelper.CreateDummyUser();
                var patient_0 = mockHelper.CreateDummyPatient( user_0 );
                var user_1 = mockHelper.CreateDummyUser();
                var patient_1 = mockHelper.CreateDummyPatient( user_1 );

                var result = mockHelper.ConsistencyRulesHelper
                    .IfPatientsAreValid( new List<Guid> { user_0.Id, user_1.Id, Guid.NewGuid() } )
                    .Then( () => {
                        return new OkObjectResult( patient_0 );
                    } )
                    .ReturnResult();

                Assert.NotNull( result as NotFoundObjectResult );
            }
        }
    }
}
