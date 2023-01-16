using Microsoft.AspNetCore.Mvc;
using Proact.Services.AuthorizationPolicies;
using Proact.Services.Entities;
using Proact.Services.Models;
using Proact.Services.Tests.Shared;
using Xunit;

namespace Proact.Services.FunctionalTests.Surveys.QuestionsSets {
    public class CreateQuestionsSet {
        [Fact]
        public void CreateQuestionsSet_ConsistencyCheck() {
            User instituteAdmin = null;
            Institute institute = null;
            Project project = null;
            MedicalTeam medicalTeam = null;
            Medic medic = null;

            var servicesProvider = new ProactServicesProvider();
            new DatabaseSnapshotProvider( servicesProvider )
                .AddInstituteWithRandomValues( out institute )
                .AddInstituteAdminWithRandomValues( institute, out instituteAdmin )
                .AddProjectWithRandomValues( institute, out project )
                .AddMedicalTeamWithRandomValues( project, out medicalTeam )
                .AddMedicWithRandomValues( medicalTeam, out medic );

            var request = new QuestionsSetCreationRequest() {
                Title = "my questions set",
                Description = "a description",
                Version = "1.0"
            };

            var provider = new QuestionsSetsControllerProvider(
                servicesProvider, medic.User, Roles.MedicalTeamAdmin );
            var result = provider.Controller.CreateQuestionsSet( project.Id, request );

            var questionsSet = ( result as OkObjectResult ).Value as SurveyQuestionsSetModel;

            Assert.NotNull( questionsSet );
            Assert.Equal( request.Title, questionsSet.Title );
            Assert.Equal( request.Description, questionsSet.Description );
            Assert.Equal( request.Version, questionsSet.Version );

            var questionsSetRetrived = provider.GetQuestionsSetModel( project.Id, questionsSet.Id );

            Assert.Equal( request.Title, questionsSetRetrived.Title );
            Assert.Equal( request.Description, questionsSetRetrived.Description );
            Assert.Equal( request.Version, questionsSetRetrived.Version );
        }
    }
}
