using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Proact.Services.Models {
    public class DocumentCreationRequest {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public IFormFile pdfFile { get; set; }
    }
}
