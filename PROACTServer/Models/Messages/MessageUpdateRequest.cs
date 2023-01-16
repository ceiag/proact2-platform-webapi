using System.ComponentModel.DataAnnotations;

namespace Proact.Services.Models {
    public class MessageUpdateRequest {
        [Required]
        public bool IsRead { get; set; }

        [Required]
        public bool IsMarked { get; set; }
    }
}
