using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using MedicinJournal.Security.Interfaces;
using MedicinJournal.Security.Services;
using System.Text.Json;
using System.Xml.Serialization;

namespace MedicinJournal.Test.Security
{
    [Collection("AsymmetricCryptographyServiceTest")]
    public class AsymmetricCryptographyServiceTest
    {
        private readonly IAsymmetricCryptographyService _service;

        public AsymmetricCryptographyServiceTest()
        {
            _service = new AsymmetricCryptographyService();
        }
        

        #region Key Generation Tests

        public void Test_GenerateKeyPair_NotNull(int keySize)
        {
            var keyPair = _service.GenerateKeyPair(keySize);
            Assert.NotNull(keyPair.publicKey);
            Assert.NotNull(keyPair.privateKey);
        }

        [Theory]
        [InlineData(1024)] // Below recommended security level
        [InlineData(2048)] // Minimum recommended security level
        [InlineData(3072)] // Default security level
        public void Test_GenerateKeyPair_under2048ThrowsException(int keySize)
        {
            if (keySize < 2048)
            {
                Assert.Throws<ArgumentException>(() => _service.GenerateKeyPair(keySize));
            }
        }

        [Theory]
        [InlineData(2048)]
        [InlineData(3072)]
        [InlineData(4096)]
        public void Test_GenerateKeyPair_CorrectByteSize(int keySize)
        {
            var keyPair = _service.GenerateKeyPair(keySize);

            int expectedByteSize = keySize / 8;

            RSAParameters deserializedPublicKey = DeserializeRSAParameters(keyPair.publicKey);
            RSAParameters deserializedPrivateKey = DeserializeRSAParameters(keyPair.privateKey);

            Assert.Equal(expectedByteSize, Convert.FromBase64String(Convert.ToBase64String(deserializedPublicKey.Modulus)).Length);
            Assert.Equal(expectedByteSize, Convert.FromBase64String(Convert.ToBase64String(deserializedPrivateKey.Modulus)).Length);
        }
        #endregion

        #region Encryption Tests

        [Fact]
        public void Test_EncryptWithPublicKey_NotNull()
        {
            var keyPair = _service.GenerateKeyPair();

            var plainText = "Hello, world!";

            var encryptedText = _service.EncryptWithPublicKey(plainText, keyPair.publicKey);
            
            Assert.NotNull(encryptedText);
            Assert.NotEmpty(encryptedText);
        }

        [Theory]
        [MemberData(nameof(KeySizeAndPaddingData))]
        public void Test_EncryptWithPublicKey_CorrectSize(int keySize, RSAEncryptionPadding padding)
        {
            var keyPair = _service.GenerateKeyPair(keySize);
            var plainText = "Hello, world!";

            var encryptedText = _service.EncryptWithPublicKey(plainText, keyPair.publicKey, padding);

            int expectedSize = keySize / 8;
            Assert.Equal(expectedSize, Convert.FromBase64String(encryptedText).Length);
        }

        [Theory]
        [InlineData(null, "PublicKey", typeof(ArgumentException))]
        [InlineData("PlainText", null, typeof(ArgumentException))]
        [InlineData("", "PublicKey", typeof(ArgumentException))]
        [InlineData("PlainText", "", typeof(ArgumentException))]
        public void Test_EncryptWithPublicKey_InvalidArguments_ThrowsException(string plainText, string publicKey, Type exceptionType)
        {
            var keyPair = _service.GenerateKeyPair();

            Assert.Throws(exceptionType, () => _service.EncryptWithPublicKey(plainText, publicKey));
        }
        #endregion

        #region Dencryption Tests

        [Fact]
        public void Test_DecryptWithPrivateKey_NotNull()
        {
            var keyPair = _service.GenerateKeyPair();
            var plainText = "Hello, world!";
            var encryptedText = _service.EncryptWithPublicKey(plainText, keyPair.publicKey);

            var decryptedText = _service.DecryptWithPrivateKey(encryptedText, keyPair.privateKey);

            Assert.NotNull(decryptedText);
            Assert.Equal(plainText, decryptedText);
        }
        [Theory]
        [InlineData(null, "PrivateKey", typeof(ArgumentException))]
        [InlineData("", "PrivateKey", typeof(ArgumentException))]
        [InlineData("EncryptedText", null, typeof(ArgumentException))]
        [InlineData("EncryptedText", "", typeof(ArgumentException))]
        [InlineData("EncryptedText", "InvalidPrivateKeyFormat", typeof(ArgumentException))]
        public void Test_DecryptWithPrivateKey_InvalidArguments_ThrowsException(string encryptedText, string privateKey, Type exceptionType)
        {
            Assert.Throws(exceptionType, () => _service.DecryptWithPrivateKey(encryptedText, privateKey));
        }

        [Fact]
        public void Test_DecryptWithPrivateKey_InvalidEncryptedText_ThrowsException()
        {
            var keyPair = _service.GenerateKeyPair();
            var invalidEncryptedText = "invalidEncryptedText";

            Assert.Throws<System.Security.Cryptography.CryptographicException>(() =>
                _service.DecryptWithPrivateKey(invalidEncryptedText, keyPair.privateKey));
        }

        [Theory]
        [MemberData(nameof(KeySizeAndPaddingData))]
        public void DecryptWithPrivateKey_VaryingKeySizesAndPaddings(int keySize, RSAEncryptionPadding padding)
        {
            var keyPair = _service.GenerateKeyPair(keySize);
            var plainText = "Hello, World!";
            var encryptedText = _service.EncryptWithPublicKey(plainText, keyPair.publicKey, padding);

            var decryptedText = _service.DecryptWithPrivateKey(encryptedText, keyPair.privateKey, padding);

            Assert.NotNull(decryptedText);
            Assert.Equal(plainText, decryptedText);
        }

        #endregion

        #region Signature Generation Tests
        [Fact]
        public void Test_GenerateSignature_NotNull()
        {
            var keyPair = _service.GenerateKeyPair();
            var data = "Hello, world!";

            var signature = _service.GenerateSignature(data, DeserializeRSAParameters(keyPair.privateKey));

            Assert.NotNull(signature);
            Assert.NotEmpty(signature);
        }

        [Fact]
        public void Test_GenerateSignature_NullData_ThrowsException()
        {
            var keyPair = _service.GenerateKeyPair();
            Assert.Throws<ArgumentNullException>(() => _service.GenerateSignature(null, DeserializeRSAParameters(keyPair.privateKey)));
        }

        [Fact]
        public void Test_GenerateSignature_NullPrivateKeyParameters_ThrowsException()
        {
            var data = "Hello, world!";
            Assert.Throws<ArgumentException>(() => _service.GenerateSignature(data, new RSAParameters()));
        }

        [Fact]
        public void Test_GenerateSignature_InvalidPrivateKeyParameters_ThrowsException()
        {
            var data = "Hello, world!";
            var invalidPrivateKeyParameters = new RSAParameters { D = null }; 
            Assert.Throws<ArgumentException>(() => _service.GenerateSignature(data, invalidPrivateKeyParameters));
        }

        #endregion

        #region Signature Verification Tests
        public void Test_VerifySignature_ValidSignature_ReturnsTrue()
        {
            var keyPair = _service.GenerateKeyPair();
            var data = "Hello, world!";
            var signature = _service.GenerateSignature(data, DeserializeRSAParameters(keyPair.privateKey));

            var isValid = _service.VerifySignature(data, Convert.ToBase64String(signature), keyPair.publicKey);

            Assert.True(isValid);
        }

        [Theory]
        [InlineData(null, "ValidSignature", "PublicKey", typeof(ArgumentException))]
        [InlineData("Hello, world!", null, "PublicKey", typeof(ArgumentException))]
        [InlineData("", "ValidSignature", "PublicKey", typeof(ArgumentException))]
        [InlineData("Hello, world!", "", "PublicKey", typeof(ArgumentException))]
        [InlineData("Hello, world!", "ValidSignature", "", typeof(ArgumentException))]
        [InlineData("Hello, world!", "ValidSignature", "InvalidPublicKeyFormat", typeof(ArgumentException))]
        public void Test_VerifySignature_InvalidArguments_ThrowsException(string data, string signature, string publicKey, Type exceptionType)
        {
            var keyPair = _service.GenerateKeyPair();
            Assert.Throws(exceptionType, () => _service.VerifySignature(data, signature, publicKey ?? keyPair.publicKey));
        }
        [Fact]
        public void Test_VerifySignature_NullPublicKey_ThrowsException()
        {
            var data = "Hello, world!";
            var signature = "ValidSignature";
            Assert.Throws<ArgumentException>(() => _service.VerifySignature(data, signature, null));
        }
        [Fact]
        public void Test_EncryptAndDecrypt_DataRemainsSame()
        {
            var data = "Hello, world!";
            var keyPair = _service.GenerateKeyPair();

            var encryptedData = _service.EncryptWithPublicKey(data, keyPair.publicKey);
            var decryptedData = _service.DecryptWithPrivateKey(encryptedData, keyPair.privateKey);

            Assert.Equal(data, decryptedData);
        }

        #endregion

        #region Helper Methods
        private RSAParameters DeserializeRSAParameters(string serializedParameters)
        {
            var serializer = new XmlSerializer(typeof(RSAParameters));
            using (var reader = new StringReader(serializedParameters))
            {
                return (RSAParameters)serializer.Deserialize(reader);
            }
        }

        #endregion

        #region IEnumerables

        public static IEnumerable<object[]> KeySizeAndPaddingData()
        {
            yield return new object[] { 2048, RSAEncryptionPadding.OaepSHA256 };
            yield return new object[] { 2048, RSAEncryptionPadding.Pkcs1 };
            yield return new object[] { 3072, RSAEncryptionPadding.OaepSHA256 };
            yield return new object[] { 3072, RSAEncryptionPadding.Pkcs1 };
            yield return new object[] { 4096, RSAEncryptionPadding.OaepSHA256 };
            yield return new object[] { 4096, RSAEncryptionPadding.Pkcs1 };
            yield return new object[] { 4096, RSAEncryptionPadding.OaepSHA512 };
        }

        #endregion
    }
}