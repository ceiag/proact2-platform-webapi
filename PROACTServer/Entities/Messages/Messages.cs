using Proact.Services.Entities.MessageAnalysis;
using Proact.Services.Models;
using System;
using System.Collections.Generic;

namespace Proact.Services.Entities {
    public class Message : TrackableEntity, IEntity {
        public Guid? OriginalMessageId { get; set; }
        public bool IsHandled { get; set; }
        public virtual MessageData MessageData { get; set; }
        public Guid? AuthorId { get; set; }
        public virtual User Author { get; set; }
        public Guid? MedicalTeamId { get; set; }
        public virtual MedicalTeam MedicalTeam { get; set; }
        public PatientMood Emotion { get; set; } = PatientMood.None;
        public MessageScope MessageScope { get; set; } = MessageScope.None;
        public DateTime? RecordedTime { get; set; }
        public MessageType MessageType { get; set; }
        public bool Show { get; set; }
        public MessageState State { get; set; }
        public virtual MessageAttachment MessageAttachment { get; set; }
        public virtual List<MessageRecipient> Recipients { get; set; }
        public virtual List<MessageReplies> Replies { get; set; }
        public virtual List<Analysis> Analysis { get; set; }

        public bool IsStartingMessage {
            get {
                return OriginalMessageId == null || OriginalMessageId == Guid.Empty;
            }
        }

        public Message() {
            Recipients = new List<MessageRecipient>();
            Analysis = new List<Analysis>();
        }
    }
}
