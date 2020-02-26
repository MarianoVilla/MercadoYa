using MercadoYa.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MercadoYa.Rest.Mock
{
    public class MockAuth : IAuth
    {
        readonly IDatabase Database;
        public MockAuth(IDatabase Database)
        {
            this.Database = Database;
        }

        public bool AuthUser(string Email, string Password)
        {
            return Database.GetUserCredentials(Email, Password) != null;
        }
    }
}
