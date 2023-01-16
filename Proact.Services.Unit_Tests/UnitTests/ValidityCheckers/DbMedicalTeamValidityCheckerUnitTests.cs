using Microsoft.AspNetCore.Mvc;
using Proact.Services.AuthorizationPolicies;
using Proact.Services.Entities;
using Proact.Services.QueriesServices;
using System;
using System.Collections.Generic;
using Xunit;

namespace Proact.Services.UnitTests.ValidityCheckers.MedicalTeams {
    public class DbMedicalTeamValidityCheckerUnitTests {
        [Fact]
        public void IfMedicalTeamIsValidMustReturnOk() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var project = mockHelper.CreateDummyProject();
                var medicalTeam = mockHelper.CreateDummyMedicalTeam( project );

                MedicalTeam medicalTeamRetrieved = null;

                var result = mockHelper.ConsistencyRulesHelper
                    .IfMedicalTeamIsValid( medicalTeam.Id, out medicalTeamRetrieved )
                    .Then( () => {
                        return new OkObjectResult( medicalTeam );
                    } )
                    .ReturnResult();

                Assert.NotNull( result as OkObjectResult );
                Assert.NotNull( medicalTeam );
            }
        }

        [Fact]
        public void IfMedicalTeamIsValidMustReturnNotFound() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var project = mockHelper.CreateDummyProject();
                var medicalTeam = mockHelper.CreateDummyMedicalTeam( project );

                MedicalTeam medicalTeamRetrieved = null;

                var result = mockHelper.ConsistencyRulesHelper
                    .IfMedicalTeamIsValid( Guid.NewGuid(), out medicalTeamRetrieved )
                    .Then( () => {
                        return new OkObjectResult( medicalTeam );
                    } )
                    .ReturnResult();

                Assert.NotNull( result as NotFoundObjectResult );
                Assert.Null( medicalTeamRetrieved );
            }
        }

        [Fact]
        public void IfMedicTeamNameAvailableMustReturnTrue() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var project = mockHelper.CreateDummyProject();
                var medicalTeam = mockHelper.CreateDummyMedicalTeam( project );

                var result = mockHelper.ConsistencyRulesHelper
                    .IfMedicTeamNameAvailable( Guid.NewGuid().ToString() )
                    .Then( () => {
                        return new OkObjectResult( medicalTeam.Name );
                    } )
                    .ReturnResult();

                Assert.NotNull( result as OkObjectResult );
            }
        }

        [Fact]
        public void IfMedicTeamNameAvailableMustReturnConflict() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var project = mockHelper.CreateDummyProject();
                var medicalTeam = mockHelper.CreateDummyMedicalTeam( project );

                var result = mockHelper.ConsistencyRulesHelper
                    .IfMedicTeamNameAvailable( medicalTeam.Name )
                    .Then( () => {
                        return new OkObjectResult( medicalTeam.Name );
                    } )
                    .ReturnResult();

                Assert.NotNull( result as ConflictObjectResult );
            }
        }

        [Fact]
        public void IfMedicIsIntoTheMedicalTeamReturnTrue() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var project = mockHelper.CreateDummyProject();
                var medicalTeam = mockHelper.CreateDummyMedicalTeam( project );
                var user = mockHelper.CreateDummyUser();
                var medic = mockHelper.CreateDummyMedic( user );

                mockHelper.ServicesProvider
                    .GetQueriesService<IMedicQueriesService>()
                    .AddToMedicalTeam( user.Id, medicalTeam.Id );

                mockHelper.ServicesProvider.SaveChanges();

                var userRoles = new UserRoles( new List<string>() { Roles.MedicalProfessional } );

                var result = mockHelper.ConsistencyRulesHelper
                    .IfMedicIsIntoTheMedicalTeam( user.Id, medicalTeam.Id, userRoles )
                    .Then( () => {
                        return new OkObjectResult( user.Id );
                    } )
                    .ReturnResult();

                Assert.NotNull( result as OkObjectResult );
            }
        }

        [Fact]
        public void IfMedicIsIntoTheMedicalTeamReturnFalse() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var project = mockHelper.CreateDummyProject();
                var medicalTeam = mockHelper.CreateDummyMedicalTeam( project );
                var user = mockHelper.CreateDummyUser();
                var medic = mockHelper.CreateDummyMedic( user );
                var userNotIntoMedicalTeam = mockHelper.CreateDummyUser();
                var medicNotInotMedicalTeam = mockHelper.CreateDummyMedic( userNotIntoMedicalTeam );

                mockHelper.ServicesProvider
                    .GetQueriesService<IMedicQueriesService>()
                    .AddToMedicalTeam( user.Id, medicalTeam.Id );

                mockHelper.ServicesProvider.SaveChanges();

                var userRoles = new UserRoles( new List<string>() { Roles.MedicalProfessional } );

                var result = mockHelper.ConsistencyRulesHelper
                    .IfMedicIsIntoTheMedicalTeam( userNotIntoMedicalTeam.Id, medicalTeam.Id, userRoles )
                    .Then( () => {
                        return new OkObjectResult( user.Id );
                    } )
                    .ReturnResult();

                Assert.NotNull( result as UnauthorizedObjectResult );
            }
        }

        [Fact]
        public void IfMedicIsNotIntoTheMedicalTeamReturnTrue() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var project = mockHelper.CreateDummyProject();
                var medicalTeam = mockHelper.CreateDummyMedicalTeam( project );
                var user = mockHelper.CreateDummyUser();
                var medic = mockHelper.CreateDummyMedic( user );
                var userNotIntoMedicalTeam = mockHelper.CreateDummyUser();
                var medicNotInotMedicalTeam = mockHelper.CreateDummyMedic( userNotIntoMedicalTeam );

                mockHelper.ServicesProvider
                    .GetQueriesService<IMedicQueriesService>()
                    .AddToMedicalTeam( user.Id, medicalTeam.Id );

                mockHelper.ServicesProvider.SaveChanges();

                var result = mockHelper.ConsistencyRulesHelper
                    .IfMedicIsNotIntoTheMedicalTeam( userNotIntoMedicalTeam.Id, medicalTeam.Id )
                    .Then( () => {
                        return new OkObjectResult( user.Id );
                    } )
                    .ReturnResult();

                Assert.NotNull( result as OkObjectResult );
            }
        }

        [Fact]
        public void IfMedicIsNotIntoTheMedicalTeamReturnConflict() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var project = mockHelper.CreateDummyProject();
                var medicalTeam = mockHelper.CreateDummyMedicalTeam( project );
                var user = mockHelper.CreateDummyUser();
                var medic = mockHelper.CreateDummyMedic( user );

                mockHelper.ServicesProvider
                    .GetQueriesService<IMedicQueriesService>()
                    .AddToMedicalTeam( user.Id, medicalTeam.Id );

                mockHelper.ServicesProvider.SaveChanges();

                var result = mockHelper.ConsistencyRulesHelper
                    .IfMedicIsNotIntoTheMedicalTeam( user.Id, medicalTeam.Id )
                    .Then( () => {
                        return new OkObjectResult( user.Id );
                    } )
                    .ReturnResult();

                Assert.NotNull( result as ConflictObjectResult );
            }
        }

        [Fact]
        public void IfMedicIsNotAlreadyMedicalTeamAdminReturnTrue() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var project = mockHelper.CreateDummyProject();
                var medicalTeam = mockHelper.CreateDummyMedicalTeam( project );
                var user = mockHelper.CreateDummyUser();
                var medic = mockHelper.CreateDummyMedic( user );

                mockHelper.ServicesProvider
                    .GetQueriesService<IMedicQueriesService>()
                    .AddToMedicalTeam( user.Id, medicalTeam.Id );

                mockHelper.ServicesProvider.SaveChanges();

                var result = mockHelper.ConsistencyRulesHelper
                    .IfMedicIsNotAlreadyMedicalTeamAdmin( user.Id, medicalTeam.Id )
                    .Then( () => {
                        return new OkObjectResult( user.Id );
                    } )
                    .ReturnResult();

                Assert.NotNull( result as OkObjectResult );
            }
        }

        [Fact]
        public void IfMedicIsNotAlreadyMedicalTeamAdminReturnConflict() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var project = mockHelper.CreateDummyProject();
                var medicalTeam = mockHelper.CreateDummyMedicalTeam( project );
                var user = mockHelper.CreateDummyUser();
                var medic = mockHelper.CreateDummyMedic( user );

                mockHelper.ServicesProvider
                    .GetQueriesService<IMedicQueriesService>()
                    .AddToMedicalTeam( user.Id, medicalTeam.Id );

                mockHelper.ServicesProvider.SaveChanges();

                mockHelper.ServicesProvider
                    .GetQueriesService<IMedicalTeamQueriesService>()
                    .AddAdmin( medicalTeam.Id, user.Id );

                mockHelper.ServicesProvider.SaveChanges();

                var result = mockHelper.ConsistencyRulesHelper
                    .IfMedicIsNotAlreadyMedicalTeamAdmin( user.Id, medicalTeam.Id )
                    .Then( () => {
                        return new OkObjectResult( user.Id );
                    } )
                    .ReturnResult();

                Assert.NotNull( result as ConflictObjectResult );
            }
        }

        [Fact]
        public void IfMedicIsMedicalTeamAdminReturnTrue() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var project = mockHelper.CreateDummyProject();
                var medicalTeam = mockHelper.CreateDummyMedicalTeam( project );
                var user = mockHelper.CreateDummyUser();
                var medic = mockHelper.CreateDummyMedic( user );

                mockHelper.ServicesProvider
                    .GetQueriesService<IMedicQueriesService>()
                    .AddToMedicalTeam( user.Id, medicalTeam.Id );

                mockHelper.ServicesProvider.SaveChanges();

                mockHelper.ServicesProvider
                    .GetQueriesService<IMedicalTeamQueriesService>()
                    .AddAdmin( medicalTeam.Id, user.Id );

                mockHelper.ServicesProvider.SaveChanges();

                var result = mockHelper.ConsistencyRulesHelper
                    .IfMedicIsMedicalTeamAdmin( user.Id, medicalTeam.Id )
                    .Then( () => {
                        return new OkObjectResult( user.Id );
                    } )
                    .ReturnResult();

                Assert.NotNull( result as OkObjectResult );
            }
        }

        [Fact]
        public void IfMedicIsMedicalTeamAdminReturnBadRequest() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var project = mockHelper.CreateDummyProject();
                var medicalTeam = mockHelper.CreateDummyMedicalTeam( project );
                var user = mockHelper.CreateDummyUser();
                var medic = mockHelper.CreateDummyMedic( user );

                mockHelper.ServicesProvider
                    .GetQueriesService<IMedicQueriesService>()
                    .AddToMedicalTeam( user.Id, medicalTeam.Id );

                mockHelper.ServicesProvider.SaveChanges();

                var result = mockHelper.ConsistencyRulesHelper
                    .IfMedicIsMedicalTeamAdmin( user.Id, medicalTeam.Id )
                    .Then( () => {
                        return new OkObjectResult( user.Id );
                    } )
                    .ReturnResult();

                Assert.NotNull( result as BadRequestObjectResult );
            }
        }

        [Fact]
        public void IfCanAssignOtherUserToMedicalTeamTrue() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var project = mockHelper.CreateDummyProject();
                var medicalTeam = mockHelper.CreateDummyMedicalTeam( project );
                var user = mockHelper.CreateDummyUser();
                var medic = mockHelper.CreateDummyMedic( user );

                mockHelper.ServicesProvider
                    .GetQueriesService<IMedicQueriesService>()
                    .AddToMedicalTeam( user.Id, medicalTeam.Id );

                mockHelper.ServicesProvider.SaveChanges();

                mockHelper.ServicesProvider
                    .GetQueriesService<IMedicalTeamQueriesService>()
                    .AddAdmin( medicalTeam.Id, user.Id );

                mockHelper.ServicesProvider.SaveChanges();

                var userRoles = new UserRoles( new List<string>() { Roles.MedicalTeamAdmin } );

                var result = mockHelper.ConsistencyRulesHelper
                    .IfIHavePermissionsToAssignUserToThisMedicalTeam( user.Id, medicalTeam.Id, userRoles )
                    .Then( () => {
                        return new OkObjectResult( user.Id );
                    } )
                    .ReturnResult();

                Assert.NotNull( result as OkObjectResult );
            }
        }

        [Fact]
        public void IfCanAssignOtherUserToMedicalTeamUnauthorized() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var project = mockHelper.CreateDummyProject();
                var medicalTeam = mockHelper.CreateDummyMedicalTeam( project );
                var user = mockHelper.CreateDummyUser();
                var medic = mockHelper.CreateDummyMedic( user );

                mockHelper.ServicesProvider
                    .GetQueriesService<IMedicQueriesService>()
                    .AddToMedicalTeam( user.Id, medicalTeam.Id );

                mockHelper.ServicesProvider.SaveChanges();

                var userRoles = new UserRoles( new List<string>() { Roles.MedicalTeamAdmin } );

                var result = mockHelper.ConsistencyRulesHelper
                    .IfIHavePermissionsToAssignUserToThisMedicalTeam( user.Id, medicalTeam.Id, userRoles )
                    .Then( () => {
                        return new OkObjectResult( user.Id );
                    } )
                    .ReturnResult();

                Assert.NotNull( result as UnauthorizedObjectResult );
            }
        }

        [Fact]
        public void IfMedicAdminIsValidMustReturnTrue() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var project = mockHelper.CreateDummyProject();
                var medicalTeam = mockHelper.CreateDummyMedicalTeam( project );
                var user = mockHelper.CreateDummyUser();
                var medic = mockHelper.CreateDummyMedic( user );

                mockHelper.ServicesProvider
                    .GetQueriesService<IMedicQueriesService>()
                    .AddToMedicalTeam( user.Id, medicalTeam.Id );

                mockHelper.ServicesProvider.SaveChanges();

                mockHelper.ServicesProvider
                    .GetQueriesService<IMedicalTeamQueriesService>()
                    .AddAdmin( medicalTeam.Id, user.Id );

                mockHelper.ServicesProvider.SaveChanges();

                MedicAdmin medicAdmin = null;

                var result = mockHelper.ConsistencyRulesHelper
                    .IfMedicAdminIsValid( user.Id, out medicAdmin )
                    .Then( () => {
                        return new OkObjectResult( user.Id );
                    } )
                    .ReturnResult();

                Assert.NotNull( result as OkObjectResult );
            }
        }

        [Fact]
        public void IfMedicAdminIsValidMustReturnUnauthorized() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var project = mockHelper.CreateDummyProject();
                var medicalTeam = mockHelper.CreateDummyMedicalTeam( project );
                var user = mockHelper.CreateDummyUser();
                var medic = mockHelper.CreateDummyMedic( user );

                mockHelper.ServicesProvider
                    .GetQueriesService<IMedicQueriesService>()
                    .AddToMedicalTeam( user.Id, medicalTeam.Id );

                mockHelper.ServicesProvider.SaveChanges();

                MedicAdmin medicAdmin = null;

                var result = mockHelper.ConsistencyRulesHelper
                    .IfMedicAdminIsValid( user.Id, out medicAdmin )
                    .Then( () => {
                        return new OkObjectResult( user.Id );
                    } )
                    .ReturnResult();

                Assert.NotNull( result as UnauthorizedObjectResult );
            }
        }

        [Fact]
        public void IfMedicNotExistMustReturnTrue() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var project = mockHelper.CreateDummyProject();
                var medicalTeam = mockHelper.CreateDummyMedicalTeam( project );
                var user = mockHelper.CreateDummyUser();

                mockHelper.ServicesProvider.SaveChanges();

                var result = mockHelper.ConsistencyRulesHelper
                    .IfMedicNotExist( Guid.NewGuid() )
                    .Then( () => {
                        return new OkObjectResult( user.Id );
                    } )
                    .ReturnResult();

                Assert.NotNull( result as OkObjectResult );
            }
        }

        [Fact]
        public void IfMedicNotExistMustReturnConflict() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var project = mockHelper.CreateDummyProject();
                var medicalTeam = mockHelper.CreateDummyMedicalTeam( project );
                var user = mockHelper.CreateDummyUser();
                var medic = mockHelper.CreateDummyMedic( user );

                mockHelper.ServicesProvider.SaveChanges();

                var result = mockHelper.ConsistencyRulesHelper
                    .IfMedicNotExist( user.Id )
                    .Then( () => {
                        return new OkObjectResult( user.Id );
                    } )
                    .ReturnResult();

                Assert.NotNull( result as ConflictObjectResult );
            }
        }

        [Fact]
        public void IfMedicalTeamHasAlmostTwoOrMoreAdminsMustReturnTrue() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var project = mockHelper.CreateDummyProject();
                var medicalTeam = mockHelper.CreateDummyMedicalTeam( project );
                var user = mockHelper.CreateDummyUser();
                var medic = mockHelper.CreateDummyMedic( user );
                var user_1 = mockHelper.CreateDummyUser();
                var medic_user_1 = mockHelper.CreateDummyMedic( user_1 );

                mockHelper.ServicesProvider.SaveChanges();

                mockHelper.ServicesProvider
                    .GetQueriesService<IMedicalTeamQueriesService>()
                    .AddAdmin( medicalTeam.Id, user.Id );

                mockHelper.ServicesProvider.SaveChanges();

                mockHelper.ServicesProvider
                    .GetQueriesService<IMedicalTeamQueriesService>()
                    .AddAdmin( medicalTeam.Id, user_1.Id );

                mockHelper.ServicesProvider.SaveChanges();

                var result = mockHelper.ConsistencyRulesHelper
                    .IfMedicalTeamHasAlmostTwoOrMoreAdmins( medicalTeam.Id )
                    .Then( () => {
                        return new OkObjectResult( medicalTeam.Id );
                    } )
                    .ReturnResult();

                Assert.NotNull( result as OkObjectResult );
            }
        }

        [Fact]
        public void IfMedicalTeamHasAlmostTwoOrMoreAdminsMustReturnBadRequest() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var project = mockHelper.CreateDummyProject();
                var medicalTeam = mockHelper.CreateDummyMedicalTeam( project );
                var user = mockHelper.CreateDummyUser();
                var medic = mockHelper.CreateDummyMedic( user );
                var user_1 = mockHelper.CreateDummyUser();
                var medic_user_1 = mockHelper.CreateDummyMedic( user_1 );

                mockHelper.ServicesProvider.SaveChanges();

                mockHelper.ServicesProvider
                    .GetQueriesService<IMedicalTeamQueriesService>()
                    .AddAdmin( medicalTeam.Id, user.Id );

                mockHelper.ServicesProvider.SaveChanges();

                var result = mockHelper.ConsistencyRulesHelper
                    .IfMedicalTeamHasAlmostTwoOrMoreAdmins( medicalTeam.Id )
                    .Then( () => {
                        return new OkObjectResult( medicalTeam.Id );
                    } )
                    .ReturnResult();

                Assert.NotNull( result as BadRequestObjectResult );
            }
        }

        [Fact]
        public void IfMedicAndPatientAreIntoSameMedicalTeamMustReturnTrue() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var project = mockHelper.CreateDummyProject();
                var medicalTeam = mockHelper.CreateDummyMedicalTeam( project );
                var user_medic = mockHelper.CreateDummyUser();
                var medic = mockHelper.CreateDummyMedic( user_medic );
                var user_patient = mockHelper.CreateDummyUser();
                var patient = mockHelper.CreateDummyPatient( user_patient );

                mockHelper.ServicesProvider
                    .GetQueriesService<IMedicQueriesService>()
                    .AddToMedicalTeam( user_medic.Id, medicalTeam.Id );

                mockHelper.ServicesProvider.SaveChanges();

                mockHelper.ServicesProvider
                    .GetQueriesService<IPatientQueriesService>()
                    .AddToMedicalTeam( user_patient.Id, medicalTeam.Id );

                mockHelper.ServicesProvider.SaveChanges();

                var result = mockHelper.ConsistencyRulesHelper
                    .IfMedicAndPatientAreIntoSameMedicalTeam( user_medic.Id, user_patient.Id )
                    .Then( () => {
                        return new OkObjectResult( user_medic.Id );
                    } )
                    .ReturnResult();

                Assert.NotNull( result as OkObjectResult );
            }
        }

        [Fact]
        public void IfMedicAndPatientAreIntoSameMedicalTeamMustReturnUnauthorized() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var project = mockHelper.CreateDummyProject();
                var medicalTeam_0 = mockHelper.CreateDummyMedicalTeam( project );
                var medicalTeam_1 = mockHelper.CreateDummyMedicalTeam( project );
                var user_medic = mockHelper.CreateDummyUser();
                var medic = mockHelper.CreateDummyMedic( user_medic );
                var user_patient = mockHelper.CreateDummyUser();
                var patient = mockHelper.CreateDummyPatient( user_patient );

                mockHelper.ServicesProvider
                    .GetQueriesService<IMedicQueriesService>()
                    .AddToMedicalTeam( user_medic.Id, medicalTeam_0.Id );

                mockHelper.ServicesProvider.SaveChanges();

                mockHelper.ServicesProvider
                    .GetQueriesService<IPatientQueriesService>()
                    .AddToMedicalTeam( user_patient.Id, medicalTeam_1.Id );

                mockHelper.ServicesProvider.SaveChanges();

                var result = mockHelper.ConsistencyRulesHelper
                    .IfMedicAndPatientAreIntoSameMedicalTeam( user_medic.Id, user_patient.Id )
                    .Then( () => {
                        return new OkObjectResult( user_medic.Id );
                    } )
                    .ReturnResult();

                Assert.NotNull( result as UnauthorizedObjectResult );
            }
        }

        [Fact]
        public void IfMedicalTeamIsClosedMustReturnTrue() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var project = mockHelper.CreateDummyProject();
                var medicalTeam_0 = mockHelper.CreateDummyMedicalTeam( project );

                mockHelper.ServicesProvider.SaveChanges();

                var result = mockHelper.ConsistencyRulesHelper
                    .IfMedicalTeamIsOpen( medicalTeam_0.Id )
                    .Then( () => {
                        return new OkObjectResult( "" );
                    } )
                    .ReturnResult();

                Assert.NotNull( result as OkObjectResult );
            }
        }

        [Fact]
        public void IfMedicalTeamIsClosedMustReturnFalse() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var project = mockHelper.CreateDummyProject();
                var medicalTeam_0 = mockHelper.CreateDummyMedicalTeam( project );

                mockHelper.ServicesProvider
                    .GetQueriesService<IMedicalTeamQueriesService>()
                    .Close( medicalTeam_0.Id );

                mockHelper.ServicesProvider.SaveChanges();

                var result = mockHelper.ConsistencyRulesHelper
                    .IfMedicalTeamIsOpen( medicalTeam_0.Id )
                    .Then( () => {
                        return new OkObjectResult( "" );
                    } )
                    .ReturnResult();

                Assert.NotNull( result as BadRequestObjectResult );
            }
        }
    }
}
