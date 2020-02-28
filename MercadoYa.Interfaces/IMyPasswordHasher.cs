using System;
using System.Collections.Generic;
using System.Text;

namespace MercadoYa.Interfaces
{
    public interface IMyPasswordHasher
    {
        string GenerateSalt(int ByteLength = 16);
        string HashPassword(string Password);
        bool CheckPassword(string HashedPassword, string ProvidedPassword);
        IUserCredentials SecureCredentials(string Email, string Username, string Password);
    }
}
