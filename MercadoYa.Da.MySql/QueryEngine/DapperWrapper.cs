using Dapper;
using MercadoYa.Lib.Util;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace MercadoYa.Da.MySql.QueryEngine
{
    public class DapperWrapper
    {
        public static IEnumerable<T> MapEntity<T>(T Entity, dynamic DapperResult)
        {
            var Output = new List<T>();
            var PropsOfT = Entity.GetType().GetProperties();
            foreach (var row in DapperResult)
            {
                T Ent = (T)Activator.CreateInstance(Entity.GetType());
                foreach (var p in PropsOfT)
                {
                    var data = (IDictionary<string, object>)row;
                    var DataInRow = data[p.Name];
                    if (DataInRow == null)
                        continue;
                    var TypedValue = EntitiesUtil.ChangeType(DataInRow, p.PropertyType);
                    p.SetValue(Ent, TypedValue, null);
                }
                Output.Add(Ent);
            }
            return Output;
        }
        public static IEnumerable<dynamic> Query(string QueryString, string ConnectionString)
        {
            using IDbConnection db = new MySqlConnection(ConnectionString);
            return db.Query(QueryString);
        }
        public static IEnumerable<dynamic> Query(string QueryString, string ConnectionString, DynamicParameters Params)
        {
            using IDbConnection db = new MySqlConnection(ConnectionString);
            return db.Query(QueryString, Params);
        }
    }
}
