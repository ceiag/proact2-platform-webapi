using Microsoft.AspNetCore.Mvc;
using Proact.Services.Entities;
using Proact.Services.QueriesServices;
using Proact.Services.Tests.Shared;
using Xunit;

namespace Proact.Services.UnitTests.DbValidityCheckers.Patients {
    public class IfPatientIsInAnyOfMyMedicalTeams {
        [Fact]
        public void _ReturnTrue() {
            Institute institute = null;
            Project project = null;
            MedicalTeam medicalTeam;
            Patient patient;
            Medic medic;

            var servicesProvider = new ProactServicesProvider();
            new DatabaseSnapshotProvider( servicesProvider )
                .AddInstituteWithRandomValues( out institute )
                .AddProjectWithRandomValues( institute, out project )
                .AddMedicalTeamWithRandomValues( project, out medicalTeam )
                .AddPatientWithRandomValues( medicalTeam, out patient )
                .AddMedicWithRandomValues( medicalTeam, out medic );

            var result = servicesProvider.ConsistencyRulesHelper
                    .IfPatientIsInAnyOfMyMedicalTeams( patient.UserId, medic.UserId )
                    .Then( () => {
                        return new OkResult();
                    } )
                    .ReturnResult();

            Assert.NotNull( result as OkResult );
        }

        [Fact]
        public void _OnDifferentProjects_ReturnFalse() {
            Institute institute = null;
            Project project_0 = null;
            Project project_1 = null;
            MedicalTeam medicalTeam_0;
            MedicalTeam medicalTeam_1;
            Patient patient;
            Medic medic;

            var servicesProvider = new ProactServicesProvider();
            new DatabaseSnapshotProvider( servicesProvider )
                .AddInstituteWithRandomValues( out institute )
                .AddProjectWithRandomValues( institute, out project_0 )
                .AddProjectWithRandomValues( institute, out project_1 )
                .AddMedicalTeamWithRandomValues( project_0, out medicalTeam_0 )
                .AddMedicalTeamWithRandomValues( project_1, out medicalTeam_1 )
                .AddPatientWithRandomValues( medicalTeam_0, out patient )
                .AddMedicWithRandomValues( medicalTeam_1, out medic );

            var result = servicesProvider.ConsistencyRulesHelper
                    .IfPatientIsInAnyOfMyMedicalTeams( patient.UserId, medic.UserId )
                    .Then( () => {
                        return new BadRequestObjectResult( "" );
                    } )
                    .ReturnResult();

            Assert.NotNull( result as BadRequestObjectResult );
        }

        [Fact]
        public void _SameProjectDifferentMedicalTeams_ReturnTrue() {
            Institute institute = null;
            Project project_0 = null;
            MedicalTeam medicalTeam_0;
            MedicalTeam medicalTeam_1;
            Patient patient;
            Medic medic;

            var servicesProvider = new ProactServicesProvider();
            new DatabaseSnapshotProvider( servicesProvider )
                .AddInstituteWithRandomValues( out institute )
                .AddProjectWithRandomValues( institute, out project_0 )
                .AddMedicalTeamWithRandomValues( project_0, out medicalTeam_0 )
                .AddMedicalTeamWithRandomValues( project_0, out medicalTeam_1 )
                .AddPatientWithRandomValues( medicalTeam_0, out patient )
                .AddMedicWithRandomValues( medicalTeam_1, out medic );

            var result = servicesProvider.ConsistencyRulesHelper
                    .IfPatientIsInAnyOfMyMedicalTeams( patient.UserId, medic.UserId )
                    .Then( () => {
                        return new BadRequestObjectResult( "" );
                    } )
                    .ReturnResult();

            Assert.NotNull( result as BadRequestObjectResult );
        }
    }
}
