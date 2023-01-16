namespace Proact.EncryptionAgentService.Models {
    public class PublicKeyModel {
        public byte[] PublicKey { get; set; }
        public byte[] Exponent { get; set; }

        public PublicKeyModel( byte[] publicKey, byte[] exponent ) {
            this.PublicKey = publicKey;
            this.Exponent = exponent;
        }
    }
}
