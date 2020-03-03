using MercadoYa.Interfaces;
using MercadoYa.Rest.Logic;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
namespace MercadoYa.Rest.Mock
{
    //ToDo: move to dependencies namespace.
    public class MyAuth : IAuthenticator
    {
        readonly IDatabase Database;
        readonly IMyPasswordHasher Hasher;
        public MyAuth(IDatabase Database, IMyPasswordHasher Hasher)
        {
            this.Database = Database;
            this.Hasher = Hasher;
        }
        //TODO: find a better solution than returning null. Don't return null!
        public object AuthUser(string Email, string Password)
        {
            IUserCredentials Credentials = Database.GetUserCredentials(Email);
            if(Credentials is null || !Hasher.CheckPassword(Credentials.Password, Password))
            {
                return null;
            }
            return Credentials.UserUid;
        }
    }
}
