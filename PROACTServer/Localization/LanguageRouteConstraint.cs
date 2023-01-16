using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Proact.Services {
    public class LanguageRouteConstraint : IRouteConstraint {

        private List<string> _languages = new List<string> {
            "en-US",
            "it-IT",
            "de-DE",
            "fr-FR",
            "es-ES",
            "nl-NL"
        };

        private const string CULTURE_KEY = "culture";

        public bool Match( 
            HttpContext httpContext, IRouter route, string routeKey, 
            RouteValueDictionary values, RouteDirection routeDirection ) {

            if ( !values.ContainsKey( CULTURE_KEY ) ) {
                return false;
            }

            var culture = values[CULTURE_KEY].ToString();
            return _languages.Contains( culture );
        }
    }
}
