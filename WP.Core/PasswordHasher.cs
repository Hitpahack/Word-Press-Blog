using System;
using System.Security.Cryptography;
using System.Text;

namespace WP.Core
{
    public static class PasswordHasher
    {
        private const int SaltLength = 8; // WordPress default salt length
        private const int StretchingRounds = 10000; // WordPress default stretching rounds

        public static string HashPassword(string password)
        {
            // Step 1: Generate a random salt
            var salt = GenerateSalt(SaltLength);

            // Step 2: Create the base MD5 hash
            var md5Hash = GetMd5Hash(password + salt);

            // Step 3: Stretch the hash (WordPress does this 10000 times)
            var stretchedHash = StretchHash(md5Hash, salt);

            // Step 4: Return the hash in the WordPress format
            return $"$P${salt}{stretchedHash}";
        }

        public static bool VerifyPassword(string password, string storedHash)
        {
            // Step 1: Extract the salt from the stored hash (Salt is the part between $P$ and the hashed password)
            var salt = storedHash.Substring(3, SaltLength);

            // Step 2: Hash the input password with the same salt
            var md5Hash = GetMd5Hash(password + salt);

            // Step 3: Stretch the hash
            var stretchedHash = StretchHash(md5Hash, salt);

            // Step 4: Check if the new hash matches the stored hash
            return storedHash == $"$P${salt}{stretchedHash}";
        }

        private static string GenerateSalt(int length)
        {
            var rng = new RNGCryptoServiceProvider();
            var saltBytes = new byte[length];
            rng.GetBytes(saltBytes);
            return Convert.ToBase64String(saltBytes).Substring(0, length);  // Trim to the specified length
        }

        private static string GetMd5Hash(string input)
        {
            using (var md5 = MD5.Create())
            {
                var hashBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
                return Convert.ToBase64String(hashBytes);
            }
        }

        private static string StretchHash(string hash, string salt)
        {
            // WordPress applies MD5 10000 times to the password hash
            var result = hash;

            for (int i = 0; i < StretchingRounds; i++)
            {
                result = GetMd5Hash(result + salt);
            }

            return result;
        }
    }
}
