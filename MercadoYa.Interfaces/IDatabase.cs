﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MercadoYa.Interfaces
{
    public interface IDatabase
    {
        string AddStoreUser(IAppUser User, IUserCredentials Credentials);
        string AddClientUser(IAppUser User, IUserCredentials Credentials);
        IAppUser GetStoreUser(string Uid);
        IAppUser GetClientUser(string Uid);
        IAppUser GetUser(string Uid);
        IAppUser GetUserByEmail(string Email);
        IUserCredentials GetUserCredentials(string Email);

    }
}
