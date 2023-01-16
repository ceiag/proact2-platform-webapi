using Proact.Services.Entities;
using System;
using System.Collections.Generic;

namespace Proact.Services.QueriesServices {
    public interface ISurveyQueriesService : IQueriesService {
        public Survey Create( Guid projectId, SurveyCreationRequest creationRequest );
        public void Update( Guid surveyId, SurveyEditRequest request );
        public Survey Get( Guid surveyId );
        public List<Survey> GetsAll( Guid projectId );
        public Survey Delete( Guid surveyId );
        public List<SurveysQuestionsRelation> AddQuestions( Guid surveyId, List<Guid> questionIds );
        public SurveyQuestion RemoveQuestion( Guid surveyId, Guid questionId );
        public bool IsQuestionAlreadyAdded( Survey survey, Guid questionId );
        public void SetState( Guid surveyId, SurveyState state );
        public void SetAssignationProperties(
            Guid surveyId, DateTime start, DateTime expire, SurveyReccurence reccurence );
    }
}
