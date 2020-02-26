using System;
using System.Collections.Generic;
using System.Text;

namespace MercadoYa.Interfaces
{
    public interface IDatabase
    {
        string AddStoreUser(IAppUser User);
        string AddClientUser(IAppUser User);
        IAppUser GetStoreUser(string Uid);
        IAppUser GetClientUser(string Uid);
        IUserCredentials GetUserCredentials(string Email, string Password);

    }
}
