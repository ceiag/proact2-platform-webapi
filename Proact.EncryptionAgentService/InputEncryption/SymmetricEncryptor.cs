using System.IO;
using System.Security.Cryptography;

namespace Proact.EncryptionAgentService.InputEncryption {
    public class SymmetricEncryptor {
        public const int AesKeySize = 256;

        public byte[] Encrypt( byte[] input, byte[] key, byte[] iv ) {
            using ( var aes = new AesManaged() ) {
                aes.KeySize = AesKeySize;

                aes.Key = key;
                aes.IV = iv;

                return Encrypt( input, aes );
            }
        }

        public byte[] Decrypt( byte[] input, byte[] key, byte[] iv ) {
            using ( var aes = new AesManaged() ) {
                aes.Key = key;
                aes.IV = iv;

                return Dencrypt( input, aes );
            }
        }

        private byte[] Encrypt( byte[] input, SymmetricAlgorithm algorithm ) {
            var encryptor = algorithm.CreateEncryptor();

            using ( var memoryStream = new MemoryStream() ) {
                using ( var cryptoStream = new CryptoStream( 
                    memoryStream, encryptor, CryptoStreamMode.Write ) ) {
                    
                    cryptoStream.Write( input, 0, input.Length );
                }

                return memoryStream.ToArray();
            }
        }

        private byte[] Dencrypt( byte[] encryptedInput, SymmetricAlgorithm algorithm ) {
            var decryptor = algorithm.CreateDecryptor();

            using ( var inputMemoryStream = new MemoryStream( encryptedInput ) ) {
                using ( var cryptoStream = new CryptoStream( 
                    inputMemoryStream, decryptor, CryptoStreamMode.Read ) ) {
                    
                    using ( var outputMemoryStream = new MemoryStream() ) {
                        cryptoStream.CopyTo( outputMemoryStream );

                        return outputMemoryStream.ToArray();
                    }
                }
            }
        }
    }
}
