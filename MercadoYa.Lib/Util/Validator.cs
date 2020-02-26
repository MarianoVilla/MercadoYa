using MercadoYa.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MercadoYa.Lib.Util
{
    public class Validator : IValidator
    {
        //TODO: extract this fields into an interface that abstracts the idea of validation messages, to handle regional stuff.
        static string InvalidEmailMessage = "El correo electrónico es inválido.\n";
        static string InvalidPhoneMessage = "El número es inválido.\n";
        static string InvalidNameMessage = "El nombre es inválido.\n";
        static string InvalidPasswordMessage = "La contraseña debe tener al menos 8 caracteres, un número y una mayúscula.\n";
        public bool IsValidEmail(string Email)
        {
            try
            {
                return new System.Net.Mail.MailAddress(Email).Address == Email;
            }
            catch
            {
                return false;
            }
        }
        public string ValidateEmail(string Email)
        {
            return IsValidEmail(Email) ? string.Empty : InvalidEmailMessage;
        }
        public bool IsValidPhone(string Phone)
        {
            if (string.IsNullOrWhiteSpace(Phone))
                return false;
            return !string.IsNullOrWhiteSpace(Phone) && Phone.RemoveNonNumeric().Length >= 10;
        }
        public string ValidatePhone(string Phone)
        {
            return IsValidPhone(Phone) ? string.Empty : InvalidPhoneMessage;
        }
        public bool IsValidPassword(string Password)
        {
            if (string.IsNullOrWhiteSpace(Password))
                return false;
            return Password.HasAtLeastNChars(8)
                && Password.HasNumber()
                && Password.HasUpperChar();
        }
        public string ValidatePassword(string Password)
        {
            return IsValidPassword(Password) ? string.Empty : InvalidPasswordMessage;
        }
        public bool IsValidName(string Name)
        {
            return !string.IsNullOrWhiteSpace(Name);
        }
        public string ValidateName(string Name)
        {
            return IsValidName(Name) ? string.Empty : InvalidNameMessage;
        }
    }
}