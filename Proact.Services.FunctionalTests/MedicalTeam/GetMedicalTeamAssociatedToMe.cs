using Microsoft.AspNetCore.Mvc;
using Proact.Services.AuthorizationPolicies;
using Proact.Services.Entities;
using Proact.Services.FunctionalTests.Institutes;
using Proact.Services.Models;
using Proact.Services.Tests.Shared;
using System;
using System.Collections.Generic;
using Xunit;

namespace Proact.Services.FunctionalTests.MedicalTeams {
    public class GetMedicalTeamsAssociatedToMe {
        [Fact]
        public void GetMedicalTeamsAssociatedToMe_AsPatient() {
            var servicesProvider = new ProactServicesProvider();
            Institute institute = null;
            Project project = null;
            MedicalTeam medicalTeam = null;
            Patient patient = null;

            new DatabaseSnapshotProvider( servicesProvider )
                .AddInstituteWithRandomValues( out institute )
                .AddProjectWithRandomValues( institute, out project )
                .AddMedicalTeamWithRandomValues( project, out medicalTeam )
                .AddPatientWithRandomValues( medicalTeam, out patient );

            var medicsController = new MedicalTeamControllerProvider(
                servicesProvider, patient.User, Roles.Patient );
            var result = medicsController.Controller
                .GetMedicalTeamAssociatedToMe( project.Id );

            var medicalTeams = ( result as OkObjectResult ).Value as List<MedicalTeamModel>;

            Assert.Single( medicalTeams );
        }

        [Fact]
        public void GetMedicalTeamsAssociatedToMe_AsMedic() {
            var servicesProvider = new ProactServicesProvider();
            Institute institute = null;
            Project project = null;
            MedicalTeam medicalTeam_0 = null;
            MedicalTeam medicalTeam_1 = null;
            Medic medic = null;

            new DatabaseSnapshotProvider( servicesProvider )
                .AddInstituteWithRandomValues( out institute )
                .AddProjectWithRandomValues( institute, out project )
                .AddMedicalTeamWithRandomValues( project, out medicalTeam_0 )
                .AddMedicalTeamWithRandomValues( project, out medicalTeam_1 )
                .AddMedicWithRandomValues( medicalTeam_0, out medic )
                .AddMedicToMedicalTeam( medicalTeam_1, medic );

            var medicsController = new MedicalTeamControllerProvider(
                servicesProvider, medic.User, Roles.MedicalProfessional );
            var result = medicsController.Controller
                .GetMedicalTeamAssociatedToMe( project.Id );

            var medicalTeams = ( result as OkObjectResult ).Value as List<MedicalTeamModel>;

            Assert.Equal( 2, medicalTeams.Count );
        }

        [Fact]
        public void GetMedicalTeamsAssociatedToMe_AsNurse() {
            var servicesProvider = new ProactServicesProvider();
            Institute institute = null;
            Project project = null;
            MedicalTeam medicalTeam_0 = null;
            MedicalTeam medicalTeam_1 = null;
            Nurse nurse = null;

            new DatabaseSnapshotProvider( servicesProvider )
                .AddInstituteWithRandomValues( out institute )
                .AddProjectWithRandomValues( institute, out project )
                .AddMedicalTeamWithRandomValues( project, out medicalTeam_0 )
                .AddMedicalTeamWithRandomValues( project, out medicalTeam_1 )
                .AddNurseWithRandomValues( medicalTeam_0, out nurse )
                .AddNurseToMedicalTeam( medicalTeam_1, nurse );

            var medicsController = new MedicalTeamControllerProvider(
                servicesProvider, nurse.User, Roles.Nurse );
            var result = medicsController.Controller
                .GetMedicalTeamAssociatedToMe( project.Id );

            var medicalTeams = ( result as OkObjectResult ).Value as List<MedicalTeamModel>;

            Assert.Equal( 2, medicalTeams.Count );
        }

        [Fact]
        public void GetMedicalTeamsAssociatedToMe_AsResearcher() {
            var servicesProvider = new ProactServicesProvider();
            Institute institute = null;
            Project project = null;
            MedicalTeam medicalTeam_0 = null;
            Researcher researcher = null;

            new DatabaseSnapshotProvider( servicesProvider )
                .AddInstituteWithRandomValues( out institute )
                .AddProjectWithRandomValues( institute, out project )
                .AddMedicalTeamWithRandomValues( project, out medicalTeam_0 )
                .AddResearcherWithRandomValues( medicalTeam_0, out researcher );

            var medicsController = new MedicalTeamControllerProvider(
                servicesProvider, researcher.User, Roles.Researcher );
            var result = medicsController.Controller
                .GetMedicalTeamAssociatedToMe( project.Id );

            var medicalTeams = ( result as OkObjectResult ).Value as List<MedicalTeamModel>;

            Assert.Single( medicalTeams );
        }
    }
}
