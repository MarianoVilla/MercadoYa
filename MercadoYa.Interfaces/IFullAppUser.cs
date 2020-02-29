using System;
using System.Collections.Generic;
using System.Text;

namespace MercadoYa.Interfaces
{
    /// <summary>
    /// A mobile app user needs the credentials to be accesible in order to store them locally.
    /// That's not needed nor desirable for other use cases, like the REST services.
    /// Hence we extend the <see cref="IAppUser"/> interface.
    /// </summary>
    public interface IFullAppUser : IAppUser
    {
        string Email { get; set; }
        string Username { get; set; }
        string Password { get; set; }
    }
}
