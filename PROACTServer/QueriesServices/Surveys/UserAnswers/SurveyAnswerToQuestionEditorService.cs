using Proact.Services.Entities;
using Proact.Services.Models;
using Proact.Services.UserAnswersSetter;
using System;
using System.Collections.Generic;

namespace Proact.Services.QueriesServices {
    public class SurveyAnswerToQuestionEditorService : ISurveyAnswerToQuestionEditorService {
        private readonly ISurveyAnswerToQuestionQueriesService _surveyAnswerToQuestionQueriesHelper;
        private readonly ISurveyAnswersQueriesService _surveyAnswersQueriesService; 
        private readonly ISurveyAssignationQueriesService _surveyAssignmentsQueriesService;
        private readonly ISurveyQuestionsQueriesService _surveyQuestionsQueriesService;

        private Dictionary<SurveyQuestionType, IUserAnswerSetter> _answerSetter
            = new Dictionary<SurveyQuestionType, IUserAnswerSetter>();

        public SurveyAnswerToQuestionEditorService(
            ISurveyAnswerToQuestionQueriesService surveyAnswerToQuestionQueriesHelper,
            ISurveyAnswersQueriesService surveyAnswersQueriesService,
            ISurveyAssignationQueriesService surveyAssignmentsQueriesService,
            ISurveyQuestionsQueriesService surveyQuestionsQueriesService ) {
            _surveyAnswerToQuestionQueriesHelper = surveyAnswerToQuestionQueriesHelper;
            _surveyAnswersQueriesService = surveyAnswersQueriesService;
            _surveyAssignmentsQueriesService = surveyAssignmentsQueriesService;
            _surveyQuestionsQueriesService = surveyQuestionsQueriesService;

            InitAnswerSetters();
        }

        private void InitAnswerSetters() {
            _answerSetter[SurveyQuestionType.OPEN_ANSWER]
                = new OpenQuestionUserAnswerSetter( _surveyAnswerToQuestionQueriesHelper );
            _answerSetter[SurveyQuestionType.RATING]
                = new RatingQuestionUserAnswerSetter( _surveyAnswerToQuestionQueriesHelper );
            _answerSetter[SurveyQuestionType.SINGLE_ANSWER]
                = new SingleChoiceQuestionUserAnswerSetter(
                    _surveyAnswerToQuestionQueriesHelper, _surveyAnswersQueriesService );
            _answerSetter[SurveyQuestionType.BOOLEAN]
               = new BooleanQuestionUserAnswerSetter( _surveyAnswerToQuestionQueriesHelper );
            _answerSetter[SurveyQuestionType.MOOD]
               = new MoodQuestionUserAnswerSetter( _surveyAnswerToQuestionQueriesHelper );
            _answerSetter[SurveyQuestionType.MULTIPLE_ANSWERS]
               = new MultipleChoiceQuestionUserAnswerSetter(
                   _surveyAnswerToQuestionQueriesHelper, _surveyAnswersQueriesService );
        }

        public void SetCompiledSurveyFromPatient( Guid assegnationId, SurveyCompileRequest compileRequest ) {
            var assignmentRelation = _surveyAssignmentsQueriesService.GetById( assegnationId );

            foreach ( var compiledQuestion in compileRequest.QuestionsCompiled ) {
                var question = _surveyQuestionsQueriesService.Get( compiledQuestion.QuestionId );
                
                _answerSetter[question.Type].Validate( question, compiledQuestion );
                _answerSetter[question.Type].SetUserAnswerToQuestionSurvey(
                    assignmentRelation, compiledQuestion );
            }

            assignmentRelation.Completed = true;
            assignmentRelation.CompletedDateTime = DateTime.UtcNow;
        }

        public SurveyCompiledModel GetCompiledSurveyFromPatient( Guid assegnationId ) {
            return SurveyCompiledEntityMapper.Map( 
                _surveyAssignmentsQueriesService.GetById( assegnationId ) );
        }
    }
}
