using MercadoYa.Interfaces;
using MercadoYa.Rest.Logic;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace MercadoYa.Rest.Mock
{
    public class MyAuth : IAuth
    {
        readonly IDatabase Database;
        readonly IMyPasswordHasher Hasher;
        public MyAuth(IDatabase Database, IMyPasswordHasher Hasher)
        {
            this.Database = Database;
            this.Hasher = Hasher;
        }

        public object AuthUser(string Email, string Password)
        {
            IUserCredentials Credentials = Database.GetUserCredentials(Email);
            if(Hasher.CheckPassword(Credentials.Password, Password))
            {
                return Credentials.UserUid;
            }
            return null;
        }
    }
}
