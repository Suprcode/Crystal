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
            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA1);
            return Encoding.UTF8.GetString(pbkdf2.GetBytes(HashSize));
        }
    }
}
