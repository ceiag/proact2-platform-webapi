using Proact.Services.Entities.MessageAnalysis;
using System;

namespace Proact.Services.Entities {
    public class ProjectProperties : TrackableEntity, IEntity {
        public bool MedicsCanSeeOtherAnalisys { get; set; } = true;
        public int MessageCanNotBeDeletedAfterMinutes { get; set; }
        public int MessageCanBeRepliedAfterMinutes { get; set; }
        public int MessageCanBeAnalizedAfterMinutes { get; set; }
        public bool IsAnalystConsoleActive { get; set; } = true;
        public bool IsSurveysSystemActive { get; set; } = true;
        public bool IsMessagingActive { get; set; } = true;
        public virtual Guid? LexiconId { get; set; }
        public virtual Lexicon Lexicon { get; set; }
        public virtual Guid ProjectId { get; set; }
        public virtual Project Project { get; set; }
    }
}
