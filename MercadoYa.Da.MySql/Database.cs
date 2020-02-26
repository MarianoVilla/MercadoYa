using MercadoYa.Da.MySql.QueryEngine;
using MercadoYa.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using System.Data;
using MySql.Data.MySqlClient;
using MercadoYa.Model.Concrete;
using System.Linq;
using MercadoYa.Lib.Util;

namespace MercadoYa.Da.MySql
{
    public class Database : IDatabase
    {
        public readonly string ConnectionString;
        public Database(string ConnectionString)
        {
            this.ConnectionString = ConnectionString;
        }

        public string AddStoreUser(IAppUser User) => AddUser(User, Const.SpInsertStoreUser);
        public string AddClientUser(IAppUser User) => AddUser(User, Const.SpInsertClientUser);

        string AddUser(IAppUser User, string Stored)
        {
            using IDbConnection conn = new MySqlConnection(ConnectionString);
            DynamicParameters Parameters = ReflectParameters(User);
            Parameters.Add("Uid", dbType: DbType.String, direction: ParameterDirection.Output);
            var Result = conn.Execute(Stored, Parameters, commandType: CommandType.StoredProcedure);
            return Parameters.Get<string>("Uid");
        }

        public IAppUser GetClientUser(string Uid) => GetUser(Uid, QueryCreator.SelectClientUser(Uid));
        public IAppUser GetStoreUser(string Uid) => GetUser(Uid, QueryCreator.SelectStoreUser(Uid));

        IAppUser GetUser(string Uid, string Query)
        {
            using IDbConnection conn = new MySqlConnection(ConnectionString);
            return conn.Query<IAppUser>(Query, new { Uid }).FirstOrDefault();
        }

        public IUserCredentials GetUserCredentials(string Email, string Password)
        {
            if (StringUtil.AnyNullOfWhiteSpace(Email, Password))
                throw new ArgumentException("Email or password were empty.");

            var Parameters = new DynamicParameters();
            Parameters.Add(nameof(Email), Email);
            Parameters.Add(nameof(Password), Password);

            using IDbConnection conn = new MySqlConnection(ConnectionString);
            return conn.Query<UserCredentials>(QueryCreator.SelectUserCredentials(Email, Password), Parameters).FirstOrDefault();
        }


        #region Helpers.
        DynamicParameters ReflectParameters(object Entity)
        {
            var Parameters = new DynamicParameters();
            foreach (var p in Entity.GetType().GetProperties())
            {
                Parameters.Add(p.Name, p.GetValue(Entity));
            }
            return Parameters;
        }
        #endregion


    }
}
