using System;

namespace Proact.Services.Models {
    public class MultipleChoiceCreationRequest : QuestionCreationRequest {
        public Guid AnswersBlockId { get; set; }
    }
}
