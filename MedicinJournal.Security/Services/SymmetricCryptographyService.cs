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
        public byte[] GenerateKey()
        {
            using (var aes = new AesCryptoServiceProvider())
            {
                aes.KeySize = 256;
                aes.GenerateKey();
                byte[] key = aes.Key;
                return key;
            }
        }

        public byte[] GenerateIV()
        {
            using (var aes = new AesCryptoServiceProvider())
            {
                aes.GenerateIV(); 
                return aes.IV;
            }
        }

        public byte[] EncryptText(byte[] key, byte[] iv, string plainText)
        {
            using (var aes = new AesCryptoServiceProvider())
            {
                aes.Key = key;
                aes.IV = iv;

                using (var encryptor = aes.CreateEncryptor())
                {
                    byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
                    byte[] cipherBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

                    byte[] result = new byte[iv.Length + cipherBytes.Length];
                    Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
                    Buffer.BlockCopy(cipherBytes, 0,result, iv.Length, cipherBytes.Length);

                    return result;
                }
            }
        }

        public string DecryptText(byte[] key, byte[] encryptedData)
        {
            using (var aes = new AesCryptoServiceProvider())
            {
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
}
