using Proact.Services.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Proact.Services.QueriesServices {
    public class SurveyAnswersQueriesService : ISurveyAnswersQueriesService {
        private readonly ProactDatabaseContext _database;

        public SurveyAnswersQueriesService( ProactDatabaseContext database ) {
            _database = database;
        }

        public SurveyAnswer Create( SurveyAnswer answer ) {
            return _database.SurveyAnswers.Add( answer ).Entity;
        }

        public SurveyAnswer Get( Guid answerId ) {
            return _database.SurveyAnswers.FirstOrDefault( x => x.Id == answerId );
        }

        public List<SurveyAnswer> GetsAll( List<Guid> answerIds ) {
            return _database.SurveyAnswers.Where( x => answerIds.Contains( x.Id ) ).ToList();
        }

        public void AddAnswerToQuestion( Guid questionId, Guid answerId ) {
            var questionAnswerRelation = new SurveyQuestionsAnswersRelation() {
                AnswerId = answerId,
                QuestionId = questionId,
            };

            _database.QuestionsAnswersRelations.Add( questionAnswerRelation );
        }
    }
}
