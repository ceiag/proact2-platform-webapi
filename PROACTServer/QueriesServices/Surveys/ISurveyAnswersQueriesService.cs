using Proact.Services.Entities;
using System;
using System.Collections.Generic;

namespace Proact.Services.QueriesServices {
    public interface ISurveyAnswersQueriesService : IQueriesService {
        public SurveyAnswer Create( SurveyAnswer answer );
        public SurveyAnswer Get( Guid answerId );
        public List<SurveyAnswer> GetsAll( List<Guid> answerIds );
        public void AddAnswerToQuestion( Guid questionId, Guid answerId );
    }
}
