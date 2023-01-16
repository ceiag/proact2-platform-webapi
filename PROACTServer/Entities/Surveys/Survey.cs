using System;
using System.Collections.Generic;

namespace Proact.Services.Entities {
    public class Survey : TrackableEntity, IEntity {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Version { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? ExpireTime { get; set; }
        public SurveyReccurence? Reccurence { get; set; }
        public SurveyState SurveyState { get; set; } = SurveyState.DRAW;
        public virtual List<SurveysQuestionsRelation> Questions { get; set; }
        public virtual Guid? ProjectId { get; set; }
        public virtual List<SurveysAssignationRelation> AssignationRelations { get; set; }
    }
}
