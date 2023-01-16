using Microsoft.AspNetCore.Mvc;
using Proact.Services.AuthorizationPolicies;
using Proact.Services.Entities;
using Proact.Services.FunctionalTests.ProjectProperties.ControllerProvider;
using Proact.Services.Models;
using Proact.Services.Tests.Shared;
using Xunit;

namespace Proact.Services.FunctionalTests.ProjectsProperties;
public class Update {
    [Fact]
    public void _AllParamsAsTrue_IntegrityCheck() {
        User systemAdmin = null;
        User instituteAdmin = null;
        Institute institute = null;
        Project project = null;

        var servicesProvider = new ProactServicesProvider();
        new DatabaseSnapshotProvider( servicesProvider )
            .AddInstituteWithRandomValues( out institute )
            .AddInstituteAdminWithRandomValues( institute, out instituteAdmin )
            .AddUserWithRandomValues( institute, out systemAdmin )
            .AddProjectWithRandomValues( institute, out project );

        var projectUpdateRequest = new ProjectPropertiesUpdateRequest() {
            IsAnalystConsoleActive = true,
            IsMessagingActive = true,
            IsSurveysSystemActive = true,
            MedicsCanSeeOtherAnalisys = true,
            MessageCanBeAnalizedAfterMinutes = 1,
            MessageCanBeRepliedAfterMinutes = 2,
            MessageCanNotBeDeletedAfterMinutes = 3
        };

        var projectsController = new ProjectPropertiesControllerProvider(
            servicesProvider, instituteAdmin, Roles.InstituteAdmin );
        var result = projectsController.Controller.Update( project.Id, projectUpdateRequest );

        var projectModelResult = ( result as OkObjectResult ).Value as ProjectPropertiesModel;

        Assert.True( projectModelResult.IsAnalystConsoleActive );
        Assert.True( projectModelResult.IsMessagingActive );
        Assert.True( projectModelResult.IsSurveysSystemActive );
        Assert.True( projectModelResult.MedicsCanSeeOtherAnalisys );
    }

    [Fact]
    public void _AllParamsAsFalse_IntegrityCheck() {
        User systemAdmin = null;
        User instituteAdmin = null;
        Institute institute = null;
        Project project = null;

        var servicesProvider = new ProactServicesProvider();
        new DatabaseSnapshotProvider( servicesProvider )
            .AddInstituteWithRandomValues( out institute )
            .AddInstituteAdminWithRandomValues( institute, out instituteAdmin )
            .AddUserWithRandomValues( institute, out systemAdmin )
            .AddProjectWithRandomValues( institute, out project );

        var projectUpdateRequest = new ProjectPropertiesUpdateRequest() {
            IsAnalystConsoleActive = false,
            IsMessagingActive = false,
            IsSurveysSystemActive = false,
            MedicsCanSeeOtherAnalisys = false,
            MessageCanBeAnalizedAfterMinutes = 1,
            MessageCanBeRepliedAfterMinutes = 2,
            MessageCanNotBeDeletedAfterMinutes = 3
        };

        var projectsController = new ProjectPropertiesControllerProvider(
            servicesProvider, instituteAdmin, Roles.InstituteAdmin );
        var result = projectsController.Controller.Update( project.Id, projectUpdateRequest );

        var projectModelResult = ( result as OkObjectResult ).Value as ProjectPropertiesModel;

        Assert.False( projectModelResult.IsAnalystConsoleActive );
        Assert.False( projectModelResult.IsMessagingActive );
        Assert.False( projectModelResult.IsSurveysSystemActive );
        Assert.False( projectModelResult.MedicsCanSeeOtherAnalisys );
    }
}
