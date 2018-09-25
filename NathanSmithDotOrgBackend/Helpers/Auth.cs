using NathanSmithDotOrgBackend.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace NathanSmithDotOrgBackend.Auth
{
    public class AuthHelper
    {
        private readonly int saltLength = 16;
        private readonly int hashLength = 20;
        private readonly int hashPasses = 5000;
        private readonly RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
        private readonly DataContext _db;
        
        public AuthHelper(DataContext db)
        {
            _db = db;
        }

        private string SaltAndHash(string password)
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

        private bool Authenticate(string password, string savedHash)
        {
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

        public string CreateAccount(string username, string password)
        {
            // Check to see if the account already exists
            if (_db.Accounts.Any(account => account.Username == username))
                return null;

            string saltyPass = SaltAndHash(password);

            AccountEntity newAccount = new AccountEntity()
            {
                UserId = Guid.NewGuid().ToString(),
                Username = username,
                SaltyHash = saltyPass
            };

            _db.Accounts.Add(newAccount);
            _db.SaveChanges();

            return newAccount.UserId;
        }

        public string Login(string username, string password)
        {
            // Does user exist?
            var user = _db.Accounts.FirstOrDefault(account => account.Username == username);
            if (user == null)
                return null;

            if (Authenticate(password, user.SaltyHash))
            {
                return user.UserId;
            }

            // Bad password
            return null;
        }

        public List<string> ListAccounts()
        {
            var users = _db.Accounts.Select(user => user.Username).ToList();
            return users;
        }
    }
}
