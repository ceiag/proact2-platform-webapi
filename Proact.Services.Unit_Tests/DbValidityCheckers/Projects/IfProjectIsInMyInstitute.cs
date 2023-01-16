using Microsoft.AspNetCore.Mvc;
using Proact.Services.Entities;
using Proact.Services.Models;
using Proact.Services.QueriesServices;
using Proact.Services.Tests.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Proact.Services.UnitTests.DbValidityCheckers.Projects {
    public class IfProjectIsInMyInstitute {
        [Fact]
        public void IfProjectIsInMyInstitute_ReturnTrue() {
            User instituteAdmin_0 = null;
            User instituteAdmin_1 = null;
            Institute institute_0 = null;
            Institute institute_1 = null;
            Project project_0 = null;
            Project project_1 = null;
            MedicalTeam medicalTeam_0 = null;
            MedicalTeam medicalTeam_1 = null;
            Medic projectAdmin_0 = null;
            Medic projectAdmin_1 = null;

            var servicesProvider = new ProactServicesProvider();
            new DatabaseSnapshotProvider( servicesProvider )
                .AddInstituteWithRandomValues( out institute_0 )
                .AddInstituteWithRandomValues( out institute_1 )
                .AddInstituteAdminWithRandomValues( institute_0, out instituteAdmin_0 )
                .AddInstituteAdminWithRandomValues( institute_1, out instituteAdmin_1 )
                .AddProjectWithRandomValues( institute_0, out project_0 )
                .AddProjectWithRandomValues( institute_1, out project_1 )
                .AddMedicalTeamWithRandomValues( project_0, out medicalTeam_0 )
                .AddMedicalTeamWithRandomValues( project_1, out medicalTeam_1 )
                .AddMedicWithRandomValues( medicalTeam_0, out projectAdmin_0 )
                .AddMedicWithRandomValues( medicalTeam_1, out projectAdmin_1 );

            var result = servicesProvider.ConsistencyRulesHelper
                    .IfProjectIsInMyInstitute( institute_0.Id, project_0 )
                    .Then( () => {
                        return new OkResult();
                    } )
                    .ReturnResult();

            Assert.Equal( 200, ( result as OkObjectResult ).Value );
        }
    }
}
