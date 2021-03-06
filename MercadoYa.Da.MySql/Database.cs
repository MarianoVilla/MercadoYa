﻿using Dapper;
using GeneralUtil.Model;
using MercadoYa.Da.MySql.QueryEngine;
using MercadoYa.Interfaces;
using MercadoYa.Model.Concrete;
using MercadoYa.Model.CustomDataStructures;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;


namespace MercadoYa.Da.MySql
{
    public class Database : IDatabase
    {
        public readonly string ConnectionString;
        public Database(string ConnectionString) => this.ConnectionString = ConnectionString;

        public string AddStoreUser(IAppUser User, IUserCredentials Credentials) => AddUser(User, Credentials, Const.SpInsertStoreUser);
        public string AddCustomerUser(IAppUser User, IUserCredentials Credentials) => AddUser(User, Credentials, Const.SpInsertCustomerUser);

        string AddUser(IAppUser User, IUserCredentials Credentials, string Stored)
        {                       
            using IDbConnection conn = new MySqlConnection(ConnectionString);

            var Parameters = new DynamicParameters();
            Parameters.AddByReflection(User);
            Parameters.AddByReflection(Credentials);
            Parameters.Add("Uid", dbType: DbType.String, direction: ParameterDirection.Output);

            int Result = conn.Execute(Stored, Parameters, commandType: CommandType.StoredProcedure);
            return Parameters.Get<string>("Uid");
        }

        public IAppUser GetCustomerUser(string Uid) => InnerGetUser(Uid, QueryCreator.SelectClientUser(Uid));
        public IAppUser GetStoreUser(string Uid) => InnerGetUser(Uid, QueryCreator.SelectStoreUser(Uid));
        public IAppUser GetUser(string Uid) => InnerGetUser(Uid, QueryCreator.SelectUser(Uid));

        IAppUser InnerGetUser(string Uid, string Query)
        {
            using IDbConnection conn = new MySqlConnection(ConnectionString);
            return conn.Query<AppUser>(Query, new { Uid }).FirstOrDefault();
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
            IEnumerable<UserCredentials> Result = conn.Query<UserCredentials>(Query, new { Email });
            return Result.FirstOrDefault();
        }
        public IEnumerable<IAppUser> GetNearbyStores(ILocationRequest Request)
        {
            using IDbConnection conn = new MySqlConnection(ConnectionString);
            var Parameters = new DynamicParameters();
            Parameters.AddByReflection(Request);

            var Result = conn.Query<dynamic>(Const.SpGetNearbyStores, Parameters, commandType: CommandType.StoredProcedure)
                .Select(x => new StoreUser()
            {
                Uid = x.Uid,
                Direction = x.Direction,
                City = x.City,
                Longitude = x.Longitude,
                Latitude = x.Latitude,
                ProfilePic = x.ProfilePic,
                Phone = x.Phone,
                DisplayableName = x.DisplayableName,
                RatingScore = x.RatingScore,
                Description = x.Description,
                Lore = x.Lore,
                Schedule = new StoreDaySchedule()
                {
                    DailyIntervals = new List<DaySchedule>()
                    {
                         new DaySchedule() { OpenIntervals = new Interval<DateTime>(new DateTime(x.OpensAt?.Ticks ?? 0), new DateTime(x.ClosesAt?.Ticks ?? 0)),
                         Day = DateTime.Now.DayOfWeek
                    }
                },
                    MightExtendIf = x.MightExtendIf,
                    MightShrinkIf = x.MightShrinkIf
                }
            }).ToList();
            return Result;
        }
        public IEnumerable<IAppUser> GetNearbyStoresIncludingTags(ILocationRequest Request)
        {
            IEnumerable<IAppUser> Stores = GetNearbyStores(Request);
            foreach(var s in Stores)
            {
                s.Tags = GetTagsForUser(s);
            }
            return Stores;
        }
        public IEnumerable<IAppUser> GetNearbyStoresIncludingFoods(ILocationRequest Request)
        {
            var Stores = (IEnumerable<StoreUser>)GetNearbyStores(Request);
            foreach (var s in Stores)
            {
                s.Foods = (IEnumerable<Food>)GetFoodTagsForUser(s);
            }
            return Stores;
        }
        public IEnumerable<ITag> GetTagsForUser(IAppUser User)
        {
            using IDbConnection conn = new MySqlConnection(ConnectionString);

            return conn.Query<Tag>(Const.SpGetTags, new { User.Uid }, commandType: CommandType.StoredProcedure);
        }
        public IEnumerable<ITag> GetFoodTagsForUser(IAppUser User)
        {
            using IDbConnection conn = new MySqlConnection(ConnectionString);

            return conn.Query<Food>(Const.SpGetFoods, new { User.Uid }, commandType: CommandType.StoredProcedure);
        }
    }
}
 