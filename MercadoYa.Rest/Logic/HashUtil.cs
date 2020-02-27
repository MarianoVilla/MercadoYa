using MercadoYa.Interfaces;
using MercadoYa.Model.Concrete;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace MercadoYa.Rest.Logic
{
    public class HashUtil : IMyPasswordHasher
    {
        IPasswordHasher<HashUtil> Hasher;
        public HashUtil()
        {
            this.Hasher = new PasswordHasher<HashUtil>();
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
            return Hasher.VerifyHashedPassword(this, HashedPassword, UnhashedPassword) == PasswordVerificationResult.Success;
        }
        public IUserCredentials GenerateCredentials(string Email, string Username, string Password)
        {
            string Salt = GenerateSalt();
            string SaltedPassword = Password + Salt;
            string HashedPassword = HashPassword(SaltedPassword);
            return new UserCredentials()
            {
                Email = Email,
                Username = Username,
                Password = HashedPassword,
                PasswordSalt = Salt,
                HashAlgorithm = HashAlgorithmName.SHA256.Name
            };
        }

        public string HashPassword(string Password)
        {
            return Hasher.HashPassword(this, Password);
        }
    }
}
