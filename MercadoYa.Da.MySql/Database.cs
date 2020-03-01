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

        public string AddStoreUser(IAppUser User, IUserCredentials Credentials) => AddUser(User, Credentials, Const.SpInsertStoreUser);
        public string AddCustomerUser(IAppUser User, IUserCredentials Credentials) => AddUser(User, Credentials, Const.SpInsertCustomerUser);

        string AddUser(IAppUser User, IUserCredentials Credentials, string Stored)
        {
            using IDbConnection conn = new MySqlConnection(ConnectionString);

            var Parameters = new DynamicParameters();
            Parameters.AddByReflection(User);
            Parameters.AddByReflection(Credentials);
            Parameters.Add("Uid", dbType: DbType.String, direction: ParameterDirection.Output);

            var Result = conn.Execute(Stored, Parameters, commandType: CommandType.StoredProcedure);
            return Parameters.Get<string>("Uid");
        }

        public IAppUser GetCustomerUser(string Uid) => InnerGetUser(Uid, QueryCreator.SelectClientUser(Uid));
        public IAppUser GetStoreUser(string Uid) => InnerGetUser(Uid, QueryCreator.SelectStoreUser(Uid));
        public IAppUser GetUser(string Uid) => InnerGetUser(Uid, QueryCreator.SelectUser(Uid));

        IAppUser InnerGetUser(string Uid, string Query)
        {
            using IDbConnection conn = new MySqlConnection(ConnectionString);
            return conn.Query<AppUser>(Query, new {  Uid }).FirstOrDefault();
        }

        public IAppUser GetUserByEmail(string Email)
        {
            using IDbConnection conn = new MySqlConnection(ConnectionString);
            return conn.Query<AppUser>(QueryCreator.SelectUserByEmail(Email), new { Email }).FirstOrDefault();
        }

        public IUserCredentials GetUserCredentials(string Email)
        {
            if (string.IsNullOrWhiteSpace(Email))
                throw new ArgumentException("Email was empty.");

            string Query = QueryCreator.SelectUserCredentials(Email);

            using IDbConnection conn = new MySqlConnection(ConnectionString);
            var Result = conn.Query<UserCredentials>(Query, new { Email });
            return Result.FirstOrDefault();
        }
    }
}
