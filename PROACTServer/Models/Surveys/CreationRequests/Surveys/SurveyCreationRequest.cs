using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Proact.Services {
    public class SurveyCreationRequest {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Version { get; set; }
        public Guid QuestionsSetId { get; set; }
        public List<Guid> QuestionsIds { get; set; } = new List<Guid>();
    }
}
