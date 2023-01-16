using Newtonsoft.Json;
using Proact.Services.Entities;
using Proact.Services.Models;
using System;

namespace Proact.Services.QueriesServices {
    public class SurveyQuestionsEditorService : ISurveyQuestionsEditorService {
        private readonly ISurveyQuestionsQueriesService _surveyQueriesHelper;
        private readonly ISurveyAnswersQueriesService _answersQueriesHelper;
        private readonly ISurveyQuestionsSetQueriesService _surveyQuestionsSetQueriesService;
        private readonly ISurveyAnswersBlockQueriesService _surveyAnswersBlockQueriesService;

        public SurveyQuestionsEditorService( 
            ISurveyQuestionsQueriesService surveyQueriesHelper, 
            ISurveyAnswersQueriesService answersQueriesHelper,
            ISurveyQuestionsSetQueriesService surveyQuestionsSetQueriesService,
            ISurveyAnswersBlockQueriesService surveyAnswersBlockQueriesService ) {
            _surveyQueriesHelper = surveyQueriesHelper;
            _answersQueriesHelper = answersQueriesHelper;
            _surveyQuestionsSetQueriesService = surveyQuestionsSetQueriesService;
            _surveyAnswersBlockQueriesService = surveyAnswersBlockQueriesService;
        }

        private SurveyQuestion CreateBaseQuestion( 
            QuestionCreationRequest creationRequest, SurveyQuestionsSet surveyQuestionsSet ) {
            int order = _surveyQuestionsSetQueriesService
                .GetGreatestQuestionOrder( surveyQuestionsSet.Id ) + 1;

            return _surveyQueriesHelper.Create( surveyQuestionsSet, order, creationRequest );
        }

        public SurveyQuestionModel CreateOpenQuestion(
            OpenQuestionCreationRequest creationRequest, SurveyQuestionsSet surveyQuestionsSet ) {
            var question = CreateBaseQuestion( creationRequest, surveyQuestionsSet );

            return CreateOpenQuestionWithAttributes( question );
        }

        public SurveyQuestionModel EditOpenQuestion(
            OpenQuestionEditRequest editRequest, SurveyQuestionsSet surveyQuestionsSet ) {
            var deletedQuestion = DeleteQuestion( editRequest.QuestionId );

            var question = CreateBaseQuestion( editRequest, surveyQuestionsSet );
            question.Order = deletedQuestion.Order;

            return CreateOpenQuestionWithAttributes( question );
        }

        private SurveyQuestionModel CreateOpenQuestionWithAttributes( SurveyQuestion question ) {
            question.Type = SurveyQuestionType.OPEN_ANSWER;

            return SurveysEntityMapper.Map( question );
        }

        public SurveyQuestionModel CreateRatingQuestion(
            RatingQuestionCreationRequest creationRequest, SurveyQuestionsSet surveyQuestionsSet ) {
            var question = CreateBaseQuestion( creationRequest, surveyQuestionsSet );

            return CreateRatingQuestionsWithAttributes( creationRequest, surveyQuestionsSet, question );
        }

        public SurveyQuestionModel EditRationQuestion(
            RatingQuestionEditRequest editRequest, SurveyQuestionsSet surveyQuestionsSet ) {
            var deletedQuestion = DeleteQuestion( editRequest.QuestionId );

            var question = CreateBaseQuestion( editRequest, surveyQuestionsSet );
            question.Order = deletedQuestion.Order;

            return CreateRatingQuestionsWithAttributes( editRequest, surveyQuestionsSet, question );
        }

        private SurveyQuestionModel CreateRatingQuestionsWithAttributes( 
            RatingQuestionCreationRequest creationRequest, 
            SurveyQuestionsSet surveyQuestionsSet, SurveyQuestion question ) {
            question.Type = SurveyQuestionType.RATING;

            var ratingProperties = new SurveyMinMaxQuestionProperties() {
                Max = creationRequest.Max,
                Min = creationRequest.Min,
                MinLabel = creationRequest.MinLabel,
                MaxLabel = creationRequest.MaxLabel
            };

            var answer = new SurveyAnswer() {
                LabelId = "rating",
                SerializedProperties = JsonConvert.SerializeObject( ratingProperties ),
            };

            _answersQueriesHelper.Create( answer );
            _answersQueriesHelper.AddAnswerToQuestion( question.Id, answer.Id );

            return SurveysEntityMapper.Map( question );
        }

        public SurveyQuestionModel EditSingleChoiceQuestion(
            SingleChoiceEditRequest editRequest, SurveyQuestionsSet surveyQuestionsSet ) {
            var deletedQuestion = DeleteQuestion( editRequest.QuestionId );

            var question = CreateBaseQuestion( editRequest, surveyQuestionsSet );
            question.Order = deletedQuestion.Order;

            return CreateSingleChoiceQuestionsWithAttributes( editRequest, question );
        }

        private SurveyQuestionModel CreateSingleChoiceQuestionsWithAttributes(
            SingleChoiceCreationRequest creationRequest, SurveyQuestion question ) {
            question.Type = SurveyQuestionType.SINGLE_ANSWER;

            var answersBlock = _surveyAnswersBlockQueriesService.Get( creationRequest.AnswersBlockId );

            foreach ( var answerItem in answersBlock.Answers ) {
                _answersQueriesHelper.AddAnswerToQuestion( question.Id, answerItem.Id );
            }

            return SurveysEntityMapper.Map( question );
        }

        public SurveyQuestionModel CreateSingleChoiceQuestion(
            SingleChoiceCreationRequest creationRequest, SurveyQuestionsSet surveyQuestionsSet ) {
            var question = CreateBaseQuestion( creationRequest, surveyQuestionsSet );

            return CreateSingleChoiceQuestionsWithAttributes( creationRequest, question );
        }

        public SurveyQuestionModel CreateMultipleChoiceQuestion( 
            MultipleChoiceCreationRequest creationRequest, SurveyQuestionsSet surveyQuestionsSet ) {
            var question = CreateBaseQuestion( creationRequest, surveyQuestionsSet );

            return CreateMultipleChoiceQuestionsWithAttributes( creationRequest, question );
        }

        public SurveyQuestionModel EditMultipleChoiceQuestion( 
            MultipleChoiceEditRequest editRequest, SurveyQuestionsSet surveyQuestionsSet ) {
            var deletedQuestion = DeleteQuestion( editRequest.QuestionId );

            var question = CreateBaseQuestion( editRequest, surveyQuestionsSet );
            question.Order = deletedQuestion.Order;

            return CreateMultipleChoiceQuestionsWithAttributes( editRequest, question );
        }

        private SurveyQuestionModel CreateMultipleChoiceQuestionsWithAttributes(
           MultipleChoiceCreationRequest creationRequest, SurveyQuestion question ) {
            question.Type = SurveyQuestionType.MULTIPLE_ANSWERS;

            var answersBlock = _surveyAnswersBlockQueriesService.Get( creationRequest.AnswersBlockId );

            foreach ( var answerItem in answersBlock.Answers ) {
                _answersQueriesHelper.AddAnswerToQuestion( question.Id, answerItem.Id );
            }

            return SurveysEntityMapper.Map( question );
        }

        public SurveyQuestionModel CreateBoolQuestion(
            BoolQuestionCreationRequest creationRequest, SurveyQuestionsSet surveyQuestionsSet ) {
            var question = CreateBaseQuestion( creationRequest, surveyQuestionsSet );

            return CreateBoolQuestionsWithAttributes( question );
        }

        public SurveyQuestionModel EditBoolQuestion(
            BoolQuestionEditRequest editRequest, SurveyQuestionsSet surveyQuestionsSet ) {
            var deletedQuestion = DeleteQuestion( editRequest.QuestionId );

            var question = CreateBaseQuestion( editRequest, surveyQuestionsSet );
            question.Order = deletedQuestion.Order;

            return CreateBoolQuestionsWithAttributes( question );
        }

        private SurveyQuestionModel CreateBoolQuestionsWithAttributes( SurveyQuestion question ) {
            question.Type = SurveyQuestionType.BOOLEAN;

            return SurveysEntityMapper.Map( question );
        }

        public SurveyQuestionModel CreateMoodQuestion( 
            MoodQuestionCreationRequest creationRequest, SurveyQuestionsSet surveyQuestionsSet ) {
            var question = CreateBaseQuestion( creationRequest, surveyQuestionsSet );

            return CreateMoodQuestionsWithAttributes( question );
        }

        public SurveyQuestionModel EditMoodQuestion(
            MoodQuestionEditRequest editRequest, SurveyQuestionsSet surveyQuestionsSet ) {
            var deletedQuestion = DeleteQuestion( editRequest.QuestionId );

            var question = CreateBaseQuestion( editRequest, surveyQuestionsSet );
            question.Order = deletedQuestion.Order;

            return CreateMoodQuestionsWithAttributes( question );
        }

        private SurveyQuestionModel CreateMoodQuestionsWithAttributes( SurveyQuestion question ) {
            question.Type = SurveyQuestionType.MOOD;

            return SurveysEntityMapper.Map( question );
        }

        public SurveyQuestionModel DeleteQuestion( Guid questionId ) {
            return SurveysEntityMapper.Map( 
                _surveyQueriesHelper.Delete( questionId ) );
        }

        public SurveyQuestionModel GetQuestion( Guid questionId ) {
            return SurveysEntityMapper.Map( 
                _surveyQueriesHelper.Get( questionId ) );
        }
    }
}
