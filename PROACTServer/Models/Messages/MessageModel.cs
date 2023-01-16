using System;
using System.ComponentModel.DataAnnotations;

namespace Proact.Services.Models {
    public class MessageModel {
        [Required]
        public Guid MessageId { get; set; }
        public Guid? OriginalMessageId { get; set; }
        public bool IsHandled { get; set; }
        public Guid? AuthorId { get; set; }
        public Guid? MedicalTeamId { get; set; }
        public bool IsRead { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public bool Modified { get; set; }
        public PatientMood Emotion { get; set; } = PatientMood.None;
        public MessageScope MessageScope { get; set; } = MessageScope.None;
        public MessageState State { get; set; } = MessageState.Active;
        public DateTime? RecordedTime { get; set; }
        public DateTime? CreatedDateTime { get; set; }
        public MessageType MessageType { get; set; }
        public string AuthorName { get; set; }
        public MessageAttachmentModel Attachment { get; set; }
        public string AvatarUrl { get; set; }
        public int AnalysisCount { get; set; }
        public bool HasAnalysis { get { return AnalysisCount > 0; } }

        public bool IsOriginalMessage {
            get { 
                return OriginalMessageId == null || OriginalMessageId == Guid.Empty;  
            }
        }

        public bool IsAReplyToAMessage {
            get {
                return !IsOriginalMessage;
            }
        }

        public Guid GetOriginalMessageId() {
            if ( IsOriginalMessage ) {
                return MessageId;
            }
            else {
                return (Guid)OriginalMessageId;
            }
        }
    }
}
