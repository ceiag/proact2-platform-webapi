using System;
using System.Text;

namespace Proact.EncryptionAgentService.InputEncryption {
    public class MixedSymAsymEncryptor {
        private byte[] _decryptedSymKey;
        private byte[] _decryptedSymIV;

        public MixedSymAsymEncryptor( byte[] encryptedKey, byte[] encryptedIV ) {
            _decryptedSymKey = RsaParametersEncryptor.Decrypt( encryptedKey );
            _decryptedSymIV = RsaParametersEncryptor.Decrypt( encryptedIV );
        }

        public byte[] Decrypt( byte[] data ) {
            if ( data == null || data.Length == 0 ) {
                return new byte[0];
            }

            return new SymmetricEncryptor().Decrypt( data, _decryptedSymKey, _decryptedSymIV );
        }

        public string DecryptAsString( string dataBase64 ) {
            var data = Convert.FromBase64String( dataBase64 );

            var decryptedData = Decrypt( data );
            return Encoding.UTF8.GetString( decryptedData );
        }
    }
}
