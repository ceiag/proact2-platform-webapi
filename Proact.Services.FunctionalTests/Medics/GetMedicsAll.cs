using Microsoft.AspNetCore.Mvc;
using Proact.Services.AuthorizationPolicies;
using Proact.Services.Entities;
using Proact.Services.Tests.Shared;
using System.Collections.Generic;
using Xunit;

namespace Proact.Services.FunctionalTests.Medics {
    public class GetMedicsAll {
        [Fact]
        public void GetAllMedicsFromMyInstitute() {
            var servicesProvider = new ProactServicesProvider();
            Institute institute_0 = null;
            Institute institute_1 = null;
            User instituteAdmin_0 = null;
            User instituteAdmin_1 = null;
            Project project_0 = null;
            Project project_1 = null;
            MedicalTeam medicalTeam_0 = null;
            MedicalTeam medicalTeam_1 = null;
            Medic medic_0 = null;
            Medic medic_1 = null;
            Medic medic_2 = null;

            new DatabaseSnapshotProvider( servicesProvider )
                .AddInstituteWithRandomValues( out institute_0 )
                .AddInstituteWithRandomValues( out institute_1 )
                .AddInstituteAdminWithRandomValues( institute_0, out instituteAdmin_0 )
                .AddInstituteAdminWithRandomValues( institute_1, out instituteAdmin_1 )
                .AddProjectWithRandomValues( institute_0, out project_0 )
                .AddProjectWithRandomValues( institute_1, out project_1 )
                .AddMedicalTeamWithRandomValues( project_0, out medicalTeam_0 )
                .AddMedicalTeamWithRandomValues( project_1, out medicalTeam_1 )
                .AddMedicWithRandomValues( medicalTeam_0, out medic_0 )
                .AddMedicWithRandomValues( medicalTeam_0, out medic_1 )
                .AddMedicWithRandomValues( medicalTeam_1, out medic_2 );

            var medicsController = new MedicsControllerProvider(
                servicesProvider, medic_0.User, Roles.MedicalProfessional );
            var result = medicsController.Controller.GetMedicAll();

            var medics = ( result as OkObjectResult ).Value as List<MedicModel>;

            Assert.Equal( 2, medics.Count );
        }
    }
}
