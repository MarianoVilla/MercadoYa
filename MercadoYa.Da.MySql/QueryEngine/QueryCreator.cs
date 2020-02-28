using MercadoYa.Lib.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace MercadoYa.Da.MySql.QueryEngine
{
    public class QueryCreator
    {
        static readonly string Users = "users";
        static readonly string StoreUsers = "store_users";
        static readonly string ClientUsers = "client_users";
        static readonly string UserCredentials = "user_credentials";

        public static string SelectStoreUser(string Uid = null)
        {
            if(Uid is null)
            {
                return SelectFrom(StoreUsers);
            }
            return $"{SelectFrom(StoreUsers)} WHERE Uid = @Uid";
        }
        public static string SelectClientUser(string Uid = null)
        {
            if(Uid is null)
            {
                return SelectFrom(ClientUsers);
            }
            return $"{SelectFrom(ClientUsers)} WHERE Uid = @Uid";
        }
        public static string SelectFrom(string Table)
        {
            return $"SELECT * FROM {Table}";
        }
        public static string SelectUserCredentials(string Email)
        {
            return $"{SelectFrom(UserCredentials)} WHERE Email = @Email";
        }
        public static string SelectUserByEmail(string Email)
        {
            return $"{SelectFrom(Users)} WHERE Email = @Email";
        }
        public static string SelectUser(string Uid)
        {
            if (Uid is null)
            {
                return SelectFrom(Users);
            }
            return $"{SelectFrom(Users)} WHERE Uid = @Uid";
        }
    }
}
