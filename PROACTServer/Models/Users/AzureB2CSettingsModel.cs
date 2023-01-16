using System;
namespace Proact.Services.Models {
    public class AzureB2CSettingsModel {
        public string Tenant { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string B2CExtentionAppClientId { get; set; }
        public string UserCommonPassword { get; set; }
    }
}
