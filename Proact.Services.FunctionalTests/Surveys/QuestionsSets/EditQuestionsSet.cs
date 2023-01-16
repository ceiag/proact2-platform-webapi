using Proact.Services.AuthorizationPolicies;
using Proact.Services.Entities;
using Proact.Services.Models;
using Proact.Services.Tests.Shared;
using Xunit;

namespace Proact.Services.FunctionalTests.Surveys.QuestionsSets {
    public class EditQuestionsSet {
        [Fact]
        public void EditQuestionsSets_ConsistencyCheck() {
            Institute institute = null;
            Project project = null;
            MedicalTeam medicalTeam = null;
            Medic medic = null;
            SurveyQuestionsSet questionsSet = null;

            var servicesProvider = new ProactServicesProvider();
            new DatabaseSnapshotProvider( servicesProvider )
                .AddInstituteWithRandomValues( out institute )
                .AddProjectWithRandomValues( institute, out project )
                .AddMedicalTeamWithRandomValues( project, out medicalTeam )
                .AddMedicWithRandomValues( medicalTeam, out medic )
                .AddQuestionsSetWithRandomValues( project, out questionsSet, false );

            var request = new QuestionsSetEditRequest() {
                Title = "edit title",
                Description = "edit description",
                Version = "edit version"
            };

            var provider = new QuestionsSetsControllerProvider(
                servicesProvider, medic.User, Roles.MedicalTeamAdmin );
            var result = provider.Controller.EditQuestionsSet( questionsSet.Id, request );

            var questionSetsEdited = provider.GetQuestionsSetModel( project.Id, questionsSet.Id );

            Assert.Equal( request.Title, questionSetsEdited.Title );
            Assert.Equal( request.Description, questionSetsEdited.Description );
            Assert.Equal( request.Version, questionSetsEdited.Version );
        }
    }
}
