using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MedicinJournal.Security.Interfaces
{
    public interface IAsymmetricCryptographyService
    {
        public (string publicKey, string privateKey) GenerateKeyPair(int keySize = 3072);

        public string EncryptWithPublicKey(string plainText, string publicKey, RSAEncryptionPadding padding = null);

        public string DecryptWithPrivateKey(string encryptedText, string privateKey, RSAEncryptionPadding padding = null);

        public byte[] GenerateSignature(string data, RSAParameters privateKeyParameters);

        public bool VerifySignature(string data, byte[] signature, string publicKey);

        public RSAParameters DeserializeRSAParameters(string serializedParameters);
    }
}
