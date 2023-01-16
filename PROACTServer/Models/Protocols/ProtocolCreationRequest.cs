using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Proact.Services.Models {
    public class ProtocolCreationRequest {
        [Required]
        public string Name { get; set; }
        [Required]
        public string InternalCode { get; set; }
        [Required]
        public string InternationalCode { get; set; }
        [Required]
        public IFormFile pdfFile { get; set; }
    }
}
