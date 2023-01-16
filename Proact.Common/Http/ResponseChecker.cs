using System.Net.Http;
using System.Threading.Tasks;

namespace Proact.Common.Http {
    public class ResponseChecker {
        //private static ILog logger = LogManager.GetLogger(typeof(ResponseChecker));

        public static async Task CheckIfSuccessful( HttpResponseMessage response ) {
            if ( !response.IsSuccessStatusCode ) {
                //logger.ErrorFormat( "HTTP client error, status code {0}", response.StatusCode );
                var responseErrorMessage = await response.Content.ReadAsStringAsync();

                //logger.ErrorFormat( "Request URI: '{0}'\n Raw response: '{1}'", response.RequestMessage.RequestUri, responseErrorMessage );

                throw new HttpClientException( response.StatusCode, responseErrorMessage );
            }
        }
    }
}
