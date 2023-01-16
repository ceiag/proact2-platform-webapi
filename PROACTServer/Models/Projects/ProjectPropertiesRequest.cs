using System.ComponentModel.DataAnnotations;

namespace Proact.Services.Models {
    public abstract class ProjectPropertiesRequest {
        public bool MedicsCanSeeOtherAnalisys { get; set; }
        [Range( 0, int.MaxValue, ErrorMessage = "Please enter a value bigger than {0}" )]
        public int MessageCanNotBeDeletedAfterMinutes { get; set; }
        [Range( 0, int.MaxValue, ErrorMessage = "Please enter a value bigger than {0}" )]
        public int MessageCanBeAnalizedAfterMinutes { get; set; }
        [Range( 0, int.MaxValue, ErrorMessage = "Please enter a value bigger than {0}" )]
        public int MessageCanBeRepliedAfterMinutes { get; set; }
        public bool IsAnalystConsoleActive { get; set; }
        public bool IsSurveysSystemActive { get; set; }
        public bool IsMessagingActive { get; set; }
    }
}
