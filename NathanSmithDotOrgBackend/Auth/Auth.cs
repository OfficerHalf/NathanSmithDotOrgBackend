using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace NathanSmithDotOrgBackend.Auth
{
    public static class Auth
    {
        private static readonly int saltLength = 16;
        private static readonly int hashLength = 20;
        private static readonly int hashPasses = 5000;
        private static readonly RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

        public static string SaltAndHash(string password)
        {
            // 36 Bytes
            // sssssssssssssssshhhhhhhhhhhhhhhhhhhh

            // Get salty
            byte[] salt;
            rng.GetBytes(salt = new byte[saltLength]);

            // Scattered, smothered, chunked
            Rfc2898DeriveBytes hash = new Rfc2898DeriveBytes(password, salt, hashPasses);
            byte[] hashBytes = hash.GetBytes(hashLength);

            // Make em extra crispy
            byte[] combo = new byte[saltLength + hashLength];
            Array.Copy(salt, 0, combo, 0, saltLength);
            Array.Copy(hashBytes, 0, combo, saltLength, hashLength);

            return Convert.ToBase64String(combo);
        }

        public static bool Authenticate(string password)
        {
            string savedHash = "todo";

            // Chop it up, reduce sodium
            byte[] combo = Convert.FromBase64String(savedHash);
            byte[] salt = new byte[saltLength];
            Array.Copy(combo, 0, salt, 0, saltLength);

            // Scattered, smothered, capped
            Rfc2898DeriveBytes hash = new Rfc2898DeriveBytes(password, salt, hashPasses);
            byte[] hashBytes = hash.GetBytes(hashLength);

            // Quality check
            for(int i = 0; i < hashLength; i++)
            {
                if (combo[i+saltLength] != hashBytes[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}
