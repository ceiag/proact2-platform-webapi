using System;

namespace Proact.Services.Entities {
    public class SurveyScheduler : TrackableEntity, IEntity {
        public DateTime StartTime { get; set; }
        public DateTime ExpireTime { get; set; }
        public DateTime LastSubmission { get; set; }
        public SurveyReccurence Reccurence { get; set; }
        public virtual Guid UserId { get; set; }
        public virtual User User { get; set; }
        public virtual Guid SurveyId { get; set; }
        public virtual Survey Survey { get; set; }
    }
}
