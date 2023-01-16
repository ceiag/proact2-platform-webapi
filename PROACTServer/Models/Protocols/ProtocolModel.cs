using System;

namespace Proact.Services.Models {
    public class ProtocolModel {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string InternalCode { get; set; }
        public string InternationalCode { get; set; }
        public string Url { get; set; }
    }
}
