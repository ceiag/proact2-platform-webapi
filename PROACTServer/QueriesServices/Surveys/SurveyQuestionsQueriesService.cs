using Proact.Services.Entities;
using Proact.Services.Models;
using System;
using System.Linq;

namespace Proact.Services.QueriesServices {
    public class SurveyQuestionsQueriesService : ISurveyQuestionsQueriesService {
        private readonly ProactDatabaseContext _database;

        public SurveyQuestionsQueriesService( ProactDatabaseContext database ) {
            _database = database;
        }

        public SurveyQuestion Create( SurveyQuestionsSet questionsSet, int order, QuestionCreationRequest creationRequest ) {
            var question = new SurveyQuestion() {
                Title = creationRequest.Title,
                Question = creationRequest.Question,
                QuestionsSet = questionsSet,
                Order = order
            };

            return _database.SurveyQuestions.Add( question ).Entity;
        }

        public SurveyQuestion Get( Guid questionId ) {
            return _database.SurveyQuestions.FirstOrDefault( x => x.Id == questionId );
        }

        public SurveyQuestion Delete( Guid questionId ) {
            return _database.SurveyQuestions.Remove( Get( questionId ) ).Entity;
        }
    }
}
