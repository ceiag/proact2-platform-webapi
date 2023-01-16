using Proact.Services.Entities;
using System;

namespace Proact.Services.Models {
    public class SurveyAssignationModel {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid SurveyId { get; set; }
        public string SurveyTitle { get; set; }
        public string SurveyDescription { get; set; }
        public string SurveyVersion { get; set; }
        public bool Completed { get; set; }
        public DateTime? CompletedDateTime { get; set; } = DateTime.MinValue;
        public SurveyState SurveyState { get; set; } = SurveyState.DRAW;
        public DateTime StartTime { get; set; }
        public DateTime ExpireTime { get; set; }
        public SurveyReccurence Reccurence { get; set; }
        public UserModel User { get; set; }
        public SurveySchedulerModel Scheduler { get; set; }

        public bool Expired { 
            get {
                return DateTime.UtcNow.Date > ExpireTime.Date;
            } 
        }
    }
}
