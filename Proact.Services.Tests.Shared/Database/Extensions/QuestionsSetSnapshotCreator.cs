using Proact.Services.Entities;
using Proact.Services.Models;
using Proact.Services.QueriesServices;
using System;
using System.Collections.Generic;

namespace Proact.Services.Tests.Shared {
    public static class QuestionsSetSnapshotCreator {
        public static DatabaseSnapshotProvider AddQuestionsSetWithRandomValues(
            this DatabaseSnapshotProvider snapshotProvider,
            Project project, out SurveyQuestionsSet questionsSet, bool published ) {

            var request = new QuestionsSetCreationRequest() {
                Title = "questionset title",
                Description = "questionset description",
                Version = "version 1"
            };

            questionsSet = snapshotProvider.ServiceProvider
                .GetQueriesService<ISurveyQuestionsSetQueriesService>()
                .Create( project.Id, request );

            snapshotProvider.ServiceProvider.Database.SaveChanges();

            if ( published ) {
                snapshotProvider.ServiceProvider
                    .GetQueriesService<ISurveyQuestionsSetQueriesService>()
                    .SetState( questionsSet.Id, QuestionsSetsState.PUBLISHED );

                snapshotProvider.ServiceProvider.Database.SaveChanges();
            }

            return snapshotProvider;
        }

        public static DatabaseSnapshotProvider AddQuestionsSetsWithRandomValues(
            this DatabaseSnapshotProvider snapshotProvider,
            Project project, int count, out List<SurveyQuestionsSet> questionsSets, bool published ) {

            questionsSets = new List<SurveyQuestionsSet>();

            for ( int i = 0; i < count; i++ ) {
                var request = new QuestionsSetCreationRequest() {
                    Title = $"questionset title {i}",
                    Description = $"questionset description {i}",
                    Version = $"version 1 {i}"
                };

                var questionsSet = snapshotProvider.ServiceProvider
                    .GetQueriesService<ISurveyQuestionsSetQueriesService>()
                    .Create( project.Id, request );

                snapshotProvider.ServiceProvider.Database.SaveChanges();
                questionsSets.Add( questionsSet );

                if ( published ) {
                    snapshotProvider.ServiceProvider
                        .GetQueriesService<ISurveyQuestionsSetQueriesService>()
                        .SetState( questionsSet.Id, QuestionsSetsState.PUBLISHED );

                    snapshotProvider.ServiceProvider.Database.SaveChanges();
                }
            }

            return snapshotProvider;
        }

        public static DatabaseSnapshotProvider AddOpenQuestionToQuestionsSet(
            this DatabaseSnapshotProvider snapshotProvider,
            SurveyQuestionsSet questionsSet, out SurveyQuestionModel question ) {

            var request = new OpenQuestionCreationRequest() {
                Question = $"open question",
                Title = "title open question"
            };

            question = snapshotProvider.ServiceProvider
                .GetEditorService<ISurveyQuestionsEditorService>()
                .CreateOpenQuestion( request, questionsSet );

            snapshotProvider.ServiceProvider.Database.SaveChanges();

            return snapshotProvider;
        }

        public static DatabaseSnapshotProvider AddBooleanQuestionToQuestionsSet(
            this DatabaseSnapshotProvider snapshotProvider,
            SurveyQuestionsSet questionsSet, out SurveyQuestionModel question ) {

            var request = new BoolQuestionCreationRequest() {
                Question = $"bool question",
                Title = "title bool question"
            };

            question = snapshotProvider.ServiceProvider
                .GetEditorService<ISurveyQuestionsEditorService>()
                .CreateBoolQuestion( request, questionsSet );

            snapshotProvider.ServiceProvider.Database.SaveChanges();

            return snapshotProvider;
        }

        public static DatabaseSnapshotProvider AddRatingQuestionToQuestionsSet(
            this DatabaseSnapshotProvider snapshotProvider,
            SurveyQuestionsSet questionsSet, out SurveyQuestionModel question,
            int min = 1, int max = 10 ) {

            var request = new RatingQuestionCreationRequest() {
                Question = $"rating question",
                Title = "title rating question",
                Min = min,
                Max = max
            };

            question = snapshotProvider.ServiceProvider
                .GetEditorService<ISurveyQuestionsEditorService>()
                .CreateRatingQuestion( request, questionsSet );

            snapshotProvider.ServiceProvider.Database.SaveChanges();

            return snapshotProvider;
        }

        public static DatabaseSnapshotProvider AddMoodQuestionToQuestionsSet(
            this DatabaseSnapshotProvider snapshotProvider,
            SurveyQuestionsSet questionsSet, out SurveyQuestionModel question ) {

            var request = new MoodQuestionCreationRequest() {
                Question = $"mood question",
                Title = "title mood question"
            };

            question = snapshotProvider.ServiceProvider
                .GetEditorService<ISurveyQuestionsEditorService>()
                .CreateMoodQuestion( request, questionsSet );

            snapshotProvider.ServiceProvider.Database.SaveChanges();

            return snapshotProvider;
        }

        public static DatabaseSnapshotProvider AddSingleChoiceQuestionToQuestionsSet(
            this DatabaseSnapshotProvider snapshotProvider,
            SurveyQuestionsSet questionsSet, SurveyAnswersBlock answersBlock,
            out SurveyQuestionModel question ) {

            var request = new SingleChoiceCreationRequest() {
                Question = $"single choice question",
                Title = "title single choice question",
                AnswersBlockId = answersBlock.Id
            };

            question = snapshotProvider.ServiceProvider
                .GetEditorService<ISurveyQuestionsEditorService>()
                .CreateSingleChoiceQuestion( request, questionsSet );

            snapshotProvider.ServiceProvider.Database.SaveChanges();

            return snapshotProvider;
        }

        public static DatabaseSnapshotProvider AddMultipleChoiceQuestionToQuestionsSet(
           this DatabaseSnapshotProvider snapshotProvider,
           SurveyQuestionsSet questionsSet, SurveyAnswersBlock answersBlock,
           out SurveyQuestionModel question ) {

            var request = new MultipleChoiceCreationRequest() {
                Question = $"multiple choice question",
                Title = "title multiple choice question",
                AnswersBlockId = answersBlock.Id
            };

            question = snapshotProvider.ServiceProvider
                .GetEditorService<ISurveyQuestionsEditorService>()
                .CreateMultipleChoiceQuestion( request, questionsSet );

            snapshotProvider.ServiceProvider.Database.SaveChanges();

            return snapshotProvider;
        }

        public static DatabaseSnapshotProvider AddAnswerBlockToQuestionsSet(
            this DatabaseSnapshotProvider snapshotProvider,
            SurveyQuestionsSet questionsSet, List<string> answers, 
            out SurveyAnswersBlock answersBlock ) {

            answersBlock = snapshotProvider.ServiceProvider
                .GetQueriesService<ISurveyAnswersBlockQueriesService>()
                .Create( (Guid)questionsSet.ProjectId, new AnswersBlockCreationRequest() {
                    Labels = answers
                } );

            snapshotProvider.ServiceProvider.Database.SaveChanges();

            return snapshotProvider;
        }
    }
}
