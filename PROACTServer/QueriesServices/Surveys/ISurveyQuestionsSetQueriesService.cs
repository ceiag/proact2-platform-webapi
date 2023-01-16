using Proact.Services.Entities;
using Proact.Services.Models;
using System;
using System.Collections.Generic;

namespace Proact.Services.QueriesServices {
    public interface ISurveyQuestionsSetQueriesService : IQueriesService {
        public SurveyQuestionsSet Create( Guid projectId, QuestionsSetCreationRequest request );
        public SurveyQuestionsSet Delete( Guid questionsSetId );
        public SurveyQuestionsSet Edit( Guid questionsSetId, QuestionsSetEditRequest request );
        public SurveyQuestionsSet Get( Guid questionsSetId );
        public List<SurveyQuestionsSet> GetsAll( Guid projectId );
        public int GetGreatestQuestionOrder( Guid questionsSetId );
        public void SetState( Guid questionsSetId, QuestionsSetsState state );
    }
}
