using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;

namespace Proact.Services {
    public class RouteDataRequestCultureProvider : RequestCultureProvider {
        public int IndexOfCulture;
        public int IndexofUICulture;

        public override Task<ProviderCultureResult> DetermineProviderCultureResult( HttpContext httpContext ) {
            if ( httpContext == null ) {
                throw new ArgumentNullException( nameof( httpContext ) );
            }
      
            string culture = null;
            string uiCulture = null;

            try {
                culture = uiCulture = httpContext.Request.Path.Value.Split( '/' )[IndexOfCulture]?.ToString();

                var providerResultCulture = new ProviderCultureResult( culture, uiCulture );

                return Task.FromResult( providerResultCulture );
            }
            catch ( Exception e ) {
                var providerResultCulture = new ProviderCultureResult( "", "" );

                return Task.FromResult( providerResultCulture );
            }
        }
    }
}
