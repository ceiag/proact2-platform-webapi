using System;
using System.ComponentModel.DataAnnotations;

namespace Proact.Services.Models {
    public class UserIdModel {
        [Required]
        public Guid UserId { get; set; }

        public string AccountId { get; set; }
    }
}
