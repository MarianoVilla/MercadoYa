using MercadoYa.Interfaces;
using MercadoYa.Lib.Util;
using MercadoYa.Model.Concrete;
using MercadoYa.Model.Enums;
using MercadoYa.Rest.Mock;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace MercadoYa.Rest.Controllers
{
    //@ToDo Add an action for store schedule.
    [ApiController]
    [Route("[controller]")]
    public class DatabaseController : ControllerBase
    {
        readonly ILogger<DatabaseController> Logger;
        readonly IDatabase Database;
        public DatabaseController(ILogger<DatabaseController> logger, IDatabase Database)
        {
            this.Logger = logger;
            this.Database = Database;
        }
        [Route("test")]
        public IActionResult Test()
        {
            return Ok();
        }
        [Route("users")]
        public IActionResult GetUserInfo(string Uid)
        {
            if (IsUnauthorized())
                return Unauthorized();

            IAppUser User = Database.GetUser(Uid);
            return new JsonResult(User);
        }
        [Route("nearbystores")]
        [HttpPost]
        public IActionResult GetNearbyStores([FromBody] LocationRequest LocRequest, NestedTagsPolicy NestedTags = NestedTagsPolicy.None)
        {

            if (IsUnauthorized())
                return Unauthorized();

            var Stores = (IEnumerable<StoreUser>)Database.GetNearbyStores(LocRequest);

            SwitchNestedIncludeType(Stores, NestedTags);

            return new JsonResult(Stores);
        }
        void SwitchNestedIncludeType(IEnumerable<StoreUser> Stores, NestedTagsPolicy NestedPolicy)
        {
            switch (NestedPolicy)
            {
                case NestedTagsPolicy.All: LoadAllTagsInStores(Stores); break;
                case NestedTagsPolicy.Foods: Stores.LoadFoods(Database); break;
                case NestedTagsPolicy.Tags: Stores.LoadTags(Database); break;
                case NestedTagsPolicy.None:
                default: break;
            }
        }
        void LoadAllTagsInStores(IEnumerable<StoreUser> Stores)
        {
            Stores.LoadFoods(Database);
            Stores.LoadTags(Database);
        }
        bool IsUnauthorized()
        {
#if DEBUG
            return false;
#endif
            string Timestamp = Request.Cookies["timestamp"] as string;
            return !ValidTimestamp(Timestamp);
        }
        bool ValidTimestamp(string Timestamp)
        {
            if (Timestamp is null)
                return false;
            Timestamp = CryptoUtil.DecryptString(Const.CryptoDevKey, Timestamp);
            if (!DateTime.TryParseExact(Timestamp, "yyyyMMddHHmmssFFF", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime Result))
                return false;
            return (DateTime.Now - Result).TotalDays < 1;
        }

    }
}