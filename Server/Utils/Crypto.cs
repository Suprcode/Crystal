using System.Security.Cryptography;
using System.Text;

namespace Server.Utils
{
    class Crypto
    {
        public const int SaltSize = 24;
        public const int HashSize = 24;
        public const int Iterations = 50;
        public static byte[] GenerateSalt()
        {
            var rng = RandomNumberGenerator.Create();
            byte[] salt = new byte[SaltSize];
            rng.GetBytes(salt);

            return salt;
        }

        public static string HashPassword(string password, byte[] salt)
        {
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, HashAlgorithmName.SHA1, HashSize);
            return Encoding.UTF8.GetString(hash);
        }
    }
}
