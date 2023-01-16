using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Proact.Services.Entities;

namespace Proact.Services.QueriesServices {
    public static class DbPatientValidityChecker {
        public static ConsistencyRulesHelper IfPatientIsValid(
            this ConsistencyRulesHelper rulesHelper, Guid userId, out Patient patient ) {
            Patient patientResult = null;

            var validityChecker = rulesHelper.CheckIf(
                () => {
                    patientResult = rulesHelper
                        .GetQueriesService<IPatientQueriesService>()
                        .Get( userId );

                    return patientResult != null;
                },
                () => {
                    return new OkObjectResult( patientResult );
                },
                () => {
                    return new NotFoundObjectResult( $"patient with user id {userId} not found!" );
                } );

            patient = patientResult;
            return validityChecker;
        }

        public static ConsistencyRulesHelper IfPatientNotExist(
            this ConsistencyRulesHelper rulesHelper, Guid userId ) {
            
            var validityChecker = rulesHelper.CheckIf(
                () => {
                    return rulesHelper.GetQueriesService<IPatientQueriesService>().Get( userId ) == null;
                },
                () => {
                    return new OkObjectResult( userId );
                },
                () => {
                    return new ConflictObjectResult( 
                        $"The patient associate to id: {userId} already exist!" );
                } );

            return validityChecker;
        }

        public static ConsistencyRulesHelper IfPatientWithCodeNotExist(
            this ConsistencyRulesHelper rulesHelper, string code ) {

            var validityChecker = rulesHelper.CheckIf(
                () => {
                    return rulesHelper
                        .GetQueriesService<IPatientQueriesService>()
                        .GetByCode( code ) == null;
                },
                () => {
                    return new OkObjectResult( "" );
                },
                () => {
                    return new ConflictObjectResult(
                        $"The patient associate to code: {code} already exist!" );
                } );

            return validityChecker;
        }

        public static ConsistencyRulesHelper IfPatientsAreValid(
            this ConsistencyRulesHelper rulesHelper, List<Guid> userIds ) {
            
            var validityChecker = rulesHelper.CheckIf(
                () => {
                    var patients = rulesHelper.GetQueriesService<IPatientQueriesService>().Gets( userIds );

                    return patients.Count == userIds.Count;
                },
                () => {
                    return new OkObjectResult( userIds );
                },
                () => {
                    return new NotFoundObjectResult( "Can't found some userIds!" );
                } );

            return validityChecker;
        }

        public static ConsistencyRulesHelper IfPatientsAreInProject(
           this ConsistencyRulesHelper rulesHelper, Guid projectId, List<Guid> userIds ) {

            var validityChecker = rulesHelper.CheckIf(
                () => {
                    return rulesHelper
                        .GetQueriesService<IPatientQueriesService>()
                        .ArePatientsIntoProject( projectId, userIds );
                },
                () => {
                    return new OkObjectResult( userIds );
                },
                () => {
                    return new BadRequestObjectResult( $"Some users is not in the project {projectId}" );
                } );

            return validityChecker;
        }

        public static ConsistencyRulesHelper IfPatientIsInAnyOfMyMedicalTeams(
            this ConsistencyRulesHelper rulesHelper, Guid patientUserId, Guid userRequesterId ) {

            var validityChecker = rulesHelper.CheckIf(
                () => {
                    return rulesHelper
                        .GetQueriesService<IMedicalTeamQueriesService>()
                        .UsersAreInTheSameMedicalTeam( patientUserId, userRequesterId );
                },
                () => {
                    return new OkObjectResult( "" );
                },
                () => {
                    return new BadRequestObjectResult(
                        $"user {userRequesterId} and patient userid: {patientUserId} are not into same medical team" );
                } );

            return validityChecker;
        }
    }
}

