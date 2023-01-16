using Microsoft.Extensions.Configuration;
using System;

namespace Proact.Services {
    public static class AzureMediaServicesConfiguration {
        private static IConfiguration _config;

        public static void Init( IConfiguration config ) {
            _config = config;
        }

        public static string SubscriptionId {
            get { return _config["AzureBlobStorage:SubscriptionId"]; }
        }

        public static string ResourceGroup {
            get { return _config["AzureBlobStorage:ResourceGroup"]; }
        }

        public static string ConnectionString {
            get { return _config["AzureBlobStorage:ConnectionString"]; }
        }

        public static string MediaStorageUrl {
            get { return _config["AzureBlobStorage:MediaStorageUrl"]; }
        }

        public static string AadTenantId {
            get { return _config["AzureMediaServices:AZURE_TENANT_ID"]; }
        }

        public static string AadClientId {
            get { return _config["AzureMediaServices:AZURE_CLIENT_ID"]; }
        }

        public static string AadSecret {
            get { return _config["AzureMediaServices:AZURE_CLIENT_SECRET"]; }
        }

        public static Uri ArmAadAudience {
            get { return new Uri( _config["AzureMediaServices:AZURE_ARM_TOKEN_AUDIENCE"] ); }
        }

        public static string AccountName {
            get { return _config["AzureMediaServices:AZURE_MEDIA_SERVICES_ACCOUNT_NAME"]; }
        }

        public static Uri AadEndpoint {
            get { return new Uri( _config["AadEndpoint"] ); }
        }

        public static Uri ArmEndpoint {
            get { return new Uri( _config["AzureMediaServices:AZURE_ARM_ENDPOINT"] ); }
        }

        public static string Location {
            get { return _config["Location"]; }
        }

        public static string SymmetricKey {
            get { return _config["SymmetricKey"]; }
        }
    }
}
