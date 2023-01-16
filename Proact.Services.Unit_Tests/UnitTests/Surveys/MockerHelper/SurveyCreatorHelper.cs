using Proact.Services.Entities;
using Proact.Services.Models;
using Proact.Services.QueriesServices;
using Proact.Services.ServicesProviders;
using System;
using System.Collections.Generic;
using Xunit;

namespace Proact.Services.UnitTests {
    public static class SurveyCreatorHelper {
        public static Survey CreateDummySurvey( MockDatabaseUnitTestHelper mockHelper ) {
            var request = new SurveyCreationRequest() {
                Title = $"Title {Guid.NewGuid()}",
                Description = $"Description {Guid.NewGuid()}",
                Version = $"Version {Guid.NewGuid()}",
                QuestionsIds = new List<Guid>()
            };

            var survey = mockHelper.ServicesProvider
                .GetQueriesService<ISurveyQueriesService>().Create( request );

            mockHelper.ServicesProvider.SaveChanges();

            return survey;
        }

        public static SurveyQuestionsSet CreateDummySurveyQuestionSet( MockDatabaseUnitTestHelper mockHelper ) {
            var request = new QuestionsSetCreationRequest() {
                Title = $"Title {Guid.NewGuid()}",
                Description = $"Description {Guid.NewGuid()}",
                Version = $"Version {Guid.NewGuid()}"
            };

            var questionsSet = mockHelper.ServicesProvider
                .GetQueriesService<ISurveyQuestionsSetQueriesService>().Create( request );

            mockHelper.ServicesProvider.SaveChanges();

            return questionsSet;
        }

        public static SurveyAnswersBlock CreateDummyAnswersBlock( MockDatabaseUnitTestHelper mockHelper ) {
            var request = new AnswersBlockCreationRequest() {
                Labels = new List<string>() {
                        $"nothing {Guid.NewGuid()}",
                        $"a little bit {Guid.NewGuid()}",
                        $"enough {Guid.NewGuid()}",
                        $"very {Guid.NewGuid()}",
                        $"very very much {Guid.NewGuid()}"
                    }
            };

            return mockHelper.ServicesProvider
                .GetQueriesService<ISurveyAnswersBlockQueriesService>().Create( request );
        }

        public static List<SurveyAnswersBlock> CreateDummyAnswersBlocks( 
            MockDatabaseUnitTestHelper mockHelper, int count ) {
            var answersBlocks = new List<SurveyAnswersBlock>();

            for ( int i = 0; i < count; i++ ) {
                answersBlocks.Add( CreateDummyAnswersBlock( mockHelper ) );
            }

            return answersBlocks;
        }

        public static SurveyQuestionModel CreateDummyOpenQuestion( 
            MockDatabaseUnitTestHelper mockHelper, SurveyQuestionsSet surveyQuestionsSet ) {
            var openQuestionRequest = new OpenQuestionCreationRequest() {
                Question = $"Question Title {Guid.NewGuid()}",
                Title = $"Title {Guid.NewGuid()}"
            };

            var createdQuestion = mockHelper.ServicesProvider
                .GetEditorsService<ISurveyQuestionsEditorService>()
                .CreateOpenQuestion( openQuestionRequest, surveyQuestionsSet );

            mockHelper.ServicesProvider.SaveChanges();

            return createdQuestion;
        }

        public static SurveyQuestionModel CreateDummySingleChoiceQuestion(
            MockDatabaseUnitTestHelper mockHelper, SurveyQuestionsSet surveyQuestionsSet,
            SurveyAnswersBlock surveyAnswersBlockModel ) {
            var singleChoiceQuestionRequest = new SingleChoiceCreationRequest() {
                AnswersBlockId = surveyAnswersBlockModel.Id,
                Question = $"{Guid.NewGuid()}",
                Title = $"{Guid.NewGuid()}"
            };

            var createdQuestion = mockHelper.ServicesProvider
                .GetEditorsService<ISurveyQuestionsEditorService>()
                .CreateSingleChoiceQuestion( singleChoiceQuestionRequest, surveyQuestionsSet );

            mockHelper.ServicesProvider.SaveChanges();

            return createdQuestion;
        }

        public static SurveyQuestionModel CreateDummyMultipleChoiceQuestion(
            MockDatabaseUnitTestHelper mockHelper, SurveyQuestionsSet surveyQuestionsSet,
            SurveyAnswersBlock surveyAnswersBlockModel ) {
            var multipleChoiceQuestionRequest = new MultipleChoiceCreationRequest() {
                AnswersBlockId = surveyAnswersBlockModel.Id,
                Question = $"{Guid.NewGuid()}",
                Title = $"{Guid.NewGuid()}",
            };

            var createdQuestion = mockHelper.ServicesProvider
                .GetEditorsService<ISurveyQuestionsEditorService>()
                .CreateMultipleChoiceQuestion( multipleChoiceQuestionRequest, surveyQuestionsSet );

            mockHelper.ServicesProvider.SaveChanges();

            return createdQuestion;
        }

        public static SurveyQuestionModel CreateDummyBoolQuestion(
            MockDatabaseUnitTestHelper mockHelper, SurveyQuestionsSet surveyQuestionsSet ) {
            var boolQuestionRequest = new BoolQuestionCreationRequest() {
                Question = $"{Guid.NewGuid()}",
                Title = $"{Guid.NewGuid()}",
            };

            var createdQuestion = mockHelper.ServicesProvider
                .GetEditorsService<ISurveyQuestionsEditorService>()
                .CreateBoolQuestion( boolQuestionRequest, surveyQuestionsSet );

            mockHelper.ServicesProvider.SaveChanges();

            return createdQuestion;
        }

        public static SurveyQuestionModel CreateDummyMoodQuestion(
            MockDatabaseUnitTestHelper mockHelper, SurveyQuestionsSet surveyQuestionsSet ) {
            var moodQuestionRequest = new MoodQuestionCreationRequest() {
                Question = $"{Guid.NewGuid()}",
                Title = $"{Guid.NewGuid()}",
            };

            var createdQuestion = mockHelper.ServicesProvider
                .GetEditorsService<ISurveyQuestionsEditorService>()
                .CreateMoodQuestion( moodQuestionRequest, surveyQuestionsSet );

            mockHelper.ServicesProvider.SaveChanges();

            return createdQuestion;
        }

        public static SurveyQuestionModel CreateDummyRatingQuestion(
            MockDatabaseUnitTestHelper mockHelper, SurveyQuestionsSet surveyQuestionsSet ) {
            var ratingQuestionRequest = new RatingQuestionCreationRequest() {
                Min = 0,
                Max = 100,
                MinLabel = $"min {Guid.NewGuid()}",
                MaxLabel = $"max {Guid.NewGuid()}",
                Question = $"Insert a number from 0 to 100 {Guid.NewGuid()}",
                Title = $"Title {Guid.NewGuid()}"
            };

            var createdQuestion = mockHelper.ServicesProvider
                .GetEditorsService<ISurveyQuestionsEditorService>()
                .CreateRatingQuestion( ratingQuestionRequest, surveyQuestionsSet );

            mockHelper.ServicesProvider.SaveChanges();

            return createdQuestion;
        }

        public static void AddQuestionToSurvey(
            MockDatabaseUnitTestHelper mockHelper, Guid surveyId, List<Guid> questionIds ) {

            mockHelper.ServicesProvider
                .GetQueriesService<ISurveyQueriesService>().AddQuestions( surveyId, questionIds );
        }

        public static void CheckAnswerBlockValidity( SurveyAnswersBlock answerBlock ) {
            Assert.NotNull( answerBlock );
            Assert.NotNull( answerBlock.Answers );

            foreach ( var answer in answerBlock.Answers ) {
                Assert.NotNull( answer );
                Assert.NotEmpty( answer.LabelId );
                Assert.True( answer.Id != Guid.Empty );
            }
        }
    }
}
