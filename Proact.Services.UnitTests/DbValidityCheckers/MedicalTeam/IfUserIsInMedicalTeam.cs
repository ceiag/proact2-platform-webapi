using Microsoft.AspNetCore.Mvc;
using Proact.Services.AuthorizationPolicies;
using Proact.Services.Entities;
using Proact.Services.QueriesServices;
using Proact.Services.Tests.Shared;
using System;
using System.Collections.Generic;
using Xunit;

namespace Proact.Services.UnitTests.DbValidityCheckers.MedicalTeams {
    public class IfUserIsInMedicalTeam {
        [Fact]
        public void IfUserIsInMedicalTeam_WithMedic_ReturnTrue() {
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
                    .IfUserIsInMedicalTeam( medic.UserId, medicalTeam.Id )
                    .Then( () => {
                        return new OkResult();
                    } )
                    .ReturnResult();

            Assert.NotNull( result as OkResult );
        }

        [Fact]
        public void IfUserIsInMedicalTeam_WithMedic_ReturnFalse() {
            Institute institute = null;
            Project project = null;
            MedicalTeam medicalTeam_0 = null;
            MedicalTeam medicalTeam_1 = null;
            Medic medic_0 = null;
            Medic medic_1 = null;

            var servicesProvider = new ProactServicesProvider();
            new DatabaseSnapshotProvider( servicesProvider )
                .AddInstituteWithRandomValues( out institute )
                .AddProjectWithRandomValues( institute, out project )
                .AddMedicalTeamWithRandomValues( project, out medicalTeam_0 )
                .AddMedicalTeamWithRandomValues( project, out medicalTeam_1 )
                .AddMedicWithRandomValues( medicalTeam_0, out medic_0 )
                .AddMedicWithRandomValues( medicalTeam_1, out medic_1 );

            var result = servicesProvider.ConsistencyRulesHelper
                    .IfUserIsInMedicalTeam( medic_1.UserId, medicalTeam_0.Id )
                    .Then( () => {
                        return new BadRequestObjectResult( "" );
                    } )
                    .ReturnResult();

            Assert.NotNull( result as BadRequestObjectResult );
        }

        [Fact]
        public void IfUserIsInMedicalTeam_WithNurse_ReturnTrue() {
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
                    .IfUserIsInMedicalTeam( nurse.UserId, medicalTeam.Id )
                    .Then( () => {
                        return new OkResult();
                    } )
                    .ReturnResult();

            Assert.NotNull( result as OkResult );
        }

        [Fact]
        public void IfUserIsInMedicalTeam_WithNurse_ReturnFalse() {
            Institute institute = null;
            Project project = null;
            MedicalTeam medicalTeam_0 = null;
            MedicalTeam medicalTeam_1 = null;
            Nurse nurse_0 = null;
            Nurse nurse_1 = null;

            var servicesProvider = new ProactServicesProvider();
            new DatabaseSnapshotProvider( servicesProvider )
                .AddInstituteWithRandomValues( out institute )
                .AddProjectWithRandomValues( institute, out project )
                .AddMedicalTeamWithRandomValues( project, out medicalTeam_0 )
                .AddMedicalTeamWithRandomValues( project, out medicalTeam_1 )
                .AddNurseWithRandomValues( medicalTeam_0, out nurse_0 )
                .AddNurseWithRandomValues( medicalTeam_1, out nurse_1 );

            var result = servicesProvider.ConsistencyRulesHelper
                    .IfUserIsInMedicalTeam( nurse_1.UserId, medicalTeam_0.Id )
                    .Then( () => {
                        return new BadRequestObjectResult( "" );
                    } )
                    .ReturnResult();

            Assert.NotNull( result as BadRequestObjectResult );
        }

        [Fact]
        public void IfUserIsInMedicalTeam_WithResearcher_ReturnTrue() {
            Institute institute = null;
            Project project = null;
            MedicalTeam medicalTeam = null;
            Researcher researcher = null;

            var servicesProvider = new ProactServicesProvider();
            new DatabaseSnapshotProvider( servicesProvider )
                .AddInstituteWithRandomValues( out institute )
                .AddProjectWithRandomValues( institute, out project )
                .AddMedicalTeamWithRandomValues( project, out medicalTeam )
                .AddResearcherWithRandomValues( medicalTeam, out researcher );

            var result = servicesProvider.ConsistencyRulesHelper
                    .IfUserIsInMedicalTeam( researcher.UserId, medicalTeam.Id )
                    .Then( () => {
                        return new OkResult();
                    } )
                    .ReturnResult();

            Assert.NotNull( result as OkResult );
        }

        [Fact]
        public void IfUserIsInMedicalTeam_WithResearcher_ReturnFalse() {
            Institute institute = null;
            Project project = null;
            MedicalTeam medicalTeam_0 = null;
            MedicalTeam medicalTeam_1 = null;
            Researcher researcher_0 = null;
            Researcher researcher_1 = null;

            var servicesProvider = new ProactServicesProvider();
            new DatabaseSnapshotProvider( servicesProvider )
                .AddInstituteWithRandomValues( out institute )
                .AddProjectWithRandomValues( institute, out project )
                .AddMedicalTeamWithRandomValues( project, out medicalTeam_0 )
                .AddMedicalTeamWithRandomValues( project, out medicalTeam_1 )
                .AddResearcherWithRandomValues( medicalTeam_0, out researcher_0 )
                .AddResearcherWithRandomValues( medicalTeam_1, out researcher_1 );

            var result = servicesProvider.ConsistencyRulesHelper
                    .IfUserIsInMedicalTeam( researcher_1.UserId, medicalTeam_0.Id )
                    .Then( () => {
                        return new BadRequestObjectResult( "" );
                    } )
                    .ReturnResult();

            Assert.NotNull( result as BadRequestObjectResult );
        }

        [Fact]
        public void IfUserIsInMedicalTeam_WithPatient_ReturnTrue() {
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
                    .IfUserIsInMedicalTeam( patient.UserId, medicalTeam.Id )
                    .Then( () => {
                        return new OkResult();
                    } )
                    .ReturnResult();

            Assert.NotNull( result as OkResult );
        }

        [Fact]
        public void IfUserIsInMedicalTeam_WithPatient_ReturnFalse() {
            Institute institute = null;
            Project project = null;
            MedicalTeam medicalTeam_0 = null;
            MedicalTeam medicalTeam_1 = null;
            Patient patient_0 = null;
            Patient patient_1 = null;

            var servicesProvider = new ProactServicesProvider();
            new DatabaseSnapshotProvider( servicesProvider )
                .AddInstituteWithRandomValues( out institute )
                .AddProjectWithRandomValues( institute, out project )
                .AddMedicalTeamWithRandomValues( project, out medicalTeam_0 )
                .AddMedicalTeamWithRandomValues( project, out medicalTeam_1 )
                .AddPatientWithRandomValues( medicalTeam_0, out patient_0 )
                .AddPatientWithRandomValues( medicalTeam_1, out patient_1 );

            var result = servicesProvider.ConsistencyRulesHelper
                    .IfUserIsInMedicalTeam( patient_1.UserId, medicalTeam_0.Id )
                    .Then( () => {
                        return new BadRequestObjectResult( "" );
                    } )
                    .ReturnResult();

            Assert.NotNull( result as BadRequestObjectResult );
        }
    }
}
