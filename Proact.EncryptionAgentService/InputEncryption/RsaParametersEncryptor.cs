using Proact.Configurations;
using System.Security.Cryptography;

namespace Proact.EncryptionAgentService.InputEncryption {
    public class RsaParametersEncryptor {
        public static byte[] Encrypt( byte[] input ) {
            using ( var rsa = new RSACryptoServiceProvider() ) {
                rsa.FromXmlString( ProactConfiguration.MessageRsaKeyValuesPrivate );

                return rsa.Encrypt( input, fOAEP: false );
            }
        }

        public static byte[] Decrypt( byte[] input ) {
            using ( var rsa = new RSACryptoServiceProvider() ) {
                rsa.FromXmlString( ProactConfiguration.MessageRsaKeyValuesPrivate );

                return rsa.Decrypt( input, fOAEP: false );
            }
        }
    }
}
