using System;
using System.Collections.Generic;
using System.Text;

namespace MercadoYa.Interfaces
{
    public interface IUserCredentials
    {
        string Email { get; set; }
        string Password { get; set; }
    }
}
