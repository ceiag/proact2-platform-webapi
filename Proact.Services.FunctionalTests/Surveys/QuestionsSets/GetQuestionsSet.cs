using Microsoft.AspNetCore.Mvc;
using Proact.Services.AuthorizationPolicies;
using Proact.Services.Entities;
using Proact.Services.Models;
using Proact.Services.Tests.Shared;
using System.Collections.Generic;
using Xunit;

namespace Proact.Services.FunctionalTests.Surveys.QuestionsSets {
    public class GetQuestionsSet {
        [Fact]
        public void GetQuestionsSets_ReturnsOnlyUnderOneProjectId() {
            Institute institute = null;
            Project project_0 = null;
            Project project_1 = null;
            MedicalTeam medicalTeam = null;
            Medic medic = null;
            List<SurveyQuestionsSet> questionsSetForProject_0 = null;
            List<SurveyQuestionsSet> questionsSetForProject_1 = null;

            var servicesProvider = new ProactServicesProvider();
            new DatabaseSnapshotProvider( servicesProvider )
                .AddInstituteWithRandomValues( out institute )
                .AddProjectWithRandomValues( institute, out project_0 )
                .AddProjectWithRandomValues( institute, out project_1 )
                .AddMedicalTeamWithRandomValues( project_0, out medicalTeam )
                .AddMedicWithRandomValues( medicalTeam, out medic )
                .AddQuestionsSetsWithRandomValues( project_0, 2, out questionsSetForProject_0, true )
                .AddQuestionsSetsWithRandomValues( project_1, 1, out questionsSetForProject_1, true );

            var provider = new QuestionsSetsControllerProvider(
                servicesProvider, medic.User, Roles.MedicalTeamAdmin );
            var result = provider.Controller.GetQuestionsSets( project_0.Id );

            var questionsSets = ( result as OkObjectResult ).Value as List<SurveyQuestionsSetModel>;

            Assert.Equal( 2, questionsSets.Count );
        }
    }
}
