using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicinJournal.Security.Interfaces
{
    public interface ISymmetricKeyService
    {
        public byte[] GenerateKey();

        public byte[] GenerateIV(); 

        public byte[] EncryptText(byte[] key, byte[] iv, string plainText);

        public string DecryptText(byte[] key, byte[] encryptedData);
    }
}
