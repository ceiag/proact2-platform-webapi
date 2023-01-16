using Proact.Services.Entities;
using Proact.Services.Models;
using System.Collections.Generic;

namespace Proact.Services {
    public class SurveyQuestionComposerProvider {
        private Dictionary<SurveyQuestionType, IQuestionModelComposer> _decorators
            = new Dictionary<SurveyQuestionType, IQuestionModelComposer>();

        public SurveyQuestionComposerProvider() {
            _decorators[SurveyQuestionType.OPEN_ANSWER] = new OpenAnswerQuestionModelComposer();
            _decorators[SurveyQuestionType.RATING] = new RatingAnswerQuestionModelComposer();
            _decorators[SurveyQuestionType.SINGLE_ANSWER] = new SingleChoiceAnswerQuestionModelComposer();
            _decorators[SurveyQuestionType.MULTIPLE_ANSWERS] = new MultipleChoiceAnswerQuestionModelComposer();
            _decorators[SurveyQuestionType.BOOLEAN] = new BoolAnswerQuestionModelComposer();
            _decorators[SurveyQuestionType.MOOD] = new MoodAnswerQuestionModelComposer();
        }

        public SurveyQuestionModel Compose( SurveyQuestion question, SurveyQuestionModel surveyQuestionModel ) {
            return _decorators[question.Type].Compose( question, surveyQuestionModel );
        }

        public SurveyCompiledQuestionModel Compose( 
            SurveyQuestion question, SurveyCompiledQuestionModel surveyQuestionModel ) {
            return _decorators[question.Type].Compose( question, surveyQuestionModel );
        }
    }
}
