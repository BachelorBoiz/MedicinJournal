using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using MedicinJournal.Security.Interfaces;

namespace MedicinJournal.Security.Services
{
    public class PasswordHasher : IPasswordHasher
    {
        private const int saltSize = 128 / 8;
        private const int KeySize = 256 / 8;
        private const int Iterations = 600000;
        private static readonly HashAlgorithmName _hashAlgorithmName = HashAlgorithmName.SHA256;
        private static char Delimiter = ';';

        public string Hash(string password)
        {
            PasswordNullException(password);

            var salt = RandomNumberGenerator.GetBytes(saltSize);
            var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, _hashAlgorithmName, KeySize);

            return string.Join(Delimiter, Convert.ToBase64String(salt), Convert.ToBase64String(hash));
        }

        public bool Verify(string passwordHash, string inputPassword)
        {
            PasswordNullException(inputPassword);

            var elements = passwordHash.Split(Delimiter);
            var salt = Convert.FromBase64String(elements[0]);
            var hash = Convert.FromBase64String(elements[1]);
            var inputHash = Rfc2898DeriveBytes.Pbkdf2(inputPassword, salt, Iterations, _hashAlgorithmName, KeySize);

            return CryptographicOperations.FixedTimeEquals(hash, inputHash);
        }

        private void PasswordNullException(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("Password cannot be null or empty.", nameof(password));
            }
        }
    }
}
