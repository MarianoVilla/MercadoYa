using System;
using System.Collections.Generic;
using System.Text;

namespace MercadoYa.Interfaces
{
    public interface IAuthenticator
    {
        object AuthUser(string Email, string Password);
    }
}
