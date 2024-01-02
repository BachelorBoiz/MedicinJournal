using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedicinJournal.Security.Interfaces;
using MedicinJournal.Security.Services;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Xunit;


namespace MedicinJournal.Test.Security
{
    [Collection("SymmetricCryptographyServiceTest")]
    public class SymmetricCryptographyServiceTest
    {
        

        [Fact]
        public void Test_SymmetricCryptography_generateKey_notNull()
        {
            ISymmetricCryptographyService _service = new SymmetricCryptographyService();

            byte[] key = _service.GenerateKey();

            Assert.NotNull(key);
        }

        [Fact]
        public void Test_SymmetricCryptography_generateKey_unique()
        {
            ISymmetricCryptographyService _service = new SymmetricCryptographyService();
            int numberOfKeyToGenerate = 120;

            List<byte[]> generatedKeys = new List<byte[]>();
            for (int i = 0; i < numberOfKeyToGenerate; i++)
            {
                byte[] key = _service.GenerateKey();
                generatedKeys.Add(key);
            }

            Assert.Equal(numberOfKeyToGenerate, generatedKeys.Distinct().Count());
        }

        [Fact]
        public void Test_SymmetricCryptography_generateKey_rightKeySize()
        {
            ISymmetricCryptographyService _service = new SymmetricCryptographyService();

            byte[] key = _service.GenerateKey();
            Console.WriteLine($"The key size is {key.Length/8}");

            Assert.Equal(256 / 8, key.Length);
        }

        [Fact]
        public void Test_SymmetricCryptography_GenerateIV_notNull()
        {
            ISymmetricCryptographyService _service = new SymmetricCryptographyService();

            byte[] key = _service.GenerateIV();

            Assert.NotNull(key);
        }

        [Fact]
        public void Test_SymmetricCryptography_GenerateIV_length()
        {
            ISymmetricCryptographyService _service = new SymmetricCryptographyService();

            byte[] key = _service.GenerateIV();

            Assert.Equal(16, key.Length);
        }

        [Fact]
        public void Test_SymmetricCryptography_GenerateIV_unique()
        {
            ISymmetricCryptographyService _service = new SymmetricCryptographyService();
            int numberOfIVsToGenerate = 20;

            List<byte[]> generatedIVs = new List<byte[]>();
            for(int i = 0; i < numberOfIVsToGenerate; i++)
            {
                byte[] iv = _service.GenerateIV();
                generatedIVs.Add(iv);
            }

            Assert.Equal(numberOfIVsToGenerate, generatedIVs.Distinct().Count());
        }

        [Fact]
        public void Test_SymmetricCryptography_Encrypt_NotNull()
        {
            ISymmetricCryptographyService _service = new SymmetricCryptographyService();
            byte[] key = _service.GenerateKey();
            byte[] iv = _service.GenerateIV();
            string textToEncrypt = "Hello world";

            byte[] encryptedData = _service.EncryptText(key, iv, textToEncrypt);

            Assert.NotNull(encryptedData);
        }

        [Fact]
        public void Test_SymmetricCryptography_Encrypt_rightLength()
        {
            ISymmetricCryptographyService _service = new SymmetricCryptographyService();
            byte[] key = _service.GenerateKey();
            byte[] iv = _service.GenerateIV();
            string textToEncrypt = "Hello world";

            byte[] encryptedData = _service.EncryptText(key, iv, textToEncrypt);
            int blockSize = 16;
            int cipherBytesLength = (textToEncrypt.Length / blockSize + 1) * blockSize;
            int expectedLength = iv.Length + cipherBytesLength;

            Assert.Equal(expectedLength, encryptedData.Length);
        }

        [Fact]
        public void Test_SymmetricCryptography_Encrypt_unique()
        {
            ISymmetricCryptographyService _service = new SymmetricCryptographyService();
            int numberOfEncryptionsToGenerate = 20;
            byte[] key = _service.GenerateKey();
            byte[] iv = _service.GenerateIV();
            string textToEncrypt = "Hello world";
            List<byte[]> encryptedDataList = new List<byte[]>();

            for(int i = 0; i < numberOfEncryptionsToGenerate; i++)
            {
                byte[] encryptedData = _service.EncryptText(key, iv, textToEncrypt);
                encryptedDataList.Add(encryptedData);
            }

            Assert.Equal(numberOfEncryptionsToGenerate, encryptedDataList.Distinct().Count());
        }

        [Fact]
        public void Test_SymmetricCryptography_Decrypt_decryptedRight()
        {
            ISymmetricCryptographyService _service = new SymmetricCryptographyService();
            byte[] key = _service.GenerateKey();
            byte[] iv = _service.GenerateIV();
            string textToEncrypt = "Hello world";

            byte[] encryptedData = _service.EncryptText(key, iv, textToEncrypt);
            string decryptedData = _service.DecryptText(key, encryptedData);

            Assert.Equal(textToEncrypt, decryptedData);
        }

    }
}
