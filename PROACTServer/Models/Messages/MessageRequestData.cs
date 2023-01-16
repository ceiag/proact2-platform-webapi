using System.ComponentModel.DataAnnotations;

namespace Proact.Services.Models.Messages {
    public class MessageRequestData {
        [MaxLength( 250 )]
        public string Title { get; set; }
        [MaxLength( 2000 )]
        public string Body { get; set; }
        public PatientMood? Emotion { get; set; }
        public MessageScope MessageScope { get; set; }
        public bool HasAttachment { get; set; }
    }
}
