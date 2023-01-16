using Microsoft.AspNetCore.Mvc;
using Proact.Services.AuthorizationPolicies;
using Proact.Services.Entities;
using Proact.Services.FunctionalTests.DataManagers.ControllerProvider;
using Proact.Services.Models.DataManagers;
using Proact.Services.Tests.Shared;
using System;
using Xunit;

namespace Proact.Services.FunctionalTests.DataManagers;
public class CreateDataManager {
    [Fact]
    public void CreateDataManager_ConsistencyCheck() {
        var servicesProvider = new ProactServicesProvider();
        Institute institute = null;
        User instituteAdmin = null;
        Project project = null;
        MedicalTeam medicalTeam = null;
        Medic medicAdmin = null;

        new DatabaseSnapshotProvider( servicesProvider )
            .AddInstituteWithRandomValues( out institute )
            .AddInstituteAdminWithRandomValues( institute, out instituteAdmin )
            .AddProjectWithRandomValues( institute, out project )
            .AddMedicalTeamWithRandomValues( project, out medicalTeam )
            .AddMedicWithRandomValues( medicalTeam, out medicAdmin )
            .AddAdminToMedicalTeam( medicAdmin, medicalTeam );

        var request = new CreateDataManagerRequest() {
            Email = "data@manager.com",
            FirstName = "datamanager_name",
            Lastname = "datamanager_surname"
        };

        var controller = new DataManagersControllerProvider(
            servicesProvider, medicAdmin.User, Roles.MedicalTeamAdmin );

        var result = controller.Controller.CreateDataManager( request );
        var dataManagerModel = ( result as OkObjectResult ).Value as DataManagerModel;

        Assert.NotNull( dataManagerModel );
        Assert.NotEqual( Guid.Empty, dataManagerModel.UserId );
    }
}
