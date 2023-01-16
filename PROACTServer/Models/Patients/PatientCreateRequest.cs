using System.ComponentModel.DataAnnotations;

namespace Proact.Services.Models {
    public class PatientCreateRequest {
        public string FirstName { get; set; } = string.Empty;
        public string Lastname { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public int BirthYear { get; set; }
        [RegularExpression( "^(F|M)$", ErrorMessage = "Please provide a valid gender - F or M" )]
        public string Gender { get; set; }
    }
}
