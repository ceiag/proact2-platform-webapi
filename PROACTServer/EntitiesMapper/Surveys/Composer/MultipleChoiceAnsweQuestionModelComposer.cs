using Proact.Services.Entities;
using Proact.Services.Models;
using System;
using System.Collections.Generic;

namespace Proact.Services {
    public class MultipleChoiceAnswerQuestionModelComposer : IQuestionModelComposer {
        public SurveyQuestionModel Compose(
            SurveyQuestion question, SurveyQuestionModel surveyQuestionModel ) {
            var selectableAnswers = new List<SelectableAnswerItem>();

            foreach ( var answer in question.Answers ) {
                selectableAnswers.Add( new SelectableAnswerItem() {
                    AnswerId = answer.AnswerId,
                    Label = answer.Answer.LabelId
                } );
            }

            var answerContainer = new SurveyMultipleChoiceAnswerContainer() {
                SelectableAnswers = selectableAnswers
            };

            answerContainer.AnswersBlockId = (Guid)question.Answers[0].Answer.AnswersBlockId;

            surveyQuestionModel.AnswersContainer = answerContainer;
            surveyQuestionModel.Properties
                = new SurveyNoQuestionProperties( SurveyQuestionType.MULTIPLE_ANSWERS );

            return surveyQuestionModel;
        }

        public SurveyCompiledQuestionModel Compose(
            SurveyQuestion question, SurveyCompiledQuestionModel surveyQuestionModel ) {
            var selectableAnswers = new List<SelectableAnswerItem>();

            foreach ( var answer in question.Answers ) {
                selectableAnswers.Add( new SelectableAnswerItem() {
                    AnswerId = answer.AnswerId,
                    Label = answer.Answer.LabelId
                } );
            }

            var answerContainer = new SurveyMultipleChoiceAnswerContainer() {
                SelectableAnswers = selectableAnswers
            };

            answerContainer.AnswersBlockId = (Guid)question.Answers[0].Answer.AnswersBlockId;

            surveyQuestionModel.Properties
                = new SurveyNoQuestionProperties( SurveyQuestionType.MULTIPLE_ANSWERS );

            return surveyQuestionModel;
        }
    }
}
