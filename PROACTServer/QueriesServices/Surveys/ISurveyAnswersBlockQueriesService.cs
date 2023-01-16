using Proact.Services.Entities;
using Proact.Services.Models;
using System;
using System.Collections.Generic;

namespace Proact.Services.QueriesServices {
    public interface ISurveyAnswersBlockQueriesService : IQueriesService {
        public SurveyAnswersBlock Create( Guid projectId, AnswersBlockCreationRequest request );
        public List<SurveyAnswersBlock> GetsAll( Guid projectId );
        public SurveyAnswersBlock Get( Guid answerBlockId );
    }
}
