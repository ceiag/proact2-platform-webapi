using Proact.Services.AuthorizationPolicies;
using Proact.Services.Entities;
using Proact.Services.Tests.Shared;
using Xunit;

namespace Proact.Services.FunctionalTests.Surveys.QuestionsSets {
    public class PublishQuestionsSet {
        [Fact]
        public void PublishQuestionsSets_ConsistencyCheck() {
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

            var provider = new QuestionsSetsControllerProvider(
                servicesProvider, medic.User, Roles.MedicalTeamAdmin );
            var result = provider.Controller.PublishQuestionsSet( questionsSet.Id );

            var questionSetsEdited = provider.GetQuestionsSetModel( project.Id, questionsSet.Id );

            Assert.Equal( QuestionsSetsState.PUBLISHED, questionSetsEdited.State );
        }
    }
}
