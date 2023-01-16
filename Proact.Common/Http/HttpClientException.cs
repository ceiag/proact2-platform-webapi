using System;
using System.Net;

namespace Proact.Common.Http {
    public class HttpClientException : ApplicationException {
        public readonly HttpStatusCode StatusCode;

        public HttpClientException( HttpStatusCode statusCode, string message ) : base( message ) {
            StatusCode = statusCode;
        }
    }
}
