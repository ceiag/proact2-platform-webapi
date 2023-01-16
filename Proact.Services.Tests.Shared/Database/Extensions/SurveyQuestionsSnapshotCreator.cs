using Proact.Services.Entities;
using Proact.Services.QueriesServices;
using System;
using System.Linq;

namespace Proact.Services.Tests.Shared {
    public static class SurveyQuestionSnapshotCreator {
        public static DatabaseSnapshotProvider AddQuestionsFromQuestionsSet(
            this DatabaseSnapshotProvider snapshotProvider, Survey survey, SurveyQuestionsSet questionsSet ) {

            var request = new SurveyCreationRequest() {
                Title = $"survey-title-{Guid.NewGuid()}",
                Description = $"survey-descr-{Guid.NewGuid()}",
                Version = $"v-{Guid.NewGuid()}"
            };

            snapshotProvider.ServiceProvider
                .GetQueriesService<ISurveyQueriesService>()
                .AddQuestions( survey.Id, questionsSet.Questions.Select( x => x.Id ).ToList() );

            snapshotProvider.ServiceProvider.Database.SaveChanges();

            return snapshotProvider;
        }
    }
}
