using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Security.Cryptography;
using System.Threading.Tasks;
using MedicinJournal.Security.Interfaces;

namespace MedicinJournal.Security.Services
{
    public class SymmetricCryptographyService : ISymmetricCryptographyService
    {
        private readonly AesCryptoServiceProvider aes;

        public SymmetricCryptographyService()
        {
            aes = new AesCryptoServiceProvider
            {
                KeySize = 256,
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7
            };
        }

        public byte[] GenerateKey()
        {
            aes.GenerateKey();
            return aes.Key;
        }

        public byte[] GenerateIV()
        {
            aes.GenerateIV();
            return aes.IV;
        }

        public byte[] EncryptText(byte[] key, byte[] iv, string plainText)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            if (iv == null)
                throw new ArgumentNullException(nameof(iv));

            if (string.IsNullOrEmpty(plainText))
                throw new ArgumentException("The plainText parameter cannot be null or empty.", nameof(plainText));

            aes.Key = key;
            aes.IV = iv;

            using (var encryptor = aes.CreateEncryptor())
            {
                byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
                byte[] cipherBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

                byte[] result = new byte[iv.Length + cipherBytes.Length];
                Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
                Buffer.BlockCopy(cipherBytes, 0, result, iv.Length, cipherBytes.Length);

                return result;
            }
        }

        public string DecryptText(byte[] key, byte[] encryptedData)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            if (encryptedData == null)
                throw new ArgumentNullException(nameof(encryptedData));

            if (encryptedData.Length < aes.BlockSize / 8)
                throw new ArgumentException("Invalid encrypted data length.", nameof(encryptedData));

            aes.Key = key;

            byte[] iv = new byte[aes.BlockSize / 8];
            Buffer.BlockCopy(encryptedData, 0, iv, 0, iv.Length);
            aes.IV = iv;

            using (var decryptor = aes.CreateDecryptor())
            {
                byte[] cipherBytes = new byte[encryptedData.Length - iv.Length];
                Buffer.BlockCopy(encryptedData, iv.Length, cipherBytes, 0, cipherBytes.Length);
                byte[] plainBytes = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
                return Encoding.UTF8.GetString(plainBytes);
            }
        }
    }
}
