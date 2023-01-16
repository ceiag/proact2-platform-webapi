using System;

namespace Proact.Services.Models {
    public class ProjectPropertiesModel {
        public LexiconModel Lexicon { get; set; }
        public bool MedicsCanSeeOtherAnalisys { get; set; }
        public int MessageCanNotBeDeletedAfterMinutes { get; set; }
        public int MessageCanBeRepliedAfterMinutes { get; set; }
        public int MessageCanBeAnalizedAfterMinutes { get; set; }
        public bool IsAnalystConsoleActive { get; set; }
        public bool IsSurveysSystemActive { get; set; }
        public bool IsMessagingActive { get; set; }
    }
}
