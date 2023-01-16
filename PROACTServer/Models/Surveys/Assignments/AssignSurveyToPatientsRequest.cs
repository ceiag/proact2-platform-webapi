using Proact.Services.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Proact.Services.Models {
    public class CreateScheduledSurveyRequest {
        public Guid SurveyId { get; set; }
        [Required]
        public List<Guid> UserIds { get; set; }
        [Required]
        [DataMustBeMajorThanNow( ErrorMessage = "Starting date must be major than now!" )]
        public DateTime StartTime { get; set; }
        [Required]
        [DataMustBeMajorThanNow( ErrorMessage = "This data is already expired!")]
        public DateTime ExpireTime { get; set; }
        [Required]
        public SurveyReccurence Reccurence { get; set; }
    }
}
