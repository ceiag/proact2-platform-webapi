using Microsoft.AspNetCore.Mvc;
using Proact.Services.AuthorizationPolicies;
using Proact.Services.Entities;
using Proact.Services.Models;
using Proact.Services.Tests.Shared;
using System.Collections.Generic;
using Xunit;

namespace Proact.Services.FunctionalTests.Surveys.QuestionsSets {
    public class DeleteQuestionsSet {
        [Fact]
        public void DeleteQuestionsSet_ReturnOk() {
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
                .AddQuestionsSetsWithRandomValues( project_0, 2, out questionsSetForProject_0, false )
                .AddQuestionsSetsWithRandomValues( project_1, 1, out questionsSetForProject_1, false );

            var provider = new QuestionsSetsControllerProvider(
                servicesProvider, medic.User, Roles.MedicalTeamAdmin );
            var result = provider.Controller
                .DeleteQuestionsSet( questionsSetForProject_0[0].Id );

            var questionsSets = ( provider.Controller.GetQuestionsSets( project_0.Id ) as OkObjectResult )
                .Value as List<SurveyQuestionsSetModel>;

            Assert.Single( questionsSets );
        }
    }
}
