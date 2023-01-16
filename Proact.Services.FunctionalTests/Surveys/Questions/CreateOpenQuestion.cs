using Microsoft.AspNetCore.Mvc;
using Proact.Services.AuthorizationPolicies;
using Proact.Services.Entities;
using Proact.Services.Models;
using Proact.Services.Tests.Shared;
using Xunit;

namespace Proact.Services.FunctionalTests.Surveys.Questions {
    public class CreateOpenQuestion {
        [Fact]
        public void CreateOpenQuestion_ConsistencyCheck() {
            User instituteAdmin = null;
            Institute institute = null;
            Project project = null;
            MedicalTeam medicalTeam = null;
            Medic medic = null;
            SurveyQuestionsSet questionsSet = null;

            var servicesProvider = new ProactServicesProvider();
            new DatabaseSnapshotProvider( servicesProvider )
                .AddInstituteWithRandomValues( out institute )
                .AddInstituteAdminWithRandomValues( institute, out instituteAdmin )
                .AddProjectWithRandomValues( institute, out project )
                .AddMedicalTeamWithRandomValues( project, out medicalTeam )
                .AddMedicWithRandomValues( medicalTeam, out medic )
                .AddQuestionsSetWithRandomValues( project, out questionsSet, false );

            var request = new OpenQuestionCreationRequest() {
                Title = "open question title",
                Question = "open question"
            };

            var provider = new QuestionsControllerProvider(
                servicesProvider, medic.User, Roles.MedicalTeamAdmin );
            var result = provider.Controller.CreateOpenQuestion( questionsSet.Id, request );

            var openQuestionCreated = ( result as OkObjectResult ).Value as SurveyQuestionModel;

            Assert.NotNull( openQuestionCreated );
            Assert.Equal( SurveyQuestionType.OPEN_ANSWER, openQuestionCreated.Type );
            Assert.Equal( request.Question, openQuestionCreated.Question );
            Assert.Equal( request.Title, openQuestionCreated.Title );
            Assert.Equal( questionsSet.Id, openQuestionCreated.QuestionsSetId );
        }
    }
}
