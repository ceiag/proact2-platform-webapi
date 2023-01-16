using Proact.Services.Entities;
using System;
using System.Collections.Generic;

namespace Proact.Services.QueriesServices {
    public class SurveyAnswerToQuestionQueriesService : ISurveyAnswerToQuestionQueriesService {
        private readonly ProactDatabaseContext _database;

        public SurveyAnswerToQuestionQueriesService( ProactDatabaseContext database ) {
            _database = database;
        }

        public SurveyUsersQuestionsAnswersRelation SetUserAnswerQuestion(
            SurveysAssignationRelation surveyAssignment, Guid questionId, SurveyUserQuestionAnswer answer ) {
            var surveyUsersQuestionsAnswersRelation = new SurveyUsersQuestionsAnswersRelation() {
                Id = Guid.NewGuid(),
                AssignmentId = surveyAssignment.Id,
                QuestionId = questionId
            };

            surveyUsersQuestionsAnswersRelation.Answers = new List<SurveyUserQuestionAnswer>();
            surveyUsersQuestionsAnswersRelation.Answers.Add( answer );

            return _database.SurveyUsersQuestionsAnswersRelations
                .Add( surveyUsersQuestionsAnswersRelation ).Entity;
        }

        public SurveyUsersQuestionsAnswersRelation SetUserAswersQuestion(
            SurveysAssignationRelation surveyAssignment, Guid questionId,
            List<SurveyUserQuestionAnswer> answers ) {
            var surveyUsersQuestionsAnswersRelation = new SurveyUsersQuestionsAnswersRelation() {
                Id = Guid.NewGuid(),
                AssignmentId = surveyAssignment.Id,
                QuestionId = questionId
            };

            surveyUsersQuestionsAnswersRelation.Answers = new List<SurveyUserQuestionAnswer>();

            foreach ( var answer in answers ) {
                surveyUsersQuestionsAnswersRelation.Answers.Add( answer );
            }

            return _database.SurveyUsersQuestionsAnswersRelations
                .Add( surveyUsersQuestionsAnswersRelation ).Entity;
        }
    }
}
