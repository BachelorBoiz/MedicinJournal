using MedicinJournal.Security.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MedicinJournal.Security.Services
{
    public class AsymmetricCryptographyService : IAsymmetricCryptographyService
    {

        public (string publicKey, string privateKey) GenerateKeyPair(int keySize = 3072)
        {
            try
            {
                if (keySize < 2048)
                {
                    throw new ArgumentException("Key size should be at least 2048 bits for security reasons.", nameof(keySize));
                }

                using (var rsa = RSA.Create())
                {
                    var secureRandom = new System.Security.Cryptography.RNGCryptoServiceProvider();
                    rsa.KeySize = keySize;

                    RSAParameters publicKeyParams = rsa.ExportParameters(false);
                    RSAParameters privateKeyParams = rsa.ExportParameters(true);

                    string publicKey = SerializeRSAParameters(publicKeyParams);
                    string privateKey = SerializeRSAParameters(privateKeyParams);

                    return (publicKey, privateKey);
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"ArgumentException in GenerateKeyPair: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generating key pair: {ex.Message}");
                throw;
            }
        }

        public string EncryptWithPublicKey(string plainText, string publicKey, RSAEncryptionPadding padding = null)
        {
            try
            {
                // Validate parameters
                if (string.IsNullOrEmpty(plainText))
                {
                    throw new ArgumentException("The plainText parameter cannot be null or empty.", nameof(plainText));
                }

                if (string.IsNullOrEmpty(publicKey))
                {
                    throw new ArgumentException("The publicKey parameter cannot be null or empty.", nameof(publicKey));
                }

                // Default padding scheme
                padding ??= RSAEncryptionPadding.OaepSHA256;

                using (var rsa = RSA.Create())
                {
                    // Validate the public key format
                    if (!IsValidRSAPublicKey(publicKey))
                    {
                        throw new ArgumentException("Invalid RSA public key format.", nameof(publicKey));
                    }

                    rsa.FromXmlString(publicKey);

                    byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
                    byte[] encryptedBytes = rsa.Encrypt(plainBytes, padding);

                    return Convert.ToBase64String(encryptedBytes);
                }
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine($"ArgumentNullException in EncryptWithPublicKey: {ex.ParamName}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error encrypting with public key: {ex.Message}");
                throw;
            }
        }

        public string DecryptWithPrivateKey(string encryptedText, string privateKey, RSAEncryptionPadding padding = null)
        {
            try
            {
                if (string.IsNullOrEmpty(encryptedText))
                {
                    throw new ArgumentException("The encryptedText parameter cannot be null or empty.", nameof(encryptedText));
                }
                if (string.IsNullOrEmpty(privateKey))
                {
                    throw new ArgumentException("The privateKey parameter cannot be null or empty.", nameof(privateKey));
                }

                padding ??= RSAEncryptionPadding.OaepSHA256;

                using (var rsa = RSA.Create())
                {
                    if (!IsValidRSAPrivateKey(privateKey))
                    {
                        throw new ArgumentException("Invalid RSA private key format.", nameof(privateKey));
                    }
                    rsa.FromXmlString(privateKey);

                    byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
                    byte[] decryptedBytes = rsa.Decrypt(encryptedBytes, padding);

                    return Encoding.UTF8.GetString(decryptedBytes);
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"ArgumentException in DecryptWithPrivateKey: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DecryptWithPrivateKey: {ex.Message}");
                throw;
            }
        }

        public byte[] GenerateSignature(string data, RSAParameters privateKeyParameters)
        {
            try
            {
                if (data == null)
                {
                    throw new ArgumentNullException(nameof(data));
                }
                if (privateKeyParameters.D == null)
                {
                    throw new ArgumentException("Private key parameters are invalid.", nameof(privateKeyParameters));
                }

                using (var rsa = RSA.Create())
                {
                    rsa.ImportParameters(privateKeyParameters);

                    byte[] dataBytes = Encoding.UTF8.GetBytes(data);

                    using (var hasher = SHA256.Create())
                    {
                        byte[] hashedData = hasher.ComputeHash(dataBytes);
                        byte[] signatureBytes = rsa.SignData(hashedData, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

                        return signatureBytes;
                    }
                }
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine($"ArgumentNullException in GenerateSignature: {ex.ParamName}");
                throw;
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"ArgumentException in GenerateSignature: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generating signature: {ex.Message}");
                throw;
            }
        }

        public bool VerifySignature(string data, string signature, string publicKey)
        {
            try
            {
                // Validate parameters
                if (string.IsNullOrEmpty(data))
                {
                    throw new ArgumentException("The data parameter cannot be null or empty.", nameof(data));
                }

                if (string.IsNullOrEmpty(signature))
                {
                    throw new ArgumentException("The signature parameter cannot be null or empty.", nameof(signature));
                }

                if (string.IsNullOrEmpty(publicKey))
                {
                    throw new ArgumentException("The publicKey parameter cannot be null or empty.", nameof(publicKey));
                }

                using (var rsa = RSA.Create())
                {
                    if (!IsValidRSAPublicKey(publicKey))
                    {
                        throw new ArgumentException("Invalid RSA public key format.", nameof(publicKey));
                    }

                    rsa.FromXmlString(publicKey);

                    byte[] dataBytes = Encoding.UTF8.GetBytes(data);
                    byte[] signatureBytes = Convert.FromBase64String(signature);

                    using (var hasher = SHA256.Create())
                    {
                        byte[] hashedData = hasher.ComputeHash(dataBytes);

                        bool isSignatureValid = rsa.VerifyData(hashedData, signatureBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

                        return isSignatureValid;
                    }
                }
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine($"ArgumentNullException in VerifySignature: {ex.ParamName}");
                throw;
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"ArgumentException in VerifySignature: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error verifying signature: {ex.Message}");
                throw;
            }
        }

        #region Helper Methods
        private bool IsValidRSAPublicKey(string publicKey)
        {
            try
            {
                var rsa = RSA.Create();
                rsa.FromXmlString(publicKey);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        private bool IsValidRSAPrivateKey(string privateKey)
        {
            try
            {
                var rsa = RSA.Create();
                rsa.FromXmlString(privateKey);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private string SerializeRSAParameters(RSAParameters parameters)
        {
            var serializer = new XmlSerializer(typeof(RSAParameters));
            using (var writer = new StringWriter())
            {
                serializer.Serialize(writer, parameters);
                return writer.ToString();
            }
        }

        private RSAParameters DeserializeRSAParameters(string serializedParameters)
        {
            var serializer = new XmlSerializer(typeof(RSAParameters));
            using (var reader = new StringReader(serializedParameters))
            {
                return (RSAParameters)serializer.Deserialize(reader);
            }
        }
        #endregion
    }
}
