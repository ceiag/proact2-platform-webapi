using System;
using System.ComponentModel.DataAnnotations;

namespace Proact.Services.Models {
    public class UserGenerality {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string Lastname { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
