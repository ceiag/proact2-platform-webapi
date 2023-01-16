using Proact.Services.Entities;
using Proact.Services.Models;
using Proact.Services.QueriesServices;
using Proact.Services.ServicesProviders;
using System;
using Xunit;

namespace Proact.Services.UnitTests.Questions {
    public class CreationQuestionsUnitTests {
        [Fact]
        public void CheckOpenQuestionConsistency() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var surveyQuestionsSet = SurveyCreatorHelper.CreateDummySurveyQuestionSet( mockHelper );
                var survey = SurveyCreatorHelper.CreateDummySurvey( mockHelper );

                mockHelper.ServicesProvider.SaveChanges();

                var openQuestionRequest = new OpenQuestionCreationRequest() {
                    Question = "Question Title",
                    Title = "Title"
                };

                var openQuestionCreated = mockHelper.ServicesProvider
                    .GetEditorsService<ISurveyQuestionsEditorService>()
                    .CreateOpenQuestion( openQuestionRequest, surveyQuestionsSet );

                Assert.NotNull( openQuestionCreated );
                Assert.NotNull( openQuestionCreated.Question );
                Assert.NotNull( openQuestionCreated.Title );
                Assert.Equal( SurveyQuestionType.OPEN_ANSWER, openQuestionCreated.Properties.Type );
                Assert.Equal( SurveyQuestionType.OPEN_ANSWER, openQuestionCreated.AnswersContainer.Type );               
            }
        }

        [Fact]
        public void CheckEditOpenQuestionConsistency() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var surveyQuestionsSet = SurveyCreatorHelper.CreateDummySurveyQuestionSet( mockHelper );
                var survey = SurveyCreatorHelper.CreateDummySurvey( mockHelper );

                mockHelper.ServicesProvider.SaveChanges();

                var question_0 = SurveyCreatorHelper
                    .CreateDummyOpenQuestion( mockHelper, surveyQuestionsSet );
                var question_1 = SurveyCreatorHelper
                    .CreateDummyOpenQuestion( mockHelper, surveyQuestionsSet );
                var question_2 = SurveyCreatorHelper
                    .CreateDummyOpenQuestion( mockHelper, surveyQuestionsSet );

                mockHelper.ServicesProvider.SaveChanges();

                var editOpenQuestion = new OpenQuestionEditRequest() {
                    QuestionId = question_1.Id,
                    Question = Guid.NewGuid().ToString(),
                    Title = Guid.NewGuid().ToString()
                };

                var editedQuestion = mockHelper.ServicesProvider
                    .GetEditorsService<ISurveyQuestionsEditorService>()
                    .EditOpenQuestion( editOpenQuestion, surveyQuestionsSet );

                mockHelper.ServicesProvider.SaveChanges();

                var editedQuestionRetrieved = mockHelper.ServicesProvider
                    .GetEditorsService<ISurveyQuestionsEditorService>()
                    .GetQuestion( editedQuestion.Id );

                Assert.NotNull( editedQuestionRetrieved );
                Assert.NotNull( editedQuestionRetrieved.Question );
                Assert.NotNull( editedQuestionRetrieved.Title );
                Assert.Equal( SurveyQuestionType.OPEN_ANSWER, editedQuestionRetrieved.Properties.Type );
                Assert.Equal( SurveyQuestionType.OPEN_ANSWER, editedQuestionRetrieved.AnswersContainer.Type );
                Assert.Equal( question_1.Order, editedQuestionRetrieved.Order );
            }
        }

        private void RatingQuestionEqual( 
            RatingQuestionCreationRequest request, SurveyQuestionModel actual ) {
            var minMaxPropsActual = ( actual.Properties as SurveyMinMaxQuestionProperties );

            Assert.NotNull( actual );
            Assert.NotNull( actual.Question );
            Assert.NotNull( actual.Title );
            Assert.Equal( SurveyQuestionType.RATING, actual.Properties.Type );
            Assert.Equal( SurveyQuestionType.RATING, actual.AnswersContainer.Type );
            Assert.Equal( request.Min, minMaxPropsActual.Min );
            Assert.Equal( request.Max, minMaxPropsActual.Max );
            Assert.Equal( request.MinLabel, minMaxPropsActual.MinLabel );
            Assert.Equal( request.MaxLabel, minMaxPropsActual.MaxLabel );
        }

        [Fact]
        public void CheckRatingQuestionConsistency() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var surveyQuestionsSet = SurveyCreatorHelper.CreateDummySurveyQuestionSet( mockHelper );
                var survey = SurveyCreatorHelper.CreateDummySurvey( mockHelper );

                mockHelper.ServicesProvider.SaveChanges();

                var ratingQuestionRequest = new RatingQuestionCreationRequest() {
                    Min = 0,
                    Max = 100,
                    MinLabel = "min",
                    MaxLabel = "max",
                    Question = "Insert a number from 0 to 100",
                    Title = "Title"
                };

                var ratingQuestionCreated = mockHelper.ServicesProvider
                    .GetEditorsService<ISurveyQuestionsEditorService>()
                    .CreateRatingQuestion( ratingQuestionRequest, surveyQuestionsSet );

                RatingQuestionEqual( ratingQuestionRequest, ratingQuestionCreated );
            }
        }

        [Fact]
        public void CheckEditRatingQuestionConsistency() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var surveyQuestionsSet = SurveyCreatorHelper.CreateDummySurveyQuestionSet( mockHelper );
                var survey = SurveyCreatorHelper.CreateDummySurvey( mockHelper );
                
                mockHelper.ServicesProvider.SaveChanges();

                var question_0 = SurveyCreatorHelper
                    .CreateDummyRatingQuestion( mockHelper, surveyQuestionsSet );
                var question_1 = SurveyCreatorHelper
                    .CreateDummyRatingQuestion( mockHelper, surveyQuestionsSet );
                var question_2 = SurveyCreatorHelper
                    .CreateDummyRatingQuestion( mockHelper, surveyQuestionsSet );

                mockHelper.ServicesProvider.SaveChanges();

                var editQuestionRequest = new RatingQuestionEditRequest() {
                    QuestionId = question_1.Id,
                    Min = 0,
                    Max = 200,
                    MinLabel = "min_edit",
                    MaxLabel = "max_edit",
                    Question = "Insert a number from 0 to 100 edit",
                    Title = "Title edit"
                };

                var editedQuestion = mockHelper.ServicesProvider
                    .GetEditorsService<ISurveyQuestionsEditorService>()
                    .EditRationQuestion( editQuestionRequest, surveyQuestionsSet );

                mockHelper.ServicesProvider.SaveChanges();

                var editedQuestionRetrieved = mockHelper.ServicesProvider
                    .GetEditorsService<ISurveyQuestionsEditorService>()
                    .GetQuestion( editedQuestion.Id );

                RatingQuestionEqual( editQuestionRequest, editedQuestionRetrieved );
                Assert.Equal( question_1.Order, editedQuestionRetrieved.Order );
            }
        }

        private void SingleChoiceQuestionEqual( 
            SingleChoiceCreationRequest request, 
            SurveyAnswersBlock expectedAnswerBlock, SurveyQuestionModel actual ) {

            var answerContainer = actual.AnswersContainer as SurveySingleChoiceAnswerContainer;

            Assert.NotNull( actual );
            Assert.Equal( request.Question, actual.Question );
            Assert.Equal( request.Title, actual.Title );
            Assert.Equal( expectedAnswerBlock.Id, answerContainer.AnswersBlockId );
            Assert.Equal( expectedAnswerBlock.Answers.Count, answerContainer.SelectableAnswers.Count );
            Assert.Equal( SurveyQuestionType.SINGLE_ANSWER, actual.AnswersContainer.Type );
            Assert.Equal( SurveyQuestionType.SINGLE_ANSWER, actual.Properties.Type );
            Assert.NotNull( actual.Question );
            Assert.NotNull( actual.Title );
        }

        [Fact]
        public void CheckSingleChoiceQuestionConsistency() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var surveyQuestionsSet = SurveyCreatorHelper.CreateDummySurveyQuestionSet( mockHelper );
                var survey = SurveyCreatorHelper.CreateDummySurvey( mockHelper );
                var answersBlock = SurveyCreatorHelper.CreateDummyAnswersBlock( mockHelper );

                mockHelper.ServicesProvider.SaveChanges();

                var singleChoiceQuestionRequest = new SingleChoiceCreationRequest() {
                    AnswersBlockId = answersBlock.Id,
                    Question = "Select a value.",
                    Title = "Title select a value"
                };

                var singleChoiceQuestionCreated = mockHelper.ServicesProvider
                        .GetEditorsService<ISurveyQuestionsEditorService>()
                        .CreateSingleChoiceQuestion( singleChoiceQuestionRequest, surveyQuestionsSet );

                SingleChoiceQuestionEqual( 
                    singleChoiceQuestionRequest, answersBlock, singleChoiceQuestionCreated );
            }
        }

        [Fact]
        public void CheckEditSingleChoiceQuestionConsistency() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var surveyQuestionsSet = SurveyCreatorHelper.CreateDummySurveyQuestionSet( mockHelper );
                var survey = SurveyCreatorHelper.CreateDummySurvey( mockHelper );
                var answersBlock = SurveyCreatorHelper.CreateDummyAnswersBlock( mockHelper );

                mockHelper.ServicesProvider.SaveChanges();

                var question_0 = SurveyCreatorHelper
                    .CreateDummySingleChoiceQuestion( mockHelper, surveyQuestionsSet, answersBlock );
                var question_1 = SurveyCreatorHelper
                    .CreateDummySingleChoiceQuestion( mockHelper, surveyQuestionsSet, answersBlock );
                var question_2 = SurveyCreatorHelper
                    .CreateDummySingleChoiceQuestion( mockHelper, surveyQuestionsSet, answersBlock );

                mockHelper.ServicesProvider.SaveChanges();

                var editQuestionRequest = new SingleChoiceEditRequest() {
                    QuestionId = question_1.Id,
                    AnswersBlockId = answersBlock.Id,
                    Question = "Select a value.",
                    Title = "Title select a value"
                };

                var editedQuestion = mockHelper.ServicesProvider
                    .GetEditorsService<ISurveyQuestionsEditorService>()
                    .EditSingleChoiceQuestion( editQuestionRequest, surveyQuestionsSet );

                mockHelper.ServicesProvider.SaveChanges();

                var editedQuestionRetrieved = mockHelper.ServicesProvider
                    .GetEditorsService<ISurveyQuestionsEditorService>()
                    .GetQuestion( editedQuestion.Id );

                SingleChoiceQuestionEqual(
                    editQuestionRequest, answersBlock, editedQuestionRetrieved );
                Assert.Equal( question_1.Order, editedQuestionRetrieved.Order );
            }
        }

        private void MultipleChoiceQuestionEqual( 
            MultipleChoiceCreationRequest request,
            SurveyAnswersBlock expectedAnswerBlock, SurveyQuestionModel actual ) {

            var answerContainer = ( actual.AnswersContainer as SurveyMultipleChoiceAnswerContainer );

            Assert.NotNull( actual );
            Assert.Equal( expectedAnswerBlock.Answers.Count, answerContainer.SelectableAnswers.Count );
            Assert.Equal( expectedAnswerBlock.Id, answerContainer.AnswersBlockId );
            Assert.Equal( SurveyQuestionType.MULTIPLE_ANSWERS, actual.AnswersContainer.Type );
            Assert.Equal( SurveyQuestionType.MULTIPLE_ANSWERS, actual.Properties.Type );
            Assert.Equal( request.Question, actual.Question );
            Assert.Equal( request.Title, actual.Title );
        }

        [Fact]
        public void CheckMultipleChoiceQuestionConsistency() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var surveyQuestionsSet = SurveyCreatorHelper.CreateDummySurveyQuestionSet( mockHelper );
                var survey = SurveyCreatorHelper.CreateDummySurvey( mockHelper );
                var answersBlock = SurveyCreatorHelper.CreateDummyAnswersBlock( mockHelper );

                mockHelper.ServicesProvider.SaveChanges();

                var multipleChoiceQuestionRequest = new MultipleChoiceCreationRequest() {
                    AnswersBlockId = answersBlock.Id,
                    Question = "Select multiple values.",
                    Title = "Title select multiple value"
                };

                var multipleChoiceQuestionCreated = mockHelper.ServicesProvider
                    .GetEditorsService<ISurveyQuestionsEditorService>()
                    .CreateMultipleChoiceQuestion( multipleChoiceQuestionRequest, surveyQuestionsSet );

                MultipleChoiceQuestionEqual( 
                    multipleChoiceQuestionRequest, answersBlock, multipleChoiceQuestionCreated );
            }
        }

        [Fact]
        public void CheckEditMultipleChoiceQuestionConsistency() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var surveyQuestionsSet = SurveyCreatorHelper.CreateDummySurveyQuestionSet( mockHelper );
                var survey = SurveyCreatorHelper.CreateDummySurvey( mockHelper );
                var answersBlock = SurveyCreatorHelper.CreateDummyAnswersBlock( mockHelper );

                mockHelper.ServicesProvider.SaveChanges();

                var question_0 = SurveyCreatorHelper
                    .CreateDummyMultipleChoiceQuestion( mockHelper, surveyQuestionsSet, answersBlock );
                var question_1 = SurveyCreatorHelper
                    .CreateDummyMultipleChoiceQuestion( mockHelper, surveyQuestionsSet, answersBlock );
                var question_2 = SurveyCreatorHelper
                    .CreateDummyMultipleChoiceQuestion( mockHelper, surveyQuestionsSet, answersBlock );

                mockHelper.ServicesProvider.SaveChanges();

                var editQuestionRequest = new MultipleChoiceEditRequest() {
                    QuestionId = question_1.Id,
                    AnswersBlockId = answersBlock.Id,
                    Question = "Select a value.",
                    Title = "Title select a value"
                };

                var editedQuestion = mockHelper.ServicesProvider
                    .GetEditorsService<ISurveyQuestionsEditorService>()
                    .EditMultipleChoiceQuestion( editQuestionRequest, surveyQuestionsSet );

                mockHelper.ServicesProvider.SaveChanges();

                var editedQuestionRetrieved = mockHelper.ServicesProvider
                    .GetEditorsService<ISurveyQuestionsEditorService>()
                    .GetQuestion( editedQuestion.Id );

                MultipleChoiceQuestionEqual( editQuestionRequest, answersBlock, editedQuestionRetrieved );
                Assert.Equal( question_1.Order, editedQuestionRetrieved.Order );
            }
        }

        private void BoolQuestionEqual( BoolQuestionCreationRequest request, SurveyQuestionModel actual ) {
            Assert.NotNull( actual );
            Assert.Equal( request.Question, actual.Question );
            Assert.Equal( request.Title, actual.Title );
            Assert.Equal( SurveyQuestionType.BOOLEAN, actual.Properties.Type );
            Assert.Equal( SurveyQuestionType.BOOLEAN, actual.AnswersContainer.Type );
        }

        [Fact]
        public void CheckBoolQuestionConsistency() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var surveyQuestionsSet = SurveyCreatorHelper.CreateDummySurveyQuestionSet( mockHelper );
                var survey = SurveyCreatorHelper.CreateDummySurvey( mockHelper );

                mockHelper.ServicesProvider.SaveChanges();

                var boolQuestionRequest = new BoolQuestionCreationRequest() {
                    Question = "Is this a bool question?",
                    Title = "Title of is this a bool question?"
                };

                var boolQuestionCreated = mockHelper.ServicesProvider
                    .GetEditorsService<ISurveyQuestionsEditorService>()
                    .CreateBoolQuestion( boolQuestionRequest, surveyQuestionsSet );

                BoolQuestionEqual( boolQuestionRequest, boolQuestionCreated );
            }
        }

        [Fact]
        public void CheckEditBoolQuestionConsistency() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var surveyQuestionsSet = SurveyCreatorHelper.CreateDummySurveyQuestionSet( mockHelper );
                var survey = SurveyCreatorHelper.CreateDummySurvey( mockHelper );
               
                mockHelper.ServicesProvider.SaveChanges();

                var question_0 = SurveyCreatorHelper
                    .CreateDummyBoolQuestion( mockHelper, surveyQuestionsSet );
                var question_1 = SurveyCreatorHelper
                    .CreateDummyBoolQuestion( mockHelper, surveyQuestionsSet );
                var question_2 = SurveyCreatorHelper
                    .CreateDummyBoolQuestion( mockHelper, surveyQuestionsSet );

                mockHelper.ServicesProvider.SaveChanges();

                var editQuestionRequest = new BoolQuestionEditRequest() {
                    QuestionId = question_1.Id,
                    Question = "Is this a bool question?",
                    Title = "Title of is this a bool question?"
                };

                var editedQuestion = mockHelper.ServicesProvider
                    .GetEditorsService<ISurveyQuestionsEditorService>()
                    .EditBoolQuestion( editQuestionRequest, surveyQuestionsSet );

                mockHelper.ServicesProvider.SaveChanges();

                var editedQuestionRetrieved = mockHelper.ServicesProvider
                    .GetEditorsService<ISurveyQuestionsEditorService>()
                    .GetQuestion( editedQuestion.Id );

                BoolQuestionEqual( editQuestionRequest, editedQuestionRetrieved );
                Assert.Equal( question_1.Order, editedQuestionRetrieved.Order );
            }
        }

        private void MoodQuestionEqual( MoodQuestionCreationRequest request, SurveyQuestionModel actual ) {
            Assert.NotNull( actual );
            Assert.Equal( request.Question, actual.Question );
            Assert.Equal( request.Title, actual.Title );
            Assert.Equal( SurveyQuestionType.MOOD, actual.Properties.Type );
            Assert.Equal( SurveyQuestionType.MOOD, actual.AnswersContainer.Type );
        }

        [Fact]
        public void CheckMoodQuestionConsistency() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var surveyQuestionsSet = SurveyCreatorHelper.CreateDummySurveyQuestionSet( mockHelper );
                var survey = SurveyCreatorHelper.CreateDummySurvey( mockHelper );

                mockHelper.ServicesProvider.SaveChanges();

                var moodQuestionRequest = new MoodQuestionCreationRequest() {
                    Question = "what's your mood?",
                    Title = "what's your mood?"
                };

                var moodQuestionCreated = mockHelper.ServicesProvider
                    .GetEditorsService<ISurveyQuestionsEditorService>()
                    .CreateMoodQuestion( moodQuestionRequest, surveyQuestionsSet );

                MoodQuestionEqual( moodQuestionRequest, moodQuestionCreated );
            }
        }

        [Fact]
        public void CheckEditMoodQuestionConsistency() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var surveyQuestionsSet = SurveyCreatorHelper.CreateDummySurveyQuestionSet( mockHelper );
                var survey = SurveyCreatorHelper.CreateDummySurvey( mockHelper );

                mockHelper.ServicesProvider.SaveChanges();

                var question_0 = SurveyCreatorHelper
                    .CreateDummyMoodQuestion( mockHelper, surveyQuestionsSet );
                var question_1 = SurveyCreatorHelper
                    .CreateDummyMoodQuestion( mockHelper, surveyQuestionsSet );
                var question_2 = SurveyCreatorHelper
                    .CreateDummyMoodQuestion( mockHelper, surveyQuestionsSet );

                mockHelper.ServicesProvider.SaveChanges();

                var editQuestionRequest = new MoodQuestionEditRequest() {
                    QuestionId = question_1.Id,
                    Question = "Is this a bool question?",
                    Title = "Title of is this a bool question?"
                };

                var editedQuestion = mockHelper.ServicesProvider
                    .GetEditorsService<ISurveyQuestionsEditorService>()
                    .EditMoodQuestion( editQuestionRequest, surveyQuestionsSet );

                mockHelper.ServicesProvider.SaveChanges();

                var editedQuestionRetrieved = mockHelper.ServicesProvider
                    .GetEditorsService<ISurveyQuestionsEditorService>()
                    .GetQuestion( editedQuestion.Id );

                MoodQuestionEqual( editQuestionRequest, editedQuestionRetrieved );
                Assert.Equal( question_1.Order, editedQuestionRetrieved.Order );
            }
        }

        [Fact]
        public void GetQuestionConsistencyCheck() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var surveyQuestionsSet = SurveyCreatorHelper.CreateDummySurveyQuestionSet( mockHelper );
                var survey = SurveyCreatorHelper.CreateDummySurvey( mockHelper );
                var answersBlock = SurveyCreatorHelper.CreateDummyAnswersBlock( mockHelper );

                mockHelper.ServicesProvider.SaveChanges();

                var multipleChoiceQuestionRequest = new MultipleChoiceCreationRequest() {
                    AnswersBlockId = answersBlock.Id,
                    Question = "Select multiple values.",
                    Title = "Title select multiple value"
                };

                var multipleChoiceQuestionCreated = mockHelper.ServicesProvider
                    .GetEditorsService<ISurveyQuestionsEditorService>()
                    .CreateMultipleChoiceQuestion( multipleChoiceQuestionRequest, surveyQuestionsSet );

                mockHelper.ServicesProvider.SaveChanges();

                var questionRetrieved = mockHelper.ServicesProvider
                    .GetEditorsService<ISurveyQuestionsEditorService>()
                    .GetQuestion( multipleChoiceQuestionCreated.Id );

                Assert.NotNull( questionRetrieved );
                Assert.Equal( multipleChoiceQuestionCreated.Question, questionRetrieved.Question );
                Assert.Equal( multipleChoiceQuestionCreated.Title, questionRetrieved.Title );
            }
        }

        [Fact]
        public void DeleteQuestionCheck() {
            using ( var mockHelper = new MockDatabaseUnitTestHelper() ) {
                var surveyQuestionsSet = SurveyCreatorHelper.CreateDummySurveyQuestionSet( mockHelper );
                var survey = SurveyCreatorHelper.CreateDummySurvey( mockHelper );
                var answersBlock = SurveyCreatorHelper.CreateDummyAnswersBlock( mockHelper );

                mockHelper.ServicesProvider.SaveChanges();

                var multipleChoiceQuestionRequest = new MultipleChoiceCreationRequest() {
                    AnswersBlockId = answersBlock.Id,
                    Question = "Select multiple values.",
                    Title = "Title select multiple value"
                };

                var multipleChoiceQuestionCreated = mockHelper.ServicesProvider
                    .GetEditorsService<ISurveyQuestionsEditorService>()
                    .CreateMultipleChoiceQuestion( multipleChoiceQuestionRequest, surveyQuestionsSet );

                mockHelper.ServicesProvider.SaveChanges();

                var deletedQuestion = mockHelper.ServicesProvider
                    .GetEditorsService<ISurveyQuestionsEditorService>()
                    .DeleteQuestion( multipleChoiceQuestionCreated.Id );

                mockHelper.ServicesProvider.SaveChanges();

                var questionRetrieved = mockHelper.ServicesProvider
                    .GetEditorsService<ISurveyQuestionsEditorService>()
                    .GetQuestion( multipleChoiceQuestionCreated.Id );

                Assert.Null( questionRetrieved );
                Assert.True( surveyQuestionsSet.Questions.Count == 0 );
            }
        }
    }
}
