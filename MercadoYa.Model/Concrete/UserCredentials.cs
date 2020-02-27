using MercadoYa.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MercadoYa.Model.Concrete
{
    public class UserCredentials : IUserCredentials
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
        public string PasswordSalt { get; set; }
        public string HashAlgorithm { get; set; }
    }
}
