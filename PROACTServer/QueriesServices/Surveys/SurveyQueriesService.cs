using Microsoft.EntityFrameworkCore;
using Proact.Services.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Proact.Services.QueriesServices {
    public class SurveyQueriesService : ISurveyQueriesService {
        private readonly ProactDatabaseContext _database;

        public SurveyQueriesService( ProactDatabaseContext database ) {
            _database = database;
        }

        public List<SurveysQuestionsRelation> AddQuestions( Guid surveyId, List<Guid> questionIds ) {
            var surveysQuestionRelations = new List<SurveysQuestionsRelation>();

            foreach ( var questionId in questionIds ) {
                var surveyQuestionRelations = new SurveysQuestionsRelation() {
                    QuestionId = questionId,
                    SurveyId = surveyId,
                };

                surveysQuestionRelations.Add( surveyQuestionRelations );
            }

            _database.SurveysQuestionsRelations.AddRange( surveysQuestionRelations );
            return surveysQuestionRelations;
        }

        public SurveyQuestion RemoveQuestion( Guid surveyId, Guid questionId ) {
            var surveyQuestionRelation = _database
                .SurveysQuestionsRelations.FirstOrDefault( x => x.QuestionId == questionId );

            return _database.SurveysQuestionsRelations.Remove( surveyQuestionRelation ).Entity.Question;
        }

        public Survey Create( Guid projectId, SurveyCreationRequest creationRequest ) {
            var survey = new Survey() {
                Title = creationRequest.Title,
                Description = creationRequest.Description,
                Version = creationRequest.Version,
                ProjectId = projectId
            };

            return _database.Surveys.Add( survey ).Entity;
        }

        public Survey Get( Guid surveyId ) {
            return _database.Surveys
                .Include( x => x.Questions )
                .FirstOrDefault( x => x.Id == surveyId );
        }
        
        public void Update( Guid surveyId, SurveyEditRequest request ) {
            var survey = Get( surveyId );
            survey.Title = request.Title;
            survey.Description = request.Description;
            survey.Version = request.Version;

            survey.Questions.Clear();
            AddQuestions( surveyId, request.QuestionsIds );
        }

        public List<Survey> GetsAll( Guid projectId ) {
            return _database.Surveys
                .Include( x => x.AssignationRelations )
                .Where( x => x.ProjectId == projectId )
                .OrderByDescending( x => x.Created )
                .ToList();
        }

        public Survey Delete( Guid surveyId ) {
            return _database.Surveys.Remove( Get( surveyId ) ).Entity;
        }

        public bool IsQuestionAlreadyAdded( Survey survey, Guid questionId ) {
            return survey.Questions.FirstOrDefault( x => x.QuestionId == questionId ) != null;
        }

        public void SetState( Guid surveyId, SurveyState state ) {
            Get( surveyId ).SurveyState = state;
        }

        public void SetAssignationProperties( 
            Guid surveyId, DateTime start, DateTime expire, SurveyReccurence reccurence ) {
            var survey = Get( surveyId );

            survey.Reccurence = reccurence;
            survey.StartTime = start;
            survey.ExpireTime = expire;

            _database.Surveys.Update( survey );
            _database.SaveChanges();
        }
    }
}
