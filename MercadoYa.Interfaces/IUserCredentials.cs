using System;
using System.Collections.Generic;
using System.Text;

namespace MercadoYa.Interfaces
{
    public interface IUserCredentials
    {
        string Email { get; set; }
        string Username { get; set; }
        string Password { get; set; }
        string PasswordSalt { get; set; }
        string HashAlgorithm { get; set; }
        string UserUid { get; set; }
    }
}
