using Proact.Services.Entities;
using System;

namespace Proact.Services.Models {
    public class SurveySchedulerModel {
        public Guid Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime ExpireTime { get; set; }
        public DateTime LastSubmission { get; set; }
        public Guid UserId { get; set; }
        public Guid SurveyId { get; set; }
        public SurveyReccurence Reccurence { get; set; }
    }
}
