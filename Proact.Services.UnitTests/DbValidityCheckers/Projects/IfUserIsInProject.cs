using Microsoft.AspNetCore.Mvc;
using Proact.Services.AuthorizationPolicies;
using Proact.Services.Entities;
using Proact.Services.Tests.Shared;
using System;
using System.Collections.Generic;
using Xunit;

namespace Proact.Services.UnitTests.DbValidityCheckers.Projects {
    public class IfUserIsInProject {
        [Fact]
        public void IfUserIsInProject_AsPatient_ReturnTrue() {
            Institute institute = null;
            Project project = null;
            MedicalTeam medicalTeam = null;
            Patient patient = null;

            var servicesProvider = new ProactServicesProvider();
            new DatabaseSnapshotProvider( servicesProvider )
                .AddInstituteWithRandomValues( out institute )
                .AddProjectWithRandomValues( institute, out project )
                .AddMedicalTeamWithRandomValues( project, out medicalTeam )
                .AddPatientWithRandomValues( medicalTeam, out patient );

            var result = servicesProvider.ConsistencyRulesHelper
                    .IfUserIsInProject( patient.UserId, project.Id )
                    .Then( () => {
                        return new OkResult();
                    } )
                    .ReturnResult();

            Assert.NotNull( result as OkResult );
        }

        [Fact]
        public void IfUserIsInProject_AsPatient_ReturnBadRequest() {
            Institute institute = null;
            Project project_0 = null;
            Project project_1 = null;
            MedicalTeam medicalTeam = null;
            Patient patient = null;

            var servicesProvider = new ProactServicesProvider();
            new DatabaseSnapshotProvider( servicesProvider )
                .AddInstituteWithRandomValues( out institute )
                .AddProjectWithRandomValues( institute, out project_0 )
                .AddProjectWithRandomValues( institute, out project_1 )
                .AddMedicalTeamWithRandomValues( project_0, out medicalTeam )
                .AddPatientWithRandomValues( medicalTeam, out patient );

            var result = servicesProvider.ConsistencyRulesHelper
                    .IfUserIsInProject( patient.UserId, project_1.Id )
                    .Then( () => {
                        return new BadRequestObjectResult( "" );
                    } )
                    .ReturnResult();

            Assert.NotNull( result as BadRequestObjectResult );
        }

        [Fact]
        public void IfUserIsInProject_AsMedic_ReturnTrue() {
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

            var result = servicesProvider.ConsistencyRulesHelper
                    .IfUserIsInProject( medic.UserId, project.Id )
                    .Then( () => {
                        return new OkResult();
                    } )
                    .ReturnResult();

            Assert.NotNull( result as OkResult );
        }

        [Fact]
        public void IfUserIsInProject_AsMedic_ReturnBadRequest() {
            Institute institute = null;
            Project project_0 = null;
            Project project_1 = null;
            MedicalTeam medicalTeam = null;
            Medic medic = null;

            var servicesProvider = new ProactServicesProvider();
            new DatabaseSnapshotProvider( servicesProvider )
                .AddInstituteWithRandomValues( out institute )
                .AddProjectWithRandomValues( institute, out project_0 )
                .AddProjectWithRandomValues( institute, out project_1 )
                .AddMedicalTeamWithRandomValues( project_0, out medicalTeam )
                .AddMedicWithRandomValues( medicalTeam, out medic );

            var result = servicesProvider.ConsistencyRulesHelper
                    .IfUserIsInProject( medic.UserId, project_1.Id )
                    .Then( () => {
                        return new BadRequestObjectResult( "" );
                    } )
                    .ReturnResult();

            Assert.NotNull( result as BadRequestObjectResult );
        }

        [Fact]
        public void IfUserIsInProject_AsNurse_ReturnTrue() {
            Institute institute = null;
            Project project = null;
            MedicalTeam medicalTeam = null;
            Nurse nurse = null;

            var servicesProvider = new ProactServicesProvider();
            new DatabaseSnapshotProvider( servicesProvider )
                .AddInstituteWithRandomValues( out institute )
                .AddProjectWithRandomValues( institute, out project )
                .AddMedicalTeamWithRandomValues( project, out medicalTeam )
                .AddNurseWithRandomValues( medicalTeam, out nurse );

            var result = servicesProvider.ConsistencyRulesHelper
                    .IfUserIsInProject( nurse.UserId, project.Id )
                    .Then( () => {
                        return new OkResult();
                    } )
                    .ReturnResult();

            Assert.NotNull( result as OkResult );
        }

        [Fact]
        public void IfUserIsInProject_AsNurse_ReturnBadRequest() {
            Institute institute = null;
            Project project_0 = null;
            Project project_1 = null;
            MedicalTeam medicalTeam = null;
            Nurse nurse = null;

            var servicesProvider = new ProactServicesProvider();
            new DatabaseSnapshotProvider( servicesProvider )
                .AddInstituteWithRandomValues( out institute )
                .AddProjectWithRandomValues( institute, out project_0 )
                .AddProjectWithRandomValues( institute, out project_1 )
                .AddMedicalTeamWithRandomValues( project_0, out medicalTeam )
                .AddNurseWithRandomValues( medicalTeam, out nurse );

            var result = servicesProvider.ConsistencyRulesHelper
                    .IfUserIsInProject( nurse.UserId, project_1.Id )
                    .Then( () => {
                        return new BadRequestObjectResult( "" );
                    } )
                    .ReturnResult();

            Assert.NotNull( result as BadRequestObjectResult );
        }
    }
}
