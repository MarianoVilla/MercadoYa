using Dapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using MercadoYa.Model.Concrete;
using MercadoYa.Interfaces;

namespace MercadoYa.Da.MySql
{
    public static class ExtensionMethods
    {
        public static void AddByReflection(this DynamicParameters Parameters, object Entity)
        {
            foreach (var p in Entity.GetType().GetProperties().Where(x => !Attribute.IsDefined(x, typeof(NotSqlParameterAttribute))))
            {
                Parameters.Add(p.Name, p.GetValue(Entity));
            }
        }
    }
}
