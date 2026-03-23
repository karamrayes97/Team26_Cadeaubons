using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Cadeaubons_Domain.Security
{
    public static class PasswordHelper
    {
        private const int SaltSize = 16; // 128-bit salt
        private const int KeySize = 32;  // 256-bit hash
        private const int Iterations = 10000;

        // GenerateNumber a new salt
        public static string GenerateSalt()
        {
            using RandomNumberGenerator rng = RandomNumberGenerator.Create();
            byte[] saltBytes = new byte[SaltSize];
            rng.GetBytes(saltBytes);
            return Convert.ToBase64String(saltBytes);
        }

        // Hash a password with a given salt
        public static string HashPassword(string password, string salt)
        {
            byte[] saltBytes = Convert.FromBase64String(salt);
            using var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, Iterations, HashAlgorithmName.SHA256);
            byte[] hash = pbkdf2.GetBytes(KeySize);
            return Convert.ToBase64String(hash);
        }

        public static bool VerifyPassword(string password, string salt, string hash)
        {
            string hashedInput = HashPassword(password, salt);
            return hashedInput == hash;
        }
    }
}
