using MercadoYa.Lib.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace MercadoYa.Da.MySql.QueryEngine
{
    public class QueryCreator
    {
        public static string SelectStoreUser(string Uid = null)
        {
            if(Uid is null)
            {
                return SelectFrom("storeusers");
            }
            return $"{SelectFrom("storeusers")} WHERE Uid = @Uid";
        }
        public static string SelectClientUser(string Uid = null)
        {
            if(Uid is null)
            {
                return SelectFrom("clientusers");
            }
            return $"{SelectFrom("clientusers")} WHERE Uid = @Uid";
        }
        public static string SelectFrom(string Table)
        {
            return $"SELECT * FROM {Table}";
        }
        public static string SelectUserCredentials(string Email, string Password)
        {
            return $"{SelectFrom("usercredentials")} WHERE Email = @Email AND Password = @Password";
        }
    }
}
