using Konscious.Security.Cryptography;
using System.Security.Cryptography;
using System.Text;

namespace Auth_Service
{
    public class Crypto
    {
        private const int _saltSize = 16;
        private const int _hashSize = 32;
        private const int _threadCount = 8;
        private const int _iterations = 4;
        private const int _memSize = 1024 * 1024;

        public string HashPassword(string pass)
        {
            if (pass == null)
                throw new ArgumentException();

            byte[] salt = new byte[_saltSize];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            byte[] hash = HashPassword(pass, salt);

            var saltHash = new byte[salt.Length + hash.Length];
            Array.Copy(salt, 0, saltHash, 0, salt.Length);
            Array.Copy(hash, 0, saltHash, salt.Length, hash.Length);

            return Convert.ToBase64String(saltHash);
        }

        private byte[] HashPassword(string pass, byte[] salt) {
            var argon2 = new Argon2id(Encoding.UTF8.GetBytes(pass))
            {
                Salt = salt,
                DegreeOfParallelism = _threadCount,
                Iterations = _iterations,
                MemorySize = _memSize
            };

            return argon2.GetBytes(_hashSize);
        }

        public bool VerifyPassword(string pass, string hashedPass)
        {
            byte[] saltHash = Convert.FromBase64String(hashedPass);

            byte[] salt = new byte[_saltSize];
            byte[] hash = new byte[_hashSize];
            Array.Copy(saltHash, 0, salt, 0, _saltSize);
            Array.Copy(saltHash, _saltSize, hash, 0, _hashSize);

            byte[] newHash = HashPassword(pass, salt);

            return CryptographicOperations.FixedTimeEquals(hash, newHash);
        }
    }
}
