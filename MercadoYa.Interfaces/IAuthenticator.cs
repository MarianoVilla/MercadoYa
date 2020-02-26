using System;
using System.Collections.Generic;
using System.Text;

namespace MercadoYa.Interfaces
{
    public interface IAuth
    {
        bool AuthUser(string Email, string Password);
    }
}
