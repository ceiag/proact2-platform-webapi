using System;

namespace Proact.Services.Models {
    public class SingleChoiceCreationRequest : QuestionCreationRequest {
        public Guid AnswersBlockId { get; set; }
    }
}
