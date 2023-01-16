using Microsoft.AspNetCore.Mvc;
using Proact.Configurations;
using Proact.EncryptionAgentService.Models;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using System.Security.Cryptography;

namespace Proact.EncryptionAgentService.Controllers {
    public class SecurityController : Controller {
        /// <summary>
        /// Return RSA Public Key
        /// </summary>
        [HttpGet]
        [Route( "api/PublicKeyForMsgEncryption" )]
        [SwaggerResponse( (int)HttpStatusCode.OK, Type = typeof( PublicKeyModel ) )]
        public IActionResult GetPublicKeyForMessageEncryption() {
            var rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString( ProactConfiguration.MessageRsaKeyValuesPublic );

            var exportedParameters = rsa.ExportParameters( false );
            var publicKeyModel = new PublicKeyModel(
                exportedParameters.Modulus, exportedParameters.Exponent );

            return Ok( publicKeyModel );
        }
    }
}
