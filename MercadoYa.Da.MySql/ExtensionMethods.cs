using Dapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace MercadoYa.Da.MySql
{
    public static class ExtensionMethods
    {
        public static void AddByReflection(this DynamicParameters Parameters, object Entity)
        {
            foreach (var p in Entity.GetType().GetProperties())
            {
                Parameters.Add(p.Name, p.GetValue(Entity));
            }
        }
    }
}
