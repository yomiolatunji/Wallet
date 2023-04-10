using System.Security.Cryptography;

namespace YomiOlatunji.Wallet.BusinessCore.Helpers
{
    public static class PasswordHelper
    {
        private const int SaltSize = 16;
        private const int KeySize = 32;

        public static string HashPassword(string password)
        {
            // Generate a random salt
            var salt = new byte[SaltSize];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }

            // Generate a key from the password and salt
            var key = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA256).GetBytes(KeySize);

            // Combine the salt and key into a single byte array
            var hashBytes = new byte[SaltSize + KeySize];
            Buffer.BlockCopy(salt, 0, hashBytes, 0, SaltSize);
            Buffer.BlockCopy(key, 0, hashBytes, SaltSize, KeySize);

            // Convert the byte array to a base64-encoded string
            return Convert.ToBase64String(hashBytes);
        }

        public static bool VerifyPassword(string password, string hashedPassword)
        {
            // Convert the base64-encoded string back to a byte array
            var hashBytes = Convert.FromBase64String(hashedPassword);

            // Extract the salt and key from the byte array
            var salt = new byte[SaltSize];
            Buffer.BlockCopy(hashBytes, 0, salt, 0, SaltSize);
            var key = new byte[KeySize];
            Buffer.BlockCopy(hashBytes, SaltSize, key, 0, KeySize);

            // Generate a key from the incoming password and stored salt
            var derivedKey = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA256).GetBytes(KeySize);

            // Compare the generated key to the stored key
            return derivedKey.SequenceEqual(key);
        }
    }
}