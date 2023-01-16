using System.ComponentModel.DataAnnotations;

namespace Proact.Services.Models {
    public class GeneralityModel {
        public string Phone { get; set; }
        [Required]
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        [Required]
        public string City { get; set; }
        public string StateOrProvince { get; set; }
        public string RegionCode { get; set; }
        [Required]
        public string PostalCode { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public string TimeZone { get; set; }
    }
}
