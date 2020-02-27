using MercadoYa.Interfaces;
using MercadoYa.Rest.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MercadoYa.Rest.Mock
{
    public class MockAuth : IAuth
    {
        readonly IDatabase Database;
        readonly IMyPasswordHasher Hasher;
        public MockAuth(IDatabase Database, IMyPasswordHasher Hasher)
        {
            this.Database = Database;
            this.Hasher = Hasher;
        }

        public bool AuthUser(string Email, string Password)
        {
            IUserCredentials Credentials = Database.GetUserCredentials(Email);
            return Hasher.CheckPassword(Password, Credentials.Password);
        }
    }
}
