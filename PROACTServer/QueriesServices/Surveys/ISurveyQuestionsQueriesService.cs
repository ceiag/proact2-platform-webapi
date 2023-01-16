using Proact.Services.Entities;
using Proact.Services.Models;
using System;

namespace Proact.Services.QueriesServices {
    public interface ISurveyQuestionsQueriesService : IQueriesService {
        public SurveyQuestion Create( 
            SurveyQuestionsSet questionsSet, int order, QuestionCreationRequest creationRequest );
        public SurveyQuestion Get( Guid questionId );
        public SurveyQuestion Delete( Guid questionId );
    }
}
