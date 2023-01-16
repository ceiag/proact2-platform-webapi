using Proact.Services.Entities;
using Proact.Services.Models;
using System;

namespace Proact.Services.QueriesServices {
    public interface ISurveyQuestionsEditorService : IDataEditorService {
        public SurveyQuestionModel CreateOpenQuestion(
            OpenQuestionCreationRequest creationRequest, SurveyQuestionsSet surveyQuestionsSet );
        public SurveyQuestionModel EditOpenQuestion(
            OpenQuestionEditRequest editRequest, SurveyQuestionsSet surveyQuestionsSet );
        public SurveyQuestionModel CreateRatingQuestion(
            RatingQuestionCreationRequest creationRequest, SurveyQuestionsSet surveyQuestionsSet );
        public SurveyQuestionModel EditRationQuestion(
            RatingQuestionEditRequest editRequest, SurveyQuestionsSet surveyQuestionsSet );
        public SurveyQuestionModel EditSingleChoiceQuestion(
            SingleChoiceEditRequest editRequest, SurveyQuestionsSet surveyQuestionsSet );
        public SurveyQuestionModel CreateSingleChoiceQuestion(
            SingleChoiceCreationRequest creationRequest, SurveyQuestionsSet surveyQuestionsSet );
        public SurveyQuestionModel CreateMultipleChoiceQuestion(
           MultipleChoiceCreationRequest creationRequest, SurveyQuestionsSet surveyQuestionsSet );
        public SurveyQuestionModel EditMultipleChoiceQuestion(
            MultipleChoiceEditRequest editRequest, SurveyQuestionsSet surveyQuestionsSet );
        public SurveyQuestionModel CreateBoolQuestion(
            BoolQuestionCreationRequest creationRequest, SurveyQuestionsSet surveyQuestionsSet );
        public SurveyQuestionModel EditBoolQuestion(
            BoolQuestionEditRequest editRequest, SurveyQuestionsSet surveyQuestionsSet );
        public SurveyQuestionModel CreateMoodQuestion(
            MoodQuestionCreationRequest creationRequest, SurveyQuestionsSet surveyQuestionsSet );
        public SurveyQuestionModel EditMoodQuestion(
            MoodQuestionEditRequest editRequest, SurveyQuestionsSet surveyQuestionsSet );
        public SurveyQuestionModel DeleteQuestion( Guid questionId );
        public SurveyQuestionModel GetQuestion( Guid questionId );
    }
}
