using Microsoft.AspNetCore.Mvc;
using Proact.Services.AuthorizationPolicies;
using Proact.Services.Entities;
using Proact.Services.QueriesServices;
using Proact.Services.Tests.Shared;
using System;
using System.Collections.Generic;
using Xunit;

namespace Proact.Services.UnitTests.DbValidityCheckers.MedicalTeams {
    public class IfMedicIsAdminOfMedicalTeamInProject {
        [Fact]
        public void IfMedicIsAdminOfMedicalTeamInProject_ReturnTrue() {
            Institute institute = null;
            Project project = null;
            MedicalTeam medicalTeam = null;
            Medic medic = null;

            var servicesProvider = new ProactServicesProvider();
            new DatabaseSnapshotProvider( servicesProvider )
                .AddInstituteWithRandomValues( out institute )
                .AddProjectWithRandomValues( institute, out project )
                .AddMedicalTeamWithRandomValues( project, out medicalTeam )
                .AddMedicWithRandomValues( medicalTeam, out medic );

            var userRoles = new UserRoles( new List<string>() { Roles.MedicalTeamAdmin } );

            var result = servicesProvider.ConsistencyRulesHelper
                    .IfMedicIsAdminOfMedicalTeamInProject( medic.UserId, project.Id, userRoles )
                    .Then( () => {
                        return new OkResult();
                    } )
                    .ReturnResult();

            Assert.NotNull( result as OkResult );
        }

        [Fact]
        public void IfMedicIsAdminOfMedicalTeamInProject_ReturnFalse() {
            Institute institute = null;
            Project project = null;
            MedicalTeam medicalTeam = null;
            Medic medic = null;

            var servicesProvider = new ProactServicesProvider();
            new DatabaseSnapshotProvider( servicesProvider )
                .AddInstituteWithRandomValues( out institute )
                .AddProjectWithRandomValues( institute, out project )
                .AddMedicalTeamWithRandomValues( project, out medicalTeam )
                .AddMedicWithRandomValues( medicalTeam, out medic );

            var userRoles = new UserRoles( new List<string>() { Roles.MedicalProfessional } );

            var result = servicesProvider.ConsistencyRulesHelper
                    .IfMedicIsAdminOfMedicalTeamInProject( medic.UserId, project.Id, userRoles )
                    .Then( () => {
                        return new UnauthorizedObjectResult( "" );
                    } )
                    .ReturnResult();

            Assert.NotNull( result as UnauthorizedObjectResult );
        }
    }
}
