using Proact.Services.Entities;
using System;
using System.Collections.Generic;

namespace Proact.Services.QueriesServices {
    public interface ISurveyAnswerToQuestionQueriesService : IQueriesService {
        public SurveyUsersQuestionsAnswersRelation SetUserAnswerQuestion(
            SurveysAssignationRelation surveyAssignment, Guid questionId, SurveyUserQuestionAnswer answer );
        public SurveyUsersQuestionsAnswersRelation SetUserAswersQuestion(
            SurveysAssignationRelation surveyAssignment, Guid questionId, 
            List<SurveyUserQuestionAnswer> answers );
    }
}
