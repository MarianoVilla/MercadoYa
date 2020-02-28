using MercadoYa.Interfaces;
using MercadoYa.Model.Concrete;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace MercadoYa.Rest.Logic
{
    public class HashUtil : IMyPasswordHasher
    {
        IPasswordHasher<object> Hasher;
        public HashUtil()
        {
            this.Hasher = new PasswordHasher<object>();
        }
        public string GenerateSalt(int ByteLength = 16)
        {
            var rng = new RNGCryptoServiceProvider();
            byte[] buffer = new byte[1024];

            rng.GetBytes(buffer);
            return BitConverter.ToString(buffer);
        }
        public bool CheckPassword(string HashedPassword, string UnhashedPassword)
        {
            return Hasher.VerifyHashedPassword(null, HashedPassword, UnhashedPassword) == PasswordVerificationResult.Success;
        }
        public IUserCredentials SecureCredentials(string Email, string Username, string Password)
        {
            string HashedPassword = HashPassword(Password);
            return new UserCredentials()
            {
                Email = Email,
                Username = Username,
                Password = HashedPassword,
                PasswordSalt = string.Empty,
                HashAlgorithm = HashAlgorithmName.SHA256.Name
            };
        }

        public string HashPassword(string Password)
        {
            return Hasher.HashPassword(null, Password);
        }
    }
}
