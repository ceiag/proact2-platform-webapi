using Microsoft.EntityFrameworkCore;
using Proact.Services.Entities;
using Proact.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Proact.Services.QueriesServices {
    public class SurveyQuestionsSetQueriesService : ISurveyQuestionsSetQueriesService {
        private readonly ProactDatabaseContext _database;

        public SurveyQuestionsSetQueriesService( ProactDatabaseContext database ) {
            _database = database;
        }

        public SurveyQuestionsSet Create( Guid projectId, QuestionsSetCreationRequest request ) {
            var questionsSet = new SurveyQuestionsSet() {
                Title = request.Title,
                Description = request.Description,
                Version = request.Version,
                ProjectId = projectId,
            };

            return _database.SurveyQuestionsSets.Add( questionsSet ).Entity;
        }

        public SurveyQuestionsSet Edit( Guid questionsSetId, QuestionsSetEditRequest request ) {
            var oldQuestionsSet = Get( questionsSetId );

            if ( oldQuestionsSet != null ) {
                oldQuestionsSet.Version = request.Version;
                oldQuestionsSet.Description = request.Description;
                oldQuestionsSet.Title = request.Title;

                return oldQuestionsSet;
            }
            else {
                return null;
            }
        }

        public SurveyQuestionsSet Get( Guid questionsSetId ) {
            return _database.SurveyQuestionsSets
                .Include( x => x.Questions )
                .FirstOrDefault( x => x.Id == questionsSetId );
        }

        public SurveyQuestionsSet Delete( Guid questionsSetId ) {
            return _database.SurveyQuestionsSets.Remove( Get( questionsSetId ) ).Entity;
        }

        public List<SurveyQuestionsSet> GetsAll( Guid projectId ) {
            return _database.SurveyQuestionsSets
                .Include( x => x.Questions.OrderBy( o => o.Order ) )
                .Where( x => x.ProjectId == projectId )
                .ToList();
        }

        public int GetGreatestQuestionOrder( Guid questionsSetId ) {
            var questionsSet = Get( questionsSetId );

            if ( questionsSet.Questions == null || questionsSet.Questions.Count == 0 ) {
                return 0;
            }

            return questionsSet.Questions.Max( x => x.Order );
        }

        public void SetState( Guid questionsSetId, QuestionsSetsState state ) {
            Get( questionsSetId ).State = state;
        }
    }
}
