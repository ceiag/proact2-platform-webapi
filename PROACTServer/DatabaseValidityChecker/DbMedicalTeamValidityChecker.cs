using Microsoft.AspNetCore.Mvc;
using Proact.Services.AuthorizationPolicies;
using Proact.Services.Entities;
using Proact.Services.Models;
using Proact.Services.QueriesServices.DataManagers;
using System;
using System.Linq;

namespace Proact.Services.QueriesServices {
    public static class DbMedicalTeamValidityChecker {
        public static ConsistencyRulesHelper IfMedicalTeamIsValid(
            this ConsistencyRulesHelper rulesHelper, Guid medicalTeamId, out MedicalTeam medicalTeam ) {
            MedicalTeam medicalTeamResult = null;

            var validityChecker = rulesHelper.CheckIf(
                () => {
                    medicalTeamResult = rulesHelper
                        .GetQueriesService<IMedicalTeamQueriesService>().Get( medicalTeamId );

                    return medicalTeamResult != null;
                },
                () => {
                    return new OkObjectResult( medicalTeamResult );
                },
                () => {
                    return new NotFoundObjectResult( $"medicalTeam with id {medicalTeamId} not found!" );
                } );

            medicalTeam = medicalTeamResult;
            return validityChecker;
        }

        public static ConsistencyRulesHelper IfMedicTeamNameAvailable(
            this ConsistencyRulesHelper rulesHelper, string name ) {

            var validityChecker = rulesHelper.CheckIf(
                () => {
                    return rulesHelper
                        .GetQueriesService<IMedicalTeamQueriesService>().IsNameAvailable( name );
                },
                () => {
                    return new OkObjectResult( name );
                },
                () => {
                    return new ConflictObjectResult( $"medical team name already taken" );
                } );

            return validityChecker;
        }

        public static ConsistencyRulesHelper IfMedicalTeamIsOpen(
            this ConsistencyRulesHelper rulesHelper, Guid medicalTeamId ) {

            var validityChecker = rulesHelper.CheckIf(
                () => {
                    return rulesHelper.GetQueriesService<IMedicalTeamQueriesService>()
                        .Get( medicalTeamId ).State == MedicalTeamState.Open;
                },
                () => {
                    return new OkObjectResult( "" );
                },
                () => {
                    return new BadRequestObjectResult(
                        new ErrorModel( ErrorCode.MedicalTeamClosed, "This medical team is closed!" ) );
                } );

            return validityChecker;
        }

        public static ConsistencyRulesHelper IfNameAvailableForExistingMedicalTeam(
            this ConsistencyRulesHelper rulesHelper, string name, Guid medicalTeamId ) {

            var validityChecker = rulesHelper.CheckIf(
                () => {
                    var medicalTeam = rulesHelper
                        .GetQueriesService<IMedicalTeamQueriesService>().Get( medicalTeamId );

                    return medicalTeam.Name == name || rulesHelper
                        .GetQueriesService<IMedicalTeamQueriesService>().IsNameAvailable( name );
                },
                () => {
                    return new OkObjectResult( name );
                },
                () => {
                    return new ConflictObjectResult( "this name is already taken" );
                } );

            return validityChecker;
        }

        public static ConsistencyRulesHelper IfProfessionistIsIntoTheMedicalTeam(
            this ConsistencyRulesHelper rulesHelper, Guid userId, Guid medicalTeamId, UserRoles userRoles ) {

            var validityChecker = rulesHelper.CheckIf(
                () => {
                    if ( userRoles.HasRoleOf( Roles.Nurse ) ) {
                        return rulesHelper.GetQueriesService<INurseQueriesService>()
                            .IsIntoMedicalTeam( userId, medicalTeamId );
                    }
                    else if ( userRoles.HasRoleOf( Roles.Researcher ) ) {
                        return rulesHelper.GetQueriesService<IResearcherQueriesService>()
                            .IsIntoMedicalTeam( userId, medicalTeamId );
                    }
                    else if ( userRoles.HasRoleOf( Roles.MedicalProfessional )
                                || userRoles.HasRoleOf( Roles.MedicalTeamAdmin ) ) {
                        return rulesHelper.GetQueriesService<IMedicQueriesService>()
                            .IsIntoMedicalTeam( userId, medicalTeamId );
                    }

                    return false;
                },
                () => {
                    return new OkObjectResult( userId );
                },
                () => {
                    return new UnauthorizedObjectResult( "You are not in this medical team" );
                } );

            return validityChecker;
        }

        public static ConsistencyRulesHelper IfMedicIsAdminOfMedicalTeamInProject(
            this ConsistencyRulesHelper rulesHelper, Guid userId, Guid projectId, UserRoles userRoles ) {

            var validityChecker = rulesHelper.CheckIf(
                () => {
                    if ( userRoles.HasRoleOf( Roles.MedicalTeamAdmin ) ) {
                        return rulesHelper.GetQueriesService<IMedicQueriesService>()
                            .IsMedicIntoProject( userId, projectId );
                    }

                    return false;
                },
                () => {
                    return new OkObjectResult( userId );
                },
                () => {
                    return new UnauthorizedObjectResult( "You are not admin of this medical team" );
                } );

            return validityChecker;
        }

        public static ConsistencyRulesHelper IfMedicAndPatientAreIntoSameMedicalTeam(
            this ConsistencyRulesHelper rulesHelper,
            Guid medicUserId, Guid patientUserId, UserRoles role ) {

            var validityChecker = rulesHelper.CheckIf(
                () => {
                    var patient = rulesHelper
                        .GetQueriesService<IPatientQueriesService>().Get( patientUserId );
                    
                    if ( role.HasRoleOf( Roles.Researcher ) ) {
                        return rulesHelper
                            .GetQueriesService<IResearcherQueriesService>()
                            .IsIntoMedicalTeam( medicUserId, (Guid)patient.MedicalTeamId );
                    }
                    else if ( role.HasRoleOf( Roles.Nurse ) ) {
                        return rulesHelper
                            .GetQueriesService<INurseQueriesService>()
                            .IsIntoMedicalTeam( medicUserId, (Guid)patient.MedicalTeamId );
                    }

                    return rulesHelper
                        .GetQueriesService<IMedicQueriesService>()
                        .IsIntoMedicalTeam( medicUserId, (Guid)patient.MedicalTeamId );

                },
                () => {
                    return new OkObjectResult( "" );
                },
                () => {
                    return new UnauthorizedObjectResult( 
                        $"Medic with id {medicUserId} and pantient with id {patientUserId} are not into" +
                        $" the same Medical Team!" );
                } );

            return validityChecker;
        }

        public static ConsistencyRulesHelper IfMedicIsNotAlreadyIntoTheMedicalTeam(
            this ConsistencyRulesHelper rulesHelper, Guid userId, Guid medicalTeamId ) {

            var validityChecker = rulesHelper.CheckIf(
                () => {
                    return !rulesHelper
                        .GetQueriesService<IMedicQueriesService>().IsIntoMedicalTeam( userId, medicalTeamId );
                },
                () => {
                    return new OkObjectResult( userId );
                },
                () => {
                    return new ConflictObjectResult( "Medic already present" );
                } );

            return validityChecker;
        }

        public static ConsistencyRulesHelper IfNurseIsNotIntoTheMedicalTeam(
            this ConsistencyRulesHelper rulesHelper, Guid userId, Guid medicalTeamId ) {

            var validityChecker = rulesHelper.CheckIf(
                () => {
                    return !rulesHelper
                        .GetQueriesService<INurseQueriesService>().IsIntoMedicalTeam( userId, medicalTeamId );
                },
                () => {
                    return new OkObjectResult( userId );
                },
                () => {
                    return new ConflictObjectResult( "Medic already present" );
                } );

            return validityChecker;
        }

        public static ConsistencyRulesHelper IfDataManagerIsNotIntoTheMedicalTeam(
            this ConsistencyRulesHelper rulesHelper, Guid userId, Guid medicalTeamId ) {

            var validityChecker = rulesHelper.CheckIf(
                () => {
                    return !rulesHelper
                        .GetQueriesService<IDataManagerQueriesService>()
                        .IsIntoMedicalTeam( userId, medicalTeamId );
                },
                () => {
                    return new OkObjectResult( userId );
                },
                () => {
                    return new ConflictObjectResult( "DataManager already present" );
                } );

            return validityChecker;
        }

        public static ConsistencyRulesHelper IfUserIsInMedicalTeam(
            this ConsistencyRulesHelper rulesHelper, Guid userId, Guid medicalTeamId ) {

            var validityChecker = rulesHelper.CheckIf(
                () => {
                    var medicalTeams = rulesHelper
                        .GetQueriesService<IMedicalTeamQueriesService>()
                        .GetByUserId( userId );

                    return medicalTeams.Any( x => x.Id == medicalTeamId );
                },
                () => {
                    return new OkObjectResult( "" );
                },
                () => {
                    return new BadRequestObjectResult(
                        $"user {userId} is not into medicalTeam {medicalTeamId}" );
                } );

            return validityChecker;
        }

        public static ConsistencyRulesHelper IfMedicIsNotAlreadyMedicalTeamAdmin(
            this ConsistencyRulesHelper rulesHelper, Guid userId, Guid medicalTeamId ) {

            var validityChecker = rulesHelper.CheckIf(
                () => {
                    return !rulesHelper.GetQueriesService<IMedicQueriesService>()
                        .IsMedicAdminOfMedicalTeam( userId, medicalTeamId );
                },
                () => {
                    return new OkObjectResult( userId );
                },
                () => {
                    return new ConflictObjectResult( "Medic already Admin" );
                } );

            return validityChecker;
        }

        public static ConsistencyRulesHelper IfMedicIsMedicalTeamAdmin(
            this ConsistencyRulesHelper rulesHelper, Guid userId, Guid medicalTeamId ) {

            var validityChecker = rulesHelper.CheckIf(
                () => {
                    return rulesHelper.GetQueriesService<IMedicQueriesService>()
                        .IsMedicAdminOfMedicalTeam( userId, medicalTeamId );
                },
                () => {
                    return new OkObjectResult( userId );
                },
                () => {
                    return new BadRequestObjectResult(
                        $"Medic is not Admin of medical Team with id {medicalTeamId}" );
                } );

            return validityChecker;
        }

        public static ConsistencyRulesHelper IfIHavePermissionsToAssignUserToThisMedicalTeam(
            this ConsistencyRulesHelper rulesHelper, Guid userId, Guid medicalTeamId, UserRoles userRoles ) {

            var validityChecker = rulesHelper.CheckIf(
                () => {
                    if ( userRoles.HasRoleOf( Roles.SystemAdmin ) ) {
                        return true;
                    }
                    else if ( userRoles.HasRoleOf( Roles.MedicalTeamAdmin ) 
                                || userRoles.HasRoleOf( Roles.MedicalProfessional )
                                   || userRoles.HasRoleOf( Roles.MedicalTeamDataManager ) ) {
                        return rulesHelper.GetQueriesService<IMedicalTeamQueriesService>()
                            .IsMedicAdminOfMedicalTeam( userId, medicalTeamId );
                    }

                    return false;
                },
                () => {
                    return new OkObjectResult( userId );
                },
                () => {;
                    return new UnauthorizedObjectResult( 
                        $"You are not authorized to assign user to medicalTeam" );
                } );

            return validityChecker;
        }

        public static ConsistencyRulesHelper IfMedicIsValid(
            this ConsistencyRulesHelper rulesHelper, Guid userId, out Medic medic ) {
            Medic medicResult = null;

            var validityChecker = rulesHelper.CheckIf(
                () => {
                    medicResult = rulesHelper.GetQueriesService<IMedicQueriesService>().Get( userId );

                    return medicResult != null;
                },
                () => {
                    return new OkObjectResult( userId );
                },
                () => {
                    return new UnauthorizedObjectResult( $"medic with userid {userId} not found" );
                } );

            medic = medicResult; 
            return validityChecker;
        }

        public static ConsistencyRulesHelper IfMedicAdminIsValid(
            this ConsistencyRulesHelper rulesHelper, Guid userId, out MedicAdmin medicAdmin ) {
            MedicAdmin medicAdminResult = null;

            var validityChecker = rulesHelper.CheckIf(
                () => {
                    medicAdminResult = rulesHelper
                        .GetQueriesService<IMedicQueriesService>().GetAdminById( userId );

                    return medicAdminResult != null;
                },
                () => {
                    return new OkObjectResult( userId );
                },
                () => {
                    return new UnauthorizedObjectResult(
                        $"The is no Medical Admin with id {userId}" );
                } );

            medicAdmin = medicAdminResult;
            return validityChecker;
        }

        public static ConsistencyRulesHelper IfMedicNotExist(
            this ConsistencyRulesHelper rulesHelper, Guid userId ) {

            var validityChecker = rulesHelper.CheckIf(
                () => {
                    return rulesHelper.GetQueriesService<IMedicQueriesService>().Get( userId ) == null;
                },
                () => {
                    return new OkObjectResult( userId );
                },
                () => {
                    return new ConflictObjectResult( 
                        $"A medic with user id: {userId} already exist!" );
                } );

            return validityChecker;
        }

        public static ConsistencyRulesHelper IfMedicalTeamHasAlmostTwoOrMoreAdmins(
            this ConsistencyRulesHelper rulesHelper, Guid medicalTeamId ) {

            var validityChecker = rulesHelper.CheckIf(
                () => {
                    return rulesHelper
                        .GetQueriesService<IMedicalTeamQueriesService>().GetAdmins( medicalTeamId ).Count > 1;
                },
                () => {
                    return new OkObjectResult( medicalTeamId );
                },
                () => {
                    return new BadRequestObjectResult( "MedicalTeam must have at last one admin" );
                } );

            return validityChecker;
        }

        public static ConsistencyRulesHelper IfMedicalTeamIsInMyInstitute(
            this ConsistencyRulesHelper rulesHelper, Guid myInstituteId, MedicalTeam medicalTeam ) {

            var validityChecker = rulesHelper.CheckIf(
                () => {
                    return medicalTeam.Project.InstituteId == myInstituteId;
                },
                () => {
                    return new OkObjectResult( "" );
                },
                () => {
                    return new BadRequestObjectResult(
                        $"medicalTeam {medicalTeam.Id} is not into institute {myInstituteId}" );
                } );

            return validityChecker;
        }
    }
}

