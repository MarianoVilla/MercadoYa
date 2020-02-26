using System;
using System.Collections.Generic;
using System.Text;

namespace MercadoYa.Interfaces
{
    public interface IValidator
    {
        bool IsValidEmail(string Email);
        bool IsValidName(string Name);
        bool IsValidPassword(string Password);
        bool IsValidPhone(string Phone);
        string ValidateEmail(string Email);
        string ValidateName(string Name);
        string ValidatePassword(string Password);
        string ValidatePhone(string Phone);
    }
}
